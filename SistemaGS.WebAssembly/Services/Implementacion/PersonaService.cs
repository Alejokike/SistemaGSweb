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

        public Task<ResponseDTO<bool>> Editar(PersonaDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<bool>> Eliminar(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<List<PersonaDTO>>> Lista(int Rol, string Buscar)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<PersonaDTO>> Obtener(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
