using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class AuditoriaService : IAuditoriaService
    {
        public Task<ResponseDTO<List<RegistroDTO>>> Listar(RegistroQuery filtro)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<RegistroDTO>> Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<RegistroDTO>> Registrar(RegistroDTO registro)
        {
            throw new NotImplementedException();
        }
    }
}
