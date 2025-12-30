using Microsoft.AspNetCore.WebUtilities;
using SistemaGS.DTO;
using SistemaGS.DTO.AuthDTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class SecurityService : ISecurityService
    {
        private readonly HttpClient _httpClient;
        public SecurityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<List<RegistroDTO>>> Listar(RegistroQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["Accion"] = filtro.Accion,
                ["UsuarioResponsable"] = filtro.UsuarioResponsable.ToString(),
                ["FechaIni"] = filtro.FechaIni.HasValue ? filtro.FechaIni.Value.ToString("yyyy-MM-dd") : null,
                ["FechaFin"] = filtro.FechaFin.HasValue ? filtro.FechaFin.Value.ToString("yyyy-MM-dd") : null
            };

            var url = QueryHelpers.AddQueryString("Security/Lista", queryparams);

            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<RegistroDTO>>>(url))!;
        }
        public async Task<ResponseDTO<RegistroDTO>> Obtener(int id)
        {
            return await new Task<ResponseDTO<RegistroDTO>>(() => new ResponseDTO<RegistroDTO>()
            {
                EsCorrecto = true,
                Mensaje = "",
                Resultado = new RegistroDTO()
            });
        }
        public Task<ResponseDTO<SesionDTO>> Autorizar()
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDTO<AuthResponse>> Refresh()
        {
            throw new NotImplementedException();
        }
        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
