using Microsoft.AspNetCore.Mvc;

using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;
        public InventarioController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        [HttpGet("Lista/{IdItem:int}/{FechaIni:datetime}/{FechaFin:datetime}/{filtro?}")]
        public async Task<IActionResult> Lista(int IdItem, DateTime? FechaIni = null, DateTime? FechaFin = null, string filtro = "NA")
        {
            var response = new ResponseDTO<List<InventarioDTO>>();

            try
            {
                if (filtro == "NA") filtro = "";
                if (FechaIni == null) FechaIni = new DateTime(DateTime.Today.Year, 1, 1);
                if (FechaFin == null) FechaFin = new DateTime(DateTime.Today.Year, 12, 31);
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Lista(IdItem, filtro, FechaIni, FechaFin);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("ListarInevntario")]
        public async Task<IActionResult> ListarInventario([FromQuery] JsonResult filtro)
        {
            var response = new ResponseDTO<JsonResult>();
        }
        [HttpGet("Obtener/{idTransaccion:int}")]
        public async Task<IActionResult> Obtener(int idTransaccion)
        {
            var response = new ResponseDTO<InventarioDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Obtener(idTransaccion);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] InventarioDTO Transaccion)
        {
            var response = new ResponseDTO<InventarioDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Registrar(Transaccion);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Desbloquear/{IdAyuda:int}")]
        public async Task<IActionResult> Registrar([FromBody] List<InventarioDTO> movimientos, int IdAyuda)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Desbloquear(movimientos, IdAyuda);
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
