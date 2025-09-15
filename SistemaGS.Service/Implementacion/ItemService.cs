using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;

namespace SistemaGS.Service.Implementacion
{
    public class ItemService : IItemService
    {
        private readonly IGenericoRepository<Item> _modelRepository;
        private readonly IMapper _mapper;
        public ItemService(IGenericoRepository<Item> modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }
        public async Task<ItemDTO> Crear(ItemDTO Model)
        {
            try
            {
                var DbModel = _mapper.Map<Item>(Model);
                var rspModel = await _modelRepository.Crear(DbModel);

                if (rspModel.IdItem != 0) return _mapper.Map<ItemDTO>(rspModel);
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Editar(ItemDTO Model, int Cedula)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.IdItem == Cedula);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    fromDBmodel = _mapper.Map<Item>(Model);
                    await Eliminar(Cedula);
                    var respuesta = await _modelRepository.Editar(fromDBmodel);

                    if (!respuesta) throw new TaskCanceledException("No se pudo editar");
                    else return respuesta;
                }
                else throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.IdItem == id);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    var respuesta = await _modelRepository.Eliminar(fromDBmodel);

                    if (!respuesta) throw new TaskCanceledException("No se pudo eliminar");
                    else return respuesta;
                }
                else throw new TaskCanceledException("No se encontraron coincidencias");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ItemDTO>> Lista(int tipo, string buscar)
        {
            try
            {
                IQueryable<Usuario>? consulta;
                /*
                if (tipo != 0)
                {
                    consulta = _modelRepository.Consultar(p => p.TipoItem == tipo && string.Concat(p.NombreCompleto.ToLower(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                else
                {
                    consulta = _modelRepository.Consultar(p => string.Concat(p.NombreCompleto.ToLower(), p.NombreUsuario.ToLower(), p.Correo.ToLower()).Contains(buscar.ToLower()));
                }
                */

                List<ItemDTO> lista = _mapper.Map<List<ItemDTO>>(await consulta.ToListAsync());

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ItemDTO> Obtener(int id)
        {
            try
            {
                var consulta = _modelRepository.Consultar(p => p.IdItem == id);
                var fromDBmodel = await consulta.FirstOrDefaultAsync();

                if (fromDBmodel != null)
                {
                    return _mapper.Map<ItemDTO>(fromDBmodel);
                }
                else throw new TaskCanceledException("Esta persona no existe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
