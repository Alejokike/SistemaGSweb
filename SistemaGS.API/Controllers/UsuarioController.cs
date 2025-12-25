using Microsoft.AspNetCore.Mvc;
using SistemaGS.Service.Contrato;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.API.Infraestructure;
using SistemaGS.DTO.AuthDTO;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly TokenProvider _tokenProvider;
        private readonly DataAccess _dataAccess;
        public UsuarioController(IUsuarioService usuarioService, TokenProvider tokenProvider, DataAccess dataAccess)
        {
            _usuarioService = usuarioService;
            _tokenProvider = tokenProvider;
            _dataAccess = dataAccess;
        }

        [HttpGet("Lista/{rol:int}/{buscar?}")]
        public async Task<IActionResult> Lista(int rol, string buscar = "")
        {
            var response = new ResponseDTO<List<UsuarioDTO>>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _usuarioService.Lista(rol, buscar);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
                Console.WriteLine(ex);
            }
            return Ok(response);
        }
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var response = new ResponseDTO<UsuarioDTO?>();
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
        public async Task<IActionResult> Crear([FromBody]UsuarioPersistent model)
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
        
        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioPersistent model)
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
        public async Task<IActionResult> Eliminar(int id)
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
        //Security
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
        public async Task<ActionResult<AuthResponse>> Refresh()
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