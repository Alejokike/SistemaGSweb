using Microsoft.AspNetCore.Mvc;
using SistemaGS.DTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        public DocumentosController()
        {
            
        }
        /*
        [HttpGet("Obtener/{TipoRegistro:alpha}/{IdRegistro:int}")]
        public async Task<IActionResult> Obtener(string TipoRegistro, int IdRegistro)
        {
            var response = new ResponseDTO<List<byte[]>>();
            try
            {
                response.EsCorrecto = true;
                //response.Resultado = 
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("Guardar/{TipoRegistro:alpha}/{IdRegistro:int}")]
        public async Task<IActionResult> Guardar([FromBody] List<FileTransferDTO> archivos)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.EsCorrecto = true;
                //response.Resultado = 
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        */
    }
}
