using Microsoft.AspNetCore.Mvc;
using SistemaGS.API.Infraestructure;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Service.Contrato;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly TokenProvider _tokenProvider;
        private readonly DataAccess _dataAccess;
        public SecurityController(ISecurityService securityService, TokenProvider tokenProvider, DataAccess dataAccess)
        {
            _securityService = securityService;
            _tokenProvider = tokenProvider;
            _dataAccess = dataAccess;
        }
        [HttpPost("Autorizacion")]
        public async Task<IActionResult> Autorizacion([FromBody] LoginDTO model)
        {
            var response = new ResponseDTO<SesionDTO>();
            try
            {
                SesionDTO sesion = await _securityService.Autorizacion(model);

                Token token = _tokenProvider.GenerateToken(sesion);
                
                sesion.AuthResponse.AccessToken = token.AccessToken;
                sesion.AuthResponse.RefreshToken = token.RefreshToken;

                _dataAccess.DisableUserTokenByCedula(sesion.Cedula);
                _dataAccess.InsertRefreshToken(token.RefreshToken, sesion.Cedula);

                response.EsCorrecto = true;
                response.Resultado = sesion;
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            try
            {
                string refreshtoken = Request.Cookies["refreshtoken"] ?? "";

                AuthResponse response = new AuthResponse();

                if (string.IsNullOrEmpty(refreshtoken)) return BadRequest();

                bool isValid = _dataAccess.IsRefreshTokenValid(refreshtoken);
                if (!isValid) return BadRequest();

                int currentUser = _dataAccess.FindUserByToken(refreshtoken);
                if (currentUser == 0) return BadRequest();

                SesionDTO usuario = await _securityService.ObtenerById(currentUser);

                Token token = _tokenProvider.GenerateToken(usuario);
                response.AccessToken = token.AccessToken;
                response.RefreshToken = token.RefreshToken;

                _dataAccess.DisableUserToken(refreshtoken);
                _dataAccess.InsertRefreshToken(token.RefreshToken, currentUser);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            string? refreshToken = Request.Cookies["refreshtoken"];
            if (!string.IsNullOrEmpty(refreshToken)) _dataAccess.DisableUserToken(refreshToken);

            return Ok();
        }
        [HttpGet("Listar")]
        public async Task<IActionResult> Auditoria([FromQuery] RegistroQuery filtro)
        {
            var response = new ResponseDTO<List<RegistroDTO>>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _securityService.Listar(filtro);
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
            var response = new ResponseDTO<RegistroDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _securityService.Obtener(id);
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
