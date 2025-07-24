using SistemaGS.DTO;

namespace SistemaGS.Service.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> Lista(int Rol, string buscar);
        Task<RolDTO> Obtener(int id);
        Task<RolDTO> Crear(RolDTO Model);
        Task<bool> Editar(RolDTO Model);
        Task<bool> Eliminar(int id);
    }
}
