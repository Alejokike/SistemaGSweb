using Blazored.LocalStorage;
using SistemaGS.DTO;
using System.Net.Http.Headers;

namespace SistemaGS.WebAssembly.Extensiones
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        public AuthMessageHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SesionDTO? sesion = await _localStorageService.GetItemAsync<SesionDTO>("sesionUsuario");

            if (sesion != null && !string.IsNullOrEmpty(sesion.AuthResponse.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sesion.AuthResponse.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);

            /*
             // 3. Si el token expiró → intentar refresh if (response.StatusCode == HttpStatusCode.Unauthorized && sesion != null) { var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "auth/refresh") { Content = JsonContent.Create(new { RefreshToken = sesion.AuthResponse.RefreshToken }) }; var refreshResponse = await base.SendAsync(refreshRequest, cancellationToken); if (refreshResponse.IsSuccessStatusCode) { var newSesion = await refreshResponse.Content.ReadFromJsonAsync<SesionDTO>(); if (newSesion != null) { // 4. Guardar nuevo token await _localStorageService.SetItemAsync("sesionUsuario", newSesion); // 5. Reintentar la petición original con el nuevo token request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newSesion.AuthResponse.AccessToken); response = await base.SendAsync(request, cancellationToken); } } } return response;
             */
        }
    }
}
