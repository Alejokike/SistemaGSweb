using SistemaGS.Model;
using SistemaGS.Repository.DBContext;
using SistemaGS.Repository.Contrato;
//Estas clases específicas son para cuando al hacer una actualización de una tabla afectamos a muchas tablas, así mantenemos la atomicidad

namespace SistemaGS.Repository.Implementacion
{
    public class PlanillaRepository : GenericoRepository<Planilla>, IPlanillaRepository
    {
        private readonly DbsistemaGsContext _dbContext;
        public PlanillaRepository(DbsistemaGsContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Planilla> Registrar(Planilla model)
        {
            Planilla planillaGenerada = new Planilla();
            /*
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.ListaItems)
                    {
                        ListaItem Match = _dbContext.ListaItems.Where(m => m.IdPlanilla == item.IdPlanilla).First();
                        _dbContext.ListaItems.Update(Match);
                    }
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Planillas.AddAsync(model);
                    planillaGenerada = model;
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            */
            return planillaGenerada;
        }
    }
}
