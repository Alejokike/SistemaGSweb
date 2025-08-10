using SistemaGS.DTO;

namespace SistemaGS.Service.Contrato
{
    public interface IPersonaService
    {
        Task<List<PersonaDTO>> Lista(int tipo, string buscar);
        Task<PersonaDTO> Obtener(int id);
        Task<PersonaDTO> Crear(PersonaDTO Model);
        Task<bool> Editar(PersonaDTO Model);
        Task<bool> Eliminar(int id);
    }
}
