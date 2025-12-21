using Microsoft.AspNetCore.Mvc;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
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
        [HttpGet("Listar")]
        public async Task<IActionResult> Lista([FromQuery] InventarioQuery filtro)
        {
            var response = new ResponseDTO<List<InventarioDTO>>();
            try
            {
                filtro.FechaIni ??= new DateTime(DateTime.Today.Year, 1, 1);
                filtro.FechaFin ??= new DateTime(DateTime.Today.Year, 12, 31);
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Lista(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
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
        [HttpGet("ListarInventario")]
        public async Task<IActionResult> ListarInventario([FromQuery] ItemQuery filtro)
        {
            var response = new ResponseDTO<List<ItemDTO>>();
            try
            {
                filtro.FechaIni ??= new DateTime(DateTime.Today.Year, 1, 1);
                filtro.FechaFin ??= new DateTime(DateTime.Today.Year, 12, 31);
                response.EsCorrecto = true;                
                response.Resultado = await _inventarioService.ListarInventario(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("ObtenerItem/{idItem:int}/{nombre?}")]
        public async Task<IActionResult> ObtenerItem(int idItem, string nombre = "N/A")
        {
            var response = new ResponseDTO<ItemDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.ObtenerItem(idItem, nombre);
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
        public async Task<IActionResult> Desbloquear(int IdAyuda, [FromBody] List<InventarioDTO> movimientos)
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
