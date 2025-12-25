using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Service.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista(int Rol, string buscar);
        Task<UsuarioDTO> Obtener(int Cedula);
        Task<SesionDTO> Autorizacion(LoginDTO Model);
        Task<UsuarioDTO> Crear(UsuarioPersistent Model);
        Task<bool> Editar(UsuarioPersistent Model);
        Task<bool> Eliminar(int Cedula);
    }
}
