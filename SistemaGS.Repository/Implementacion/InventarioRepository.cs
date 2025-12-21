using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.DBContext;

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
                            default:
                                {
                                    throw new InvalidOperationException("No existe esa transacción de inventario");
                                }
                        }
                    }

                    ayuda.Estado = itemsAyuda.Any(i => i.CantidadEntregada > 0) ? "Lista Para Entregar" : "En Proceso";

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
        public async Task<List<Item>> ListarInventario(ItemQuery query)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var consulta = from i in _dbContext.Items
                                   where
                                    (query.ID == 0 || i.IdItem == query.ID) &&
                                    (string.IsNullOrEmpty(query.Nombre) || EF.Functions.Like((i.Nombre ?? "").ToLower(), query.Nombre.ToLower())) &&
                                    (string.IsNullOrEmpty(query.Categoria) || i.Categoria == query.Categoria) &&
                                    (string.IsNullOrEmpty(query.Unidad) || i.Unidad == query.Unidad)
                                   select i;
                    return await consulta.ToListAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
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
                                    if (movimiento.Unidad != item.Unidad) throw new InvalidOperationException("Las unidades no son iguales, revise nuevamente");

                                    await _dbContext.Items.AddAsync(new Item()
                                    {
                                        IdItem = 0,
                                        Nombre = item.Nombre,
                                        Categoria = item.Categoria,
                                        Descripcion = item.Descripcion,
                                        Unidad = item.Unidad,
                                        Cantidad = movimiento.Cantidad
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
                                if(!await _dbContext.Items.AnyAsync(i => i.IdItem == movimiento.Item)) throw new InvalidOperationException("Operación inválida, no puede deolver un ítem inexistente");

                                var inventario = await _dbContext.Items.Where(i => i.IdItem == movimiento.Item).FirstAsync();
                                if (inventario.Unidad != movimiento.Unidad || inventario.Cantidad < movimiento.Cantidad) throw new InvalidOperationException("Operación inválida, revise nuevamente");
                                else inventario.Cantidad -= movimiento.Cantidad;
                                
                                _dbContext.Items.Update(inventario);
                                await _dbContext.SaveChangesAsync();

                                break;
                            }
                        default:
                            {
                                throw new InvalidOperationException("No existe esa transacción de inventario");
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
    }
}
