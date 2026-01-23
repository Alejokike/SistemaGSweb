using Microsoft.AspNetCore.Mvc;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Service.Contrato;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] CategoriaQuery filtro)
        {
            var response = new ResponseDTO<List<CategoriaDTO>>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _categoriaService.Listar(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var response = new ResponseDTO<CategoriaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _categoriaService.Obtener(id);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CategoriaDTO model)
        {
            var response = new ResponseDTO<CategoriaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _categoriaService.Crear(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] CategoriaDTO model)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _categoriaService.Editar(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Editar(int id)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _categoriaService.Eliminar(id);
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
