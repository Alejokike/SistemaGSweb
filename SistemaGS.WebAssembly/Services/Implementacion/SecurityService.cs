using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Extensiones;
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

            var url = QueryHelpers.AddQueryString("Security/Listar", queryparams);
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<RegistroDTO>>>(url))!;
        }
        public async Task<ResponseDTO<RegistroDTO>> Obtener(int id)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<RegistroDTO>>($"Security/Obtener/{id}"))!;
        }
        public async Task<ResponseDTO<SesionDTO>> Autorizacion(LoginDTO model)
        {
            LoginDTO login = JsonConvert.DeserializeObject<LoginDTO>(JsonConvert.SerializeObject(model))!;
            login.Clave = Ferramentas.ConvertToSha256(login.Clave);
            model.Clave = "";

            var response = await _httpClient.PostAsJsonAsync("Security/Autorizacion", login);

            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<SesionDTO>>();
            return result!;
        }
    }
}
