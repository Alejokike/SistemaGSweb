using Microsoft.AspNetCore.Mvc;
using SistemaGS.Service.Contrato;
using SistemaGS.DTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService; 

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpGet("Lista/{Tipo:int}/{buscar?}")]
        public async Task<IActionResult> Lista(int Tipo, string buscar = "NA")
        {
            var response = new ResponseDTO<List<ItemDTO>>();

            try
            {
                if (buscar == "NA") buscar = "";

                response.EsCorrecto = true;
                response.Resultado = await _itemService.Lista(Tipo, buscar);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("Obtener/{Cedula:int}")]
        public async Task<IActionResult> Obtener(int Cedula)
        {
            var response = new ResponseDTO<ItemDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _itemService.Obtener(Cedula);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] ItemDTO model)
        {
            var response = new ResponseDTO<ItemDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _itemService.Crear(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Editar/{Cedula:int}")]
        public async Task<IActionResult> Editar([FromBody] ItemDTO model, int Cedula)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _itemService.Editar(model, Cedula);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _itemService.Eliminar(id);
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
