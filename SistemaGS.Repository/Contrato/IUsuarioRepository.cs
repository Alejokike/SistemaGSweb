using SistemaGS.Model;
namespace SistemaGS.Repository.Contrato
{
    public interface IUsuarioRepository
    {
        Task<(Usuario usuario, Persona persona, Rol rol)> Obtener(int IdUsuario);
        Task<List<(Usuario usuario, Persona persona, Rol rol)>> Listar(int rol, string buscar);
        Task<bool> Registrar(Usuario usuario, Persona persona);
        Task<bool> Editar(Usuario usuario, Persona persona);
        Task<bool> Eliminar(int Cedula);
    }
}
