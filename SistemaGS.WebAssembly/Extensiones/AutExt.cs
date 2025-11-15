using Blazored.LocalStorage;
using SistemaGS.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SistemaGS.WebAssembly.Extensiones
{
    public class AutExt : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _sinInformacion = new ClaimsPrincipal(new ClaimsIdentity());
        public AutExt(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task ActualizarEstadoAut(SesionDTO? sesionUsuario)
        {
            ClaimsPrincipal claimsPrincipal;

            if(sesionUsuario != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, sesionUsuario.Cedula.ToString()),
                    new Claim(ClaimTypes.Name, sesionUsuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Email, sesionUsuario.Correo.ToString()),
                    new Claim(ClaimTypes.Role, sesionUsuario.Rol.Nombre),
                }, "JwtAuth"));

                await _localStorage.SetItemAsync("sesionUsuario", sesionUsuario);
            }
            else
            {
                claimsPrincipal = _sinInformacion;
                await _localStorage.RemoveItemAsync("sesionUsuario");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionUsuario = await _localStorage.GetItemAsync<SesionDTO>("sesionUsuario");
            if (sesionUsuario == null) return await Task.FromResult(new AuthenticationState(_sinInformacion));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, sesionUsuario.Cedula.ToString()),
                    new Claim(ClaimTypes.Name, sesionUsuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Email, sesionUsuario.Correo.ToString()),
                    new Claim(ClaimTypes.Role, sesionUsuario.Rol.Nombre),
                }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
    }
}
