using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.DBContext;

namespace SistemaGS.Repository.Implementacion
{
    public class AyudaRepository : GenericoRepository<Ayuda>, IAyudaRepository
    {
        private readonly DbsistemaGsContext _dbContext;

        public AyudaRepository(DbsistemaGsContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> MasiveAttack(List<Ayuda> ayudas)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _dbContext.Ayuda.AddRangeAsync(ayudas);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<List<Ayuda>> Listar(AyudaQuery filtro)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string datasoli = (filtro.DataSoli ?? "").ToLower();
                    string datafunci = (filtro.DataFunci ?? "").ToLower();
                    string[] estados = (filtro.Estado ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);

                    var consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    filtro.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= filtro.FechaFin &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&
                                     
                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataSoli) ||
                                        EF.Functions.Like((s.Nombre ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.Apellido ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoTrabajo ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoHabitacion ?? "").ToLower(), $"%{datasoli}%")
                                    ) &&

                                    (string.IsNullOrEmpty(filtro.DataFunci) ||
                                        EF.Functions.Like((f.Nombre ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.Apellido ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoTrabajo ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoHabitacion ?? "").ToLower(), $"%{datafunci}%")
                                    )
                                   select new
                                   {
                                       Ayuda = a,
                                       Solicitante = s,
                                       Funcionario = f
                                   };

                    transaction.Commit();
                    return await consulta.Select(a => a.Ayuda).ToListAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
