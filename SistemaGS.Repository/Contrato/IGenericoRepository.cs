using System.Linq.Expressions;

namespace SistemaGS.Repository.Contrato
{
    public interface IGenericoRepository<TModel> where TModel : class
    {
        IQueryable<TModel> Consultar(Expression<Func<TModel, bool>>? filtro = null);
        Task<TModel> Crear(TModel model);
        Task<bool> Editar(TModel model);
        Task<bool> Eliminar(TModel model);
    }
}
