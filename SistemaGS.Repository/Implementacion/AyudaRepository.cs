using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task<bool> CambiarEstado(string estado, int idAyuda)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var ayuda = await _dbContext.Ayuda.FirstOrDefaultAsync(a => a.IdAyuda == idAyuda);
                    if (ayuda == null) throw new TaskCanceledException("La ayuda seleccionada no existe");

                    List<ListaItemDTO> lista = JsonConvert.DeserializeObject<List<ListaItemDTO>>(ayuda.ListaItems!)!;

                    //List<string> Estados = new List<string>(["rechazada", "pendiente", "en proceso", "lista para entregar", "cerrada"]);

                    switch (estado)
                    {
                        case "Pendiente":
                            {
                                if (ayuda.Estado != "En Proceso") throw new InvalidOperationException("Error");
                                ayuda.Estado = estado;
                                break;
                            }
                        case "En Proceso":
                            {
                                if (ayuda.Estado != "Pendiente") throw new InvalidOperationException("Error");
                                ayuda.Estado = estado;
                                break;
                            }
                        case "Cerrado":
                            {
                                if (ayuda.Estado != "Lista Para Entregar" || lista.Any(i => i.CantidadEntregada <= 0)) throw new InvalidOperationException("La ayuda no puede cerrarse");
                                break;
                            }
                        case "Rechazado":
                            {
                                if (ayuda.Estado != "Pendiente") throw new InvalidOperationException("Error");
                                ayuda.Estado = estado;
                                break;
                            }
                        default:
                            {
                                throw new InvalidOperationException("El estado seleccionado no existe");
                            }
                    };

                    _dbContext.Update(ayuda);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public async Task<List<Ayuda>> Listar(string q)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    AyudaQuery query = JsonConvert.DeserializeObject<AyudaQuery>(q!)!;

                    string filtro = $"{query.buscar?.ToLower()}";

                    #region comentario
                    /*
                    var consulta = _dbContext.Ayuda
                        .Join(_dbContext.Personas, a => a.Solicitante, s => s.Cedula, (a, s) => new { a, s })
                        .Join(_dbContext.Personas, x => x.a.Funcionario, f => f.Cedula, (x, f) => new { x.a, x.s, f });

                    if (!string.IsNullOrEmpty(categoria))
                        consulta = consulta.Where(x => x.a.Categoria == categoria);

                    if (beneficiario != 0)
                        consulta = consulta.Where(x => x.s.Cedula == beneficiario);

                    if (funcionario != 0)
                        consulta = consulta.Where(x => x.f.Cedula == funcionario);

                    if (!string.IsNullOrEmpty(buscar))
                    {
                        var b = buscar.ToLower();
                        consulta = consulta.Where(x =>
                            (x.a.Detalle ?? "").ToLower().Contains(b) ||
                            (x.s.Nombre ?? "").ToLower().Contains(b) ||
                            (x.s.Apellido ?? "").ToLower().Contains(b) ||
                            (x.f.Nombre ?? "").ToLower().Contains(b) ||
                            (x.f.Apellido ?? "").ToLower().Contains(b));
                    }

                    var query = consulta
                        .Where(x => x.a.FechaSolicitud >= FechaIni && x.a.FechaSolicitud <= FechaFin)
                        .Select(x => new { Ayuda = x.a, Solicitante = x.s, Funcionario = x.f });


                    consulta = q.OrdenarPor switch
    {
        "FechaSolicitud" => q.Ascendente
            ? consulta.OrderBy(x => x.Ayuda.FechaSolicitud)
            : consulta.OrderByDescending(x => x.Ayuda.FechaSolicitud),

        "Categoria" => q.Ascendente
            ? consulta.OrderBy(x => x.Ayuda.Categoria)
            : consulta.OrderByDescending(x => x.Ayuda.Categoria),

        "Solicitante" => q.Ascendente
            ? consulta.OrderBy(x => x.Solicitante.Nombre)
            : consulta.OrderByDescending(x => x.Solicitante.Nombre),

        "Funcionario" => q.Ascendente
            ? consulta.OrderBy(x => x.Funcionario.Nombre)
            : consulta.OrderByDescending(x => x.Funcionario.Nombre),

        _ => consulta.OrderByDescending(x => x.Ayuda.FechaSolicitud)
    };

                    List<Ayuda> lista = await consulta.Select(a => a.a).ToListAsync();
                    */
                    #endregion

                    var consulta = from a in _dbContext.Ayuda
                                   join s in _dbContext.Personas on a.Solicitante equals s.Cedula
                                   join f in _dbContext.Personas on a.Funcionario equals f.Cedula
                                   where
                                    query.FechaIni <= a.FechaSolicitud && a.FechaSolicitud <= query.FechaFin &&

                                    (string.IsNullOrEmpty(query.categoria) || a.Categoria == query.categoria) &&

                                    (query.solicitante == 0 || query.solicitante == s.Cedula) &&

                                    (query.funcionario == 0 || query.funcionario == f.Cedula) &&

                                    (string.IsNullOrEmpty(query.buscar) ||
                                        EF.Functions.Like((a.Detalle ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((s.Nombre ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((s.Apellido ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((s.TelefonoTrabajo ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((s.TelefonoHabitacion ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((f.Nombre ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((f.Apellido ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((f.TelefonoTrabajo ?? "").ToLower(), filtro) ||
                                        EF.Functions.Like((f.TelefonoHabitacion ?? "").ToLower(), filtro)
                                    )
                                   select new
                                   {
                                       Ayuda = a,
                                       Solicitante = s,
                                       Funcionario = f
                                   };

                    consulta = query.OrdenarPor switch
                    {
                        "FechaSolicitud" => query.Ascendente
                        ? consulta.OrderBy(x => x.Ayuda.FechaSolicitud)
                        : consulta.OrderByDescending(x => x.Ayuda.FechaSolicitud),

                        "Categoria" => query.Ascendente
                        ? consulta.OrderBy(x => x.Ayuda.Categoria)
                        : consulta.OrderByDescending(x => x.Ayuda.Categoria),

                        "Solicitante" => query.Ascendente
                        ? consulta.OrderBy(x => x.Solicitante.Nombre)
                        : consulta.OrderByDescending(x => x.Solicitante.Nombre),

                        "Funcionario" => query.Ascendente
                        ? consulta.OrderBy(x => x.Funcionario.Nombre)
                        : consulta.OrderByDescending(x => x.Funcionario.Nombre),

                        _ => query.Ascendente
                        ? consulta.OrderBy(x => x.Ayuda.IdAyuda)
                        : consulta.OrderByDescending(x => x.Ayuda.IdAyuda)
                    };

                    //total
                    Console.WriteLine("Total elementos: " + await consulta.CountAsync());
                    //pagina
                    Console.WriteLine($"pagina: {query.Pagina} con {query.PageSize} elementos");

                    var lista = await consulta
                        .Skip((query.Pagina - 1) * query.PageSize)
                        .Take(query.PageSize)
                        .Select(a => a.Ayuda)
                        .ToListAsync();

                    //List<Ayuda> lista = await consulta.Select(a => a.Ayuda).ToListAsync();

                    transaction.Commit();
                    return lista;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
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
        public class AyudaQuery
        {
            //propiedades filtro
            public string categoria { get; set; } = "";
            public string buscar { get; set; } = "";
            public int solicitante { get; set; } = 0;
            public int funcionario { get; set; } = 0;
            public DateTime? FechaIni { get; set; } = null;
            public DateTime? FechaFin { get; set; } = null;
            //propiedades paginación
            public string OrdenarPor { get; set; } = "";
            public bool Ascendente { get; set; } = false;
            public int Pagina { get; set; } = 1;
            public int PageSize { get; set; } = 20;
        }
    }
}
