using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.WebAssembly.Services.RealServices
{
    public class AyudaService : IAyudaService
    {
        private readonly HttpClient _httpClient;
        public AyudaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<AyudaDTO>> Crear(AyudaDTO ayuda)
        {
            var response = new ResponseDTO<AyudaDTO>() { EsCorrecto = true, Resultado = ayuda , Mensaje = ""};
            return await Task.FromResult(response);
        }

        public async Task<ResponseDTO<bool>> Editar(AyudaDTO ayuda)
        {
            return await Task.FromResult(new ResponseDTO<bool>() { EsCorrecto = true, Resultado = true, Mensaje = ""} );
        }

        public async Task<ResponseDTO<bool>> Eliminar(int idAyuda)
        {
            return await Task.FromResult(new ResponseDTO<bool>() { EsCorrecto = true, Resultado = true, Mensaje = "" });
        }

        public async Task<ResponseDTO<List<AyudaDTO>>> Listar(AyudaQuery filtro)
        {
            return await Task.FromResult(new ResponseDTO<List<AyudaDTO>>() { EsCorrecto = true, Resultado = new List<AyudaDTO>(), Mensaje = "" });
        }

        public async Task<ResponseDTO<AyudaDTO>> Obtener(int idAyuda)
        {
            return await Task.FromResult(new ResponseDTO<AyudaDTO>() { EsCorrecto = true, Resultado = new AyudaDTO(), Mensaje = "" });
        }
    }
}
