using Blazored.LocalStorage;
using SistemaGS.DTO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

            //if (response.StatusCode == HttpStatusCode.Unauthorized && sesion != null)
            //{
            //    var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "auth/refresh")
            //    {
            //        Content = JsonContent.Create(new
            //        {
            //            RefreshToken = sesion.AuthResponse.RefreshToken
            //        })
            //    };

            //    var refreshResponse = await base.SendAsync(refreshRequest, cancellationToken);

            //    if (refreshResponse.IsSuccessStatusCode)
            //    {
            //        var newSesion = await refreshResponse.Content.ReadFromJsonAsync<SesionDTO>();
            //        if (newSesion != null)
            //        {
            //            await _localStorageService.SetItemAsync("sesionUsuario", newSesion);
            //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newSesion.AuthResponse.AccessToken);
            //            response = await base.SendAsync(request, cancellationToken);
            //        }
            //    }
            //}
            //return response;
        }
    }
}
