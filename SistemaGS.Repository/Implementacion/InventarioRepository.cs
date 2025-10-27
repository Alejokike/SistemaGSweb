using SistemaGS.Model;
using SistemaGS.Repository.DBContext;
using SistemaGS.Repository.Contrato;
using System.Text.Json;

namespace SistemaGS.Repository.Implementacion
{
    public class InventarioRepository : GenericoRepository<Inventario>, IInventarioRepository
    {
        private readonly DbsistemaGsContext _dbContext;

        public InventarioRepository(DbsistemaGsContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /*
        public async Task<Ayuda> Desbloqeuar(List<Inventario> movimientos, Ayuda ayuda)
        {
            Inventario operacion = new Inventario();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Ayuda ayudaModificada = _dbContext.Ayuda.Where(a => a.IdAyuda == ayuda.IdAyuda).First();

                    if (ayudaModificada == null) throw new TaskCanceledException("La ayuda seleccionada no existe");

                    movimientos = movimientos.OrderBy(i => i.Item).ToList();

                    List<Item> ListaItems = JsonSerializer.Deserialize<List<Item>>(ayudaModificada.ListaItems!)!;
                    ListaItems = ListaItems.OrderBy(i => i.IdItem).ToList();

                    movimientos.OrderBy

                    foreach (var movimiento in movimientos)
                    {
                        Item item = _dbContext.Items.Where(i => i.IdItem == movimiento.Item).First();
                        switch (model.TipoOperacion)
                        {
                            case "ASI":
                                {
                                    item.Cantidad += model.Cantidad;
                                    break;
                                }
                            case "RET":
                                {
                                    if (item.Cantidad > model.Cantidad) item.Cantidad -= model.Cantidad;

                                    break;
                                }
                        }
                    }

                    _dbContext.Items.Update(item);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Inventarios.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    operacion = model;

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                return operacion;
            }
        }
        */
        public async Task<Inventario> Registrar(Inventario model)
        {
            Inventario operacion = new Inventario();
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Item item = _dbContext.Items.Where(i => i.IdItem == model.Item).First();

                    switch(model.TipoOperacion)
                    {
                        case "REC":
                            {
                                item.Cantidad += model.Cantidad;
                                break;
                            }
                        case "DEV":
                            {
                                if (item.Cantidad > model.Cantidad) item.Cantidad -= model.Cantidad;
                                
                                    break;
                            }
                    }
                    _dbContext.Items.Update(item);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Inventarios.AddAsync(model);
                    await _dbContext.SaveChangesAsync();

                    operacion = model;

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                return operacion;
            }
        }
    }
}
