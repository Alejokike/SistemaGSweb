using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Prng;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Repository.DBContext;
using SistemaGS.Service.Contrato;
using SistemaGS.Service.Implementacion;
using System.Threading.Tasks;

namespace SistemaGS.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ResourceController : Controller
    {
        private readonly IAyudaService ayudaService;
        private readonly IUsuarioService usuarioService;
        private readonly IInventarioService inventarioService;
        public ResourceController(IAyudaService ayudaService, IUsuarioService usuarioService, IInventarioService inventarioService)
        {
            this.ayudaService = ayudaService;
            this.inventarioService = inventarioService;
            this.usuarioService = usuarioService;
        }
        [Authorize]
        [HttpGet("Random")]
        public async Task<ActionResult> Random()
        {
            Random aleatorio = new Random();

            List<UsuarioDTO> funcionarios = await usuarioService.Lista(2, "");
            int cantf = funcionarios.Count;

            List<UsuarioDTO> solicitantes = await usuarioService.Lista(3, "");
            int cants = solicitantes.Count;

            List<ItemDTO> items = await inventarioService.ListarInventario(new DTO.Query.ItemQuery());
            int canti = items.Count;

            string[] categorias = { "Medicamentos", "Insumos quirúrgicos", "Citas para exámenes de laboratorio", "Citas para exámenes de diagnóstico", "Equipos de movilidad asistida", "Prótesis", "Apoyo financiero para cirugías", "Utiles escolares", "Alimentos", "Financiamiento deportivo", "Ayudas funerarias" };
            int cantic = categorias.Length;

            string[] estados = { "Por Aprobar", "En Proceso", "Lista Para Entregar", "Cerrada", "Rechazada" };
            int cante = estados.Length;

            List<AyudaDTO> ayudas = new List<AyudaDTO>();

            for (int i = 0; i < 100; i++)
            {
                DateTime ini = new DateTime(2025, 1, 1).AddDays(aleatorio.Next(0, 730));
                DateTime fin = ini.AddMonths(aleatorio.Next(1, 3)).AddDays(aleatorio.Next(1, 8));
                string estado = estados[aleatorio.Next(0, cante)];

                AyudaDTO ayuda = new AyudaDTO()
                {
                    IdAyuda = 0,
                    Funcionario = funcionarios[aleatorio.Next(0, cantf)].Cedula ?? 0,
                    Solicitante = solicitantes[aleatorio.Next(0, cants)].Cedula ?? 0,
                    Categoria = categorias[aleatorio.Next(0, cantic)],
                    Detalle = new Dictionary<string, string>()
                    {
                        ["Solicitud"] = "Lorem Ipsum",
                        ["Observaciones"] = "Dolor",
                        ["Aprobado"] = estado == "Cerrada" ? "Sit" : "",
                        ["Rechazado"] = estado == "Rechazada" ? "Amet" : ""
                    },
                    ListaItems = new List<ListaItemDTO>(),
                    Estado = estado,
                    FechaSolicitud = ini,
                    FechaEntrega = estado == "Cerrada" || estado == "Rechazada" ? fin : null
                };
                /*
                switch (ayuda.Categoria)
                {
                    case "Medicamentos":
                        {
                            items = items.Where(c => c.Categoria == "Médicamentos").ToList();
                            canti = items.Count;
                            break;
                        }
                    case "Insumos quirúrgicos":
                        {
                            items = items.Where(c => c.Categoria == "Insumos quirúrgicos").ToList();
                            canti = items.Count;
                            break;
                        }
                    case "Prótesis":
                        {
                            items = items.Where(c => c.Categoria == "Prótesis").ToList();
                            canti = items.Count;
                            break;
                        }
                    case "Equipos de movilidad asistida":
                        {
                            items = items.Where(c => c.Categoria == "Equipos de movilidad asistida").ToList();
                            canti = items.Count;
                            break;
                        }
                    case "Citas para exámenes de laboratorio":
                    case "Citas para exámenes de diagnóstico":
                        {
                            items = items.Where(c => c.Categoria == "Servicios médicos").ToList();
                            canti = items.Count;
                            break;
                        }
                    case "Alimentos":
                    case "Utiles escolares":
                        {
                            items = items.Where(c => c.Categoria == "Bienes de apoyo social").ToList();
                            canti = items.Count;
                            break;
                        }
                    default:
                        {
                            items = items.Where(c => c.Categoria == "Fondos y financiamiento").ToList();
                            canti = items.Count;
                            break;
                        }
                }
                */
                int cantItemsLista = aleatorio.Next(5, 15);

                for (int j = 0; j < cantItemsLista; j++)
                {
                    int cantidad = aleatorio.Next(10, 20);
                    ayuda.ListaItems.Add(new ListaItemDTO()
                    {
                        IdLista = j + 1,
                        ItemLista = items[aleatorio.Next(0,canti)],
                        CantidadSolicitada = cantidad,
                        CantidadEntregada = ayuda.Estado != "Cerrada" ? 0 : aleatorio.Next(1,cantidad)
                    });
                }

                ayudas.Add(ayuda);
            }

            await ayudaService.MasiveAttack(ayudas);

            return Ok();
        }
    }
}
