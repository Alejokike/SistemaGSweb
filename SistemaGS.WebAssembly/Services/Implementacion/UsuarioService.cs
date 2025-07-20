using SistemaGS.DTO;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;
using System.Reflection;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;
        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<SesionDTO>> Autorizacion(LoginDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Usuario/Autorizacion", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<SesionDTO>>();
            return result!;
        }

        public async Task<ResponseDTO<UsuarioDTO>> Crear(UsuarioDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Usuario/Crear", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<UsuarioDTO>>();
            return result!;
        }

        public async Task<ResponseDTO<bool>> Editar(UsuarioDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Usuario/Editar", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }

        public async Task<ResponseDTO<bool>> Eliminar(int Id)
        {
            return (await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Usuario/Eliminar/{Id}"))!;
        }

        public async Task<ResponseDTO<List<UsuarioDTO>>> Lista(int Rol, string Buscar)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<UsuarioDTO>>>($"Usuario/Lista/{Rol}/{Buscar}"))!;
        }

        public async Task<ResponseDTO<UsuarioDTO>> Obtener(int Id)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<UsuarioDTO>>($"Usuario/Obtener/{Id}"))!;
        }
    }
}
