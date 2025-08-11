using SistemaGS.DTO;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IPersonaService
    {
        Task<ResponseDTO<List<PersonaDTO>>> Lista(int Tipo, string Buscar);
        Task<ResponseDTO<PersonaDTO>> Obtener(int Cedula);
        Task<ResponseDTO<PersonaDTO>> Crear(PersonaDTO model);
        Task<ResponseDTO<bool>> Editar(PersonaDTO model);
        Task<ResponseDTO<bool>> Eliminar(int Cedula);
    }
}
