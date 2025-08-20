using System.Linq.Expressions;
using SistemaGS.Repository.DBContext;
using SistemaGS.Repository.Contrato;

namespace SistemaGS.Repository.Implementacion
{
    public class GenericoRepository<TModel> : IGenericoRepository<TModel> where TModel : class
    {
        private readonly DbsistemaGsContext _dbContext;

        public GenericoRepository(DbsistemaGsContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IQueryable<TModel> Consultar(Expression<Func<TModel, bool>>? filtro = null)
        {
            IQueryable<TModel> consulta = (filtro == null) ? _dbContext.Set<TModel>() : _dbContext.Set<TModel>().Where(filtro);
            return consulta;
        }

        public async Task<TModel> Crear(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Add(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Update(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(TModel model)
        {
            try
            {
                _dbContext.Set<TModel>().Remove(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
