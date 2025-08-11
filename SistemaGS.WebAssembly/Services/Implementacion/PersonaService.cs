using SistemaGS.DTO;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class PersonaService : IPersonaService
    {
        private readonly HttpClient _httpClient;
        public PersonaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<PersonaDTO>> Crear(PersonaDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Persona/Crear", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<PersonaDTO>>();
            return result!;
        }

        public async Task<ResponseDTO<bool>> Editar(PersonaDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Persona/Editar", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }

        public async Task<ResponseDTO<bool>> Eliminar(int Cedula)
        {
            return (await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Persona/Eliminar/{Cedula}"))!;
        }

        public async Task<ResponseDTO<List<PersonaDTO>>> Lista(int Tipo, string Buscar)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<PersonaDTO>>>($"Persona/Lista/{Tipo}/{Buscar}"))!;
        }

        public async Task<ResponseDTO<PersonaDTO>> Obtener(int Cedula)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<PersonaDTO>>($"Persona/Obtener/{Cedula}"))!;
        }
    }
}
