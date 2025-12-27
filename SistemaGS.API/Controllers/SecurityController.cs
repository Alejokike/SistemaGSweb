using Microsoft.AspNetCore.Mvc;
using SistemaGS.API.Infraestructure;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        public SecurityController() 
        { 
            
        }
        [HttpPost("Autorizacion")]
        public async Task<IActionResult> Autorizacion([FromBody] LoginDTO model)
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
        [HttpPost("Refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh(string refresh)
        {
            AuthResponse response = new AuthResponse();

            string? refreshToken = Request.Cookies["refreshtoken"];
            if (string.IsNullOrEmpty(refreshToken)) return BadRequest();

            bool isValid = _dataAccess.IsRefreshTokenValid(refreshToken);
            if (!isValid) return BadRequest();

            UsuarioDTO usuario;
            int currentUser = _dataAccess.FindUserByToken(refreshToken);
            if (currentUser == 0) return BadRequest();

            usuario = await _usuarioService.Obtener(currentUser);

            var token = _tokenProvider.GenerateToken(usuario);
            response.AccessToken = token.AccessToken;
            response.RefreshToken = token.RefreshToken;

            _dataAccess.DisableUserToken(refreshToken);
            _dataAccess.InsertRefreshToken(token.RefreshToken, currentUser);

            return Ok(response);

        }
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            string? refreshToken = Request.Cookies["refreshtoken"];
            if (!string.IsNullOrEmpty(refreshToken)) _dataAccess.DisableUserToken(refreshToken);

            return Ok();
        }
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
    }
}
