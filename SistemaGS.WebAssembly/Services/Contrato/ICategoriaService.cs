using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface ICategoriaService
    {
        Task<ResponseDTO<List<CategoriaDTO>>> Listar(CategoriaQuery filtro);
        Task<ResponseDTO<CategoriaDTO>> Obtener(int id);
        Task<ResponseDTO<CategoriaDTO>> Crear(CategoriaDTO model);
        Task<ResponseDTO<bool>> Editar(CategoriaDTO model);
        Task<ResponseDTO<bool>> Eliminar(int id);
    }
}
