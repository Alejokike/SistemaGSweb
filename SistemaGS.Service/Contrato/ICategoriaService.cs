using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.Service.Contrato
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> Listar(CategoriaQuery filtro);
        Task<CategoriaDTO> Obtener(int id);
        Task<CategoriaDTO> Crear(CategoriaDTO model);
        Task<bool> Editar(CategoriaDTO model);
        Task<bool> Eliminar(int id);        
    }
}
