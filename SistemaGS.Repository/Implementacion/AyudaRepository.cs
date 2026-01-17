using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO;
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

                    IQueryable<Ayuda>? consulta;

                    if (string.IsNullOrEmpty(datasoli) && string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (filtro.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula)

                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Detalle = a.Detalle,
                                       ListaItems = a.ListaItems,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (filtro.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataSoli) ||
                                        EF.Functions.Like((s.Nombre ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.Apellido ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoTrabajo ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoHabitacion ?? "").ToLower(), $"%{datasoli}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datasoli))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (filtro.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataFunci) ||
                                        EF.Functions.Like((f.Nombre ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.Apellido ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoTrabajo ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoHabitacion ?? "").ToLower(), $"%{datafunci}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (filtro.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= filtro.FechaFin) &&

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
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }

                    return await consulta.AsNoTracking().ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<List<Ayuda>> ListarCerradas(AyudaQuery filtro)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string datasoli = (filtro.DataSoli ?? "").ToLower();
                    string datafunci = (filtro.DataFunci ?? "").ToLower();
                    string[] estados = (filtro.Estado ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);

                    /*
                    var consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

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
                    
                    return await consulta.AsNoTracking().Select(a => a.Ayuda).ToListAsync();
                    */

                    IQueryable<Ayuda>? consulta;

                    if (string.IsNullOrEmpty(datasoli) && string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula)

                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataSoli) ||
                                        EF.Functions.Like((s.Nombre ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.Apellido ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoTrabajo ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoHabitacion ?? "").ToLower(), $"%{datasoli}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datasoli))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataFunci) ||
                                        EF.Functions.Like((f.Nombre ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.Apellido ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoTrabajo ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoHabitacion ?? "").ToLower(), $"%{datafunci}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

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
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }

                    return await consulta.AsNoTracking().ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<List<Ayuda>> ListarImpresion(AyudaQuery filtro)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    string datasoli = (filtro.DataSoli ?? "").ToLower();
                    string datafunci = (filtro.DataFunci ?? "").ToLower();
                    string[] estados = (filtro.Estado ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);

                    IQueryable<Ayuda>? consulta;

                    if (string.IsNullOrEmpty(datasoli) && string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula)

                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       ListaItems = a.ListaItems,
                                       Detalle = a.Detalle,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datafunci))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataSoli) ||
                                        EF.Functions.Like((s.Nombre ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.Apellido ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoTrabajo ?? "").ToLower(), $"%{datasoli}%") ||
                                        EF.Functions.Like((s.TelefonoHabitacion ?? "").ToLower(), $"%{datasoli}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       ListaItems = a.ListaItems,
                                       Detalle = a.Detalle,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else if (string.IsNullOrEmpty(datasoli))
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

                                    (estados.Length == 0 || estados.Contains(a.Estado)) &&

                                    (string.IsNullOrEmpty(filtro.categoria) || a.Categoria == filtro.categoria) &&

                                    (filtro.solicitante == null || filtro.solicitante == s.Cedula) &&

                                    (filtro.funcionario == null || filtro.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(filtro.DataFunci) ||
                                        EF.Functions.Like((f.Nombre ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.Apellido ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoTrabajo ?? "").ToLower(), $"%{datafunci}%") ||
                                        EF.Functions.Like((f.TelefonoHabitacion ?? "").ToLower(), $"%{datafunci}%")
                                    )
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       ListaItems = a.ListaItems,
                                       Detalle = a.Detalle,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }
                    else
                    {
                        consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula into funcGroup
                                   from f in funcGroup.DefaultIfEmpty()
                                   where
                                    (a.FechaEntrega != null && filtro.FechaIni <= a.FechaEntrega && a.FechaEntrega <= filtro.FechaFin) &&

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
                                   select new Ayuda()
                                   {
                                       IdAyuda = a.IdAyuda,
                                       Solicitante = a.Solicitante,
                                       Funcionario = a.Funcionario,
                                       Categoria = a.Categoria,
                                       ListaItems = a.ListaItems,
                                       Detalle = a.Detalle,
                                       Estado = a.Estado,
                                       FechaSolicitud = a.FechaSolicitud,
                                       FechaEntrega = a.FechaEntrega
                                   };
                    }

                    return await consulta.AsNoTracking().ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
        public async Task<DashboardDTO> Dashboard()
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    DashboardDTO d = new DashboardDTO();

                    DateTime iniCM = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime finCM = iniCM.AddMonths(1).AddDays(-1);

                    DateTime iniLM = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                    DateTime finLM = iniLM.AddMonths(1).AddDays(-1);

                    long year = iniCM.Year;
                    long lastyear = year - 1;

                    d.AyudasCM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniCM && a.FechaEntrega.Value <= finCM
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();
                    d.AyudasLM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniLM && a.FechaEntrega.Value <= finLM
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();

                    d.AyudasCY = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value.Year == year
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();
                    d.AyudasLY = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value.Year == lastyear
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();

                    d.PersonasCM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniLM && a.FechaEntrega.Value <= finLM
                                 select new
                                 {
                                     id = a.Solicitante
                                 }
                                 ).AsNoTracking().Distinct().CountAsync();
                    d.PersonasLM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Cerrada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniLM && a.FechaEntrega.Value <= finLM
                                 select new
                                 {
                                     id = a.Solicitante
                                 }
                                 ).AsNoTracking().Distinct().CountAsync();

                    d.RechazadasCM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Rechazada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniCM && a.FechaEntrega.Value <= finCM
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();
                    d.RechazadasLM = await (
                                 from a in _dbContext.Ayuda
                                 where
                                 a.Estado == "Rechazada" &&
                                 a.FechaEntrega.HasValue && a.FechaEntrega.Value >= iniLM && a.FechaEntrega.Value <= finLM
                                 select new
                                 {
                                     id = a.IdAyuda
                                 }
                                 ).AsNoTracking().CountAsync();

                    return d;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}