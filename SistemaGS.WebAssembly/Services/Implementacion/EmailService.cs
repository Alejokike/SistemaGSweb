using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.WebAssembly.Extensiones;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<bool>> CambiarClave(UsuarioDTO model)
        {
            UsuarioPersistent usuario = JsonConvert.DeserializeObject<UsuarioPersistent>(JsonConvert.SerializeObject(model))!;
            //usuario.Persona = new PersonaPersistent();
            usuario.Clave = Ferramentas.ConvertToSha256(model.Clave);
            usuario.ConfirmarClave = "";

            var response = await _httpClient.PutAsJsonAsync("Email/CambiarClave", usuario);

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
        public async Task<ResponseDTO<bool>> EnviarCorreo(EmailDTO correo)
        {
            var response = await _httpClient.PostAsJsonAsync("Email/EnviarCorreo", correo);

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
        public async Task<ResponseDTO<UsuarioDTO>> ObtenerUsuarioByCorreo(string email)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<UsuarioDTO>>($"Email/ObtenerUsuarioByCorreo/{email}"))!;
        }
    }
}
