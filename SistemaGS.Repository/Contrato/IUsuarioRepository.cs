using SistemaGS.Model;
namespace SistemaGS.Repository.Contrato
{
    public interface IUsuarioRepository
    {
        Task<bool> Registrar(Usuario usuario, Persona persona);
        Task<bool> Editar(Usuario usuario, Persona persona);
        Task<bool> Eliminar(int Cedula);
    }
}
