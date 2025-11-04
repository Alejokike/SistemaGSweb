using Microsoft.EntityFrameworkCore;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.DBContext;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace SistemaGS.Repository.Implementacion
{
    public class InventarioRepository : GenericoRepository<Inventario>, IInventarioRepository
    {
        private readonly DbsistemaGsContext _dbContext;

        public InventarioRepository(DbsistemaGsContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<bool> Desbloquear(List<Inventario> movimientos, int IdAyuda)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Ayuda ayuda = await _dbContext.Ayuda.Where(a => a.IdAyuda == IdAyuda).FirstAsync();
                    
                    if (ayuda == null) throw new TaskCanceledException("La ayuda seleccionada no existe");

                    List<ListaItemDTO> itemsAyuda = JsonConvert.DeserializeObject<List<ListaItemDTO>>(ayuda.ListaItems!)!;
                    if (itemsAyuda.IsNullOrEmpty()) throw new TaskCanceledException("La lista de items esta vacía");

                    foreach (var movimiento in movimientos)
                    {
                        Item item = await _dbContext.Items.Where(i => i.IdItem == movimiento.Item).FirstAsync();

                        switch (movimiento.TipoOperacion)
                        {
                            case "ASI":
                                {
                                    if (item.Cantidad < movimiento.Cantidad) throw new InvalidOperationException("No hay stock para hacer este movimiento");

                                    var ItemLista = itemsAyuda.FirstOrDefault(ia => ia.ItemLista.IdItem == item.IdItem);

                                    if (ItemLista == null || ItemLista.CantidadSolicitada < movimiento.Cantidad) throw new InvalidOperationException("Operación inválida");

                                    item.Cantidad -= movimiento.Cantidad;
                                    ItemLista.CantidadEntregada += movimiento.Cantidad;

                                    _dbContext.Items.Update(item);

                                    break;
                                }
                            case "RET":
                                {
                                    var ItemLista = itemsAyuda.FirstOrDefault(ia => ia.ItemLista.IdItem == item.IdItem) ;

                                    if (ItemLista!.CantidadEntregada < movimiento.Cantidad) throw new InvalidOperationException("La ayuda no tiene ese stock asignado");

                                    item.Cantidad += movimiento.Cantidad;
                                    ItemLista.CantidadEntregada -= movimiento.Cantidad;

                                    _dbContext.Items.Update(item);

                                    break;
                                }
                        }
                    }

                    ayuda.ListaItems = JsonConvert.SerializeObject(itemsAyuda);
                    _dbContext.Ayuda.Update(ayuda);
                    await _dbContext.SaveChangesAsync();
                    
                    await _dbContext.Inventarios.AddRangeAsync(movimientos);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return true;
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return false;
                    throw;
                }
            }
        }
        public async Task<bool> Registrar(Inventario movimiento, Item item)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    switch(movimiento.TipoOperacion)
                    {
                        case "REC":
                            {
                                if (!await _dbContext.Items.AnyAsync(i => i.IdItem == movimiento.Item))
                                {
                                    await _dbContext.Items.AddAsync(new Item()
                                    {
                                        IdItem = 0,
                                        Nombre = item.Nombre,
                                        Categoria = item.Categoria,
                                        Descripcion = item.Descripcion,
                                        Unidad = item.Unidad,
                                        Cantidad = 0
                                    });
                                    await _dbContext.SaveChangesAsync();

                                    movimiento.Item = await _dbContext.Items.MaxAsync(i => i.IdItem);
                                }
                                else
                                {
                                    var inventario = await _dbContext.Items.Where(i => i.IdItem == movimiento.Item).FirstAsync();
                                    if (inventario.Unidad == movimiento.Unidad) inventario.Cantidad += movimiento.Cantidad;
                                    else throw new InvalidOperationException("Las unidades no son iguales, revise nuevamente");

                                    _dbContext.Items.Update(inventario);
                                    await _dbContext.SaveChangesAsync();
                                }
                                break;
                            }
                        case "DEV":
                            {
                                var inventario = await _dbContext.Items.Where(i => i.IdItem == movimiento.Item).FirstAsync();
                                if (inventario.Unidad == movimiento.Unidad || inventario.Cantidad > movimiento.Cantidad) inventario.Cantidad -= movimiento.Cantidad;
                                else throw new InvalidOperationException("Operación inválida, revise nuevamente");

                                _dbContext.Items.Update(inventario);
                                await _dbContext.SaveChangesAsync();

                                break;
                            }
                    }
                    

                    await _dbContext.Inventarios.AddAsync(movimiento);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();

                    return true;
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return false;
                    throw;
                }
            }
        }
        public class ItemDTO
        {
            public int IdItem { get; set; }
            public string Nombre { get; set; } = null!;
            public string? Categoria { get; set; }
            public string Descripcion { get; set; } = null!;
            public string? Unidad { get; set; }
        }
        public class ListaItemDTO
        {
            public int IdLista { get; set; }
            public ItemDTO ItemLista { get; set; } = null!;
            public decimal CantidadSolicitada { get; set; }
            public decimal? CantidadEntregada { get; set; }
        }
    }
}
