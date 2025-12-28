using Microsoft.AspNetCore.Mvc;
using SistemaGS.API.Infraestructure;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
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
        [HttpPost("Refresh/{refreshtoken}")]
        public async Task<ActionResult<AuthResponse>> Refresh(string refreshtoken)
        {
            try
            {
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
        /*
        [HttpGet("Auditoria")]
        public async Task<IActionResult> Auditoria([FromQuery] LoginDTO model)
        {
            var response = new ResponseDTO<SesionDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Autorizacion(model);

                var token = _tokenProvider.GenerateToken(await _usuarioService.Obtener(response.Resultado.Cedula));
                response.Resultado.AuthResponse.AccessToken = token.AccessToken;
                response.Resultado.AuthResponse.RefreshToken = token.RefreshToken;

                _dataAccess.DisableUserTokenByCedula(response.Resultado.Cedula);
                _dataAccess.InsertRefreshToken(token.RefreshToken, response.Resultado.Cedula);
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
