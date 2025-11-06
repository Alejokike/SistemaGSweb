using SistemaGS.DTO;


namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IUsuarioService
    {
        Task<ResponseDTO<List<UsuarioDTO>>> Lista(int Rol, string Buscar);
        Task<ResponseDTO<UsuarioDTO>> Obtener(int Id);
        Task<ResponseDTO<SesionDTO>> Autorizacion(LoginDTO model);
        Task<ResponseDTO<UsuarioDTO>> Crear(UsuarioDTO model);
        Task<ResponseDTO<bool>> Editar(UsuarioDTO model);
        Task<ResponseDTO<bool>> Eliminar(int Id);
    }
}
