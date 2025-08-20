using Microsoft.AspNetCore.Mvc;
using SistemaGS.Service.Contrato;
using SistemaGS.DTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        public PersonaController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        [HttpGet("Lista/{Tipo:int}/{buscar?}")]
        public async Task<IActionResult> Lista(int Tipo, string buscar = "NA")
        {
            var response = new ResponseDTO<List<PersonaDTO>>();

            try
            {
                if (buscar == "NA") buscar = "";

                response.EsCorrecto = true;
                response.Resultado = await _personaService.Lista(Tipo, buscar);
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
            var response = new ResponseDTO<PersonaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _personaService.Obtener(Cedula);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] PersonaDTO model)
        {
            var response = new ResponseDTO<PersonaDTO>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _personaService.Crear(model);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("Editar/{Cedula:int}")]
        public async Task<IActionResult> Editar([FromBody] PersonaDTO model, int Cedula)
        {
            var response = new ResponseDTO<bool>();

            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _personaService.Editar(model, Cedula);
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
                response.Resultado = await _personaService.Eliminar(id);
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
