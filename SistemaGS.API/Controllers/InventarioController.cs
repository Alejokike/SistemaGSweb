using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.DTO.Responses;
using SistemaGS.Service.Contrato;
using System.Text.Json;

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
        [HttpGet("ListarInventario")]
        public async Task<IActionResult> ListarInventario([FromQuery] ItemQuery filtro)
        {
            var response = new ResponseDTO<InventarioRespuesta>();

            try
            {
                response.EsCorrecto = true;
                //JsonDocument resultado = JsonDocument.Parse(await _inventarioService.ListarInventario(filtro));
                var aux = await _inventarioService.ListarInventario(filtro);

                response.Resultado = new InventarioRespuesta()
                {
                    contenido = aux.Item1,
                    contador = aux.Item2
                };
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
        public async Task<IActionResult> Desbloquear([FromQuery] List<InventarioDTO> movimientos, int IdAyuda)
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
