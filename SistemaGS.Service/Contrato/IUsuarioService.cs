using SistemaGS.DTO;

namespace SistemaGS.Service.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista(int Rol, string buscar);
        Task<UsuarioDTO> Obtener(int Cedula);
        Task<SesionDTO> Autorizacion(LoginDTO Model);
        Task<UsuarioDTO> Crear(UsuarioDTO Model);
        Task<bool> Editar(UsuarioDTO Model);
        Task<bool> Eliminar(int Cedula);
    }
}
