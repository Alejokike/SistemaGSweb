using Microsoft.AspNetCore.Mvc;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Service.Contrato;
using SistemaGS.Service.Implementacion;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AyudaController : Controller
    {
        private readonly IAyudaService _ayudaService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AyudaController(IAyudaService usuarioService, IHttpContextAccessor contextAccessor   )
        {
            _ayudaService = usuarioService;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("Lista")]
        public async Task<IActionResult> Lista([FromQuery] AyudaQuery filtro)
        {
            var response = new ResponseDTO<List<AyudaDTO>>();
            
            try
            {
                if (filtro.FechaIni == null) filtro.FechaIni = new DateTime(DateTime.Now.Year, 1, 1);
                if (filtro.FechaFin == null) filtro.FechaFin = new DateTime(DateTime.Now.Year, 12, 31);

                response.EsCorrecto = true;
                response.Resultado = await _ayudaService.Lista(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("Obtener/{IdAyuda:int}")]
        public async Task<IActionResult> Obtener(int IdAyuda)
        {
            var response = new ResponseDTO<AyudaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _ayudaService.Obtener(IdAyuda);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] AyudaDTO ayuda)
        {
            var response = new ResponseDTO<AyudaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _ayudaService.Crear(ayuda);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] AyudaDTO ayuda)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _ayudaService.Editar(ayuda);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Eliminar/{idAyuda:int}")]
        public async Task<IActionResult> Eliminar(int idAyuda)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _ayudaService.Eliminar(idAyuda);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
    }
}
