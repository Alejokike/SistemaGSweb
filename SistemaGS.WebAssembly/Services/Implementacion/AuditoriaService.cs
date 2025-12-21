using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class AuditoriaService : IAuditoriaService
    {
        public async Task<ResponseDTO<List<RegistroDTO>>> Listar(RegistroQuery filtro)
        {
            var lista = new List<RegistroDTO>();
            lista.Add(new RegistroDTO()
            {
                IdRegistro = 1,
                TablaAfectada = "Prueba",
                IdRegistroAfectado = 0,
                UsuarioResponsable = 0,
                Accion = "Prueba",
                FechaAccion = DateTime.Today,
                Detalle = "Prueba"
            });
            return await new Task<ResponseDTO<List<RegistroDTO>>>(() => new ResponseDTO<List<RegistroDTO>>()
            {
                EsCorrecto = true,
                Mensaje = "",
                Resultado = lista
            });
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
    }
}
