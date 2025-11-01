using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;

namespace SistemaGS.Service.Implementacion
{
    public class InventarioService : IInventarioService
    {
        private readonly IGenericoRepository<Inventario> _modelRepository;
        private readonly IGenericoRepository<Item> _ItemRepository;
        private readonly IInventarioRepository _InventarioRepository;

        private readonly IMapper _mapper;
        public InventarioService(IGenericoRepository<Inventario> modelRepository, IGenericoRepository<Item> ItemRepository, IInventarioRepository InventarioRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _ItemRepository = ItemRepository;
            _InventarioRepository = InventarioRepository;
            _mapper = mapper;
        }

        public async Task<bool> Desbloquear(List<InventarioDTO> movimientos, AyudaDTO ayuda)
        {
            try
            {
                List<Inventario> auxm = _mapper.Map<List<Inventario>>(movimientos);
                Ayuda auxA = _mapper.Map<Ayuda>(ayuda);
                bool responseDB = await _InventarioRepository.Desbloquear(auxm, auxA);
                if (responseDB) return true;
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public async Task<List<InventarioDTO>> Lista(int IdItem, string filtro, DateTime? FechaIni, DateTime? FechaFin)
        {
            try
            {
                IQueryable<Inventario>? consulta;

                if (IdItem != 0)
                {
                    consulta = _modelRepository.Consultar(p => p.Item == IdItem && string.Concat(p.TipoOperacion, p.Fecha).Contains(filtro.ToLower()));
                }
                else
                {
                    consulta = _modelRepository.Consultar(p => string.Concat(p.TipoOperacion, p.Fecha).Contains(filtro.ToLower()));
                }
                return _mapper.Map<List<InventarioDTO>>(await consulta.ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<InventarioDTO> Obtener(int IdTransaccion)
        {
            try
            {
                var consulta = _mapper.Map<InventarioDTO>(await _modelRepository.Consultar(i => i.IdTransaccion == IdTransaccion).AsNoTracking().FirstOrDefaultAsync());
                consulta.Item = _mapper.Map<ItemDTO>(await _ItemRepository.Consultar(i => i.IdItem == consulta.Item.IdItem).AsNoTracking().FirstOrDefaultAsync());

                if (consulta != null || consulta.Item != null) return consulta;
                else throw new TaskCanceledException("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<InventarioDTO> Registrar(InventarioDTO Transaccion)
        {
            try
            {
                Inventario auxIn = _mapper.Map<Inventario>(Transaccion);
                Item auxIt = _mapper.Map<Item>(Transaccion.Item);
                bool responseDB = await _InventarioRepository.Registrar(auxIn, auxIt);
                if (responseDB) return Transaccion;
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
