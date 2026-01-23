using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _httpClient;
        public CategoriaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<CategoriaDTO>> Crear(CategoriaDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Categoria/Crear", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<CategoriaDTO>>();
                return result!;
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Body: {errorText}");
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<CategoriaDTO>>();
                return result!;
            }
        }
        public async Task<ResponseDTO<bool>> Editar(CategoriaDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Categoria/Editar", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
                return result!;
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Body: {errorText}");
                var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
                return result!;
            }
        }
        public async Task<ResponseDTO<bool>> Eliminar(int id)
        {
            return (await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Categoria/Eliminar/{id}"))!;
        }
        public async Task<ResponseDTO<List<CategoriaDTO>>> Listar(CategoriaQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["Nombre"] = filtro.Nombre,
                ["TipoCategoria"] = filtro.TipoCategoria,
                ["Activo"] = filtro.Activo.HasValue ? filtro.Activo.Value.ToString() : null,
            };
            var url = QueryHelpers.AddQueryString("Categoria/Listar", queryparams);
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<CategoriaDTO>>>(url))!;
        }
        public async Task<ResponseDTO<CategoriaDTO>> Obtener(int id)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<CategoriaDTO>>($"Categoria/Obtener/{id}"))!;
        }
    }
}