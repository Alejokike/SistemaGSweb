using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;

namespace SistemaGS.Service.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericoRepository<Categoria> _modelRepository;
        private readonly IGenericoRepository<Ayuda> _AyudaRepository;
        private readonly IGenericoRepository<Item> _ItemRepository;
        private readonly IMapper _mapper;
        public CategoriaService(IGenericoRepository<Categoria> modelRepository, IGenericoRepository<Ayuda> AyudaRepository, IGenericoRepository<Item> ItemRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _AyudaRepository = AyudaRepository;
            _ItemRepository = ItemRepository;
            _mapper = mapper;
        }
        public async Task<CategoriaDTO> Crear(CategoriaDTO model)
        {
            try
            {
                if (!await _modelRepository.Consultar(c => c.Nombre == model.Nombre && c.TipoCategoria == model.TipoCategoria).AsNoTracking().AnyAsync())
                {
                    return _mapper.Map<CategoriaDTO>(await _modelRepository.Crear(_mapper.Map<Categoria>(model)));                    
                }
                else throw new TaskCanceledException("Esa categoría ya existe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Editar(CategoriaDTO model)
        {
            try
            {
                if (await _modelRepository.Consultar(c => c.IdCategoria == model.IdCategoria).AsNoTracking().AnyAsync())
                {
                    if(model.TipoCategoria == "Ayuda")
                    {
                        if (model.Activo == false && await _AyudaRepository.Consultar(a => a.Categoria == model.Nombre && (a.Estado != "Rechazada" || a.Estado != "Cerrada")).AsNoTracking().AnyAsync()) 
                            throw new TaskCanceledException("No se puede inactivar una categoría de ayudas en proceso");
                    }
                    else
                    {
                        if (model.Activo == false && await _ItemRepository.Consultar(i => i.Categoria == model.Nombre && i.Activo == true).AsNoTracking().AnyAsync())
                            throw new TaskCanceledException("No se puede inactivar una categoría de ítems activos en el inventario");
                    }
                    return await _modelRepository.Editar(_mapper.Map<Categoria>(model));
                }
                else throw new TaskCanceledException("Categoría no encontrada");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var encontrado = await _modelRepository.Consultar(c => c.IdCategoria == id).AsNoTracking().FirstOrDefaultAsync();
                if (encontrado != null)
                {
                    if (encontrado.TipoCategoria == "Ayuda") 
                    { 
                        if (await _AyudaRepository.Consultar(a => a.Categoria == encontrado.Nombre).AsNoTracking().AnyAsync()) 
                            throw new TaskCanceledException("La categoría ya esta siendo usada en una ayuda"); 
                    }
                    else 
                    { 
                        if(await _ItemRepository.Consultar(i => i.Categoria == encontrado.Nombre).AsNoTracking().AnyAsync()) 
                            throw new TaskCanceledException("La categoría ya esta siendo usada en un ítem"); 
                    }
                    return await _modelRepository.Eliminar(encontrado);
                }
                else throw new TaskCanceledException("No existe la categoría");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<List<CategoriaDTO>> Listar(CategoriaQuery filtro)
        {
            string? busqueda = string.IsNullOrEmpty(filtro.Nombre) ? null : $"%{filtro.Nombre.Trim()}%";
            var consulta = from c in _modelRepository.Consultar().AsNoTracking()
                           where
                           (string.IsNullOrEmpty(filtro.TipoCategoria) || c.TipoCategoria == filtro.TipoCategoria) &&
                           (string.IsNullOrEmpty(busqueda) || EF.Functions.Like((c.Nombre ?? ""), busqueda)) &&
                           (filtro.Activo == null || c.Activo == filtro.Activo)
                           select c;
            return _mapper.Map<List<CategoriaDTO>>(await consulta.AsNoTracking().ToListAsync());
        }
        public async Task<CategoriaDTO> Obtener(int id)
        {
            return _mapper.Map<CategoriaDTO>(await _modelRepository.Consultar(c => c.IdCategoria == id).AsNoTracking().FirstOrDefaultAsync());
        }
    }
}
