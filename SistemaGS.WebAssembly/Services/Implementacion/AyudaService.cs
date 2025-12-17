using Microsoft.AspNetCore.WebUtilities;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class AyudaService : IAyudaService
    {
        private readonly HttpClient _httpClient;
        public AyudaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<AyudaDTO>> Crear(AyudaDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Ayuda/Crear", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<AyudaDTO>>();
                return result!;
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Body: {errorText}");
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<AyudaDTO>>();
                return result!;
            }
        }
        public async Task<ResponseDTO<bool>> Editar(AyudaDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Ayuda/Editar", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }
        public async Task<ResponseDTO<bool>> Eliminar(int idAyuda)
        {
            return (await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Ayuda/Eliminar/{idAyuda}"))!;
        }
        public async Task<ResponseDTO<List<AyudaDTO>>> Listar(AyudaQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["categoria"] = filtro.categoria,
                ["solicitante"] = filtro.solicitante.ToString(),
                ["funcionario"] = filtro.funcionario.ToString(),
                ["DataSoli"] = filtro.DataSoli,
                ["DataFunci"] = filtro.DataFunci,
                ["Estado"] = filtro.Estado,
                ["FechaIni"] = filtro.FechaIni.HasValue ? filtro.FechaIni.Value.ToString("yyyy-MM-dd") : null,
                ["FechaFin"] = filtro.FechaFin.HasValue ? filtro.FechaFin.Value.ToString("yyyy-MM-dd") : null
            };

            var url = QueryHelpers.AddQueryString("Ayuda/Lista", queryparams);

            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<AyudaDTO>>>(url))!;
        }
        public async Task<ResponseDTO<AyudaDTO>> Obtener(int idAyuda)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<AyudaDTO>>($"Ayuda/Obtener/{idAyuda}"))!;
        }
    }
}
