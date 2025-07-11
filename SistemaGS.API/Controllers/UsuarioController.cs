using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaGS.Service.Contrato;
using SistemaGS.DTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("Lista/{rol:int}/{buscar:alpha?}")]
        public async Task<IActionResult> Lista(int rol, string buscar = "NA")
        {
            var response = new ResponseDTO<List<UsuarioDTO>>();

            try
            {
                if (buscar == "NA") buscar = "";

                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Lista(rol, buscar);
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
            var response = new ResponseDTO<UsuarioDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Obtener(id); 
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody]UsuarioDTO model)
        {
            var response = new ResponseDTO<UsuarioDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Crear(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Autorizacion")]
        public async Task<IActionResult> Autorizacion([FromBody] LoginDTO model)
        {
            var response = new ResponseDTO<SesionDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Autorizacion(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO model)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Editar(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar([FromBody] int id)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Eliminar(id);
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
