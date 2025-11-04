using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO.ModelDTO;
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

        public async Task<bool> Desbloquear(List<InventarioDTO> movimientos, int IdAyuda)
        {
            try
            {
                List<Inventario> auxm = _mapper.Map<List<Inventario>>(movimientos);
                bool responseDB = await _InventarioRepository.Desbloquear(auxm, IdAyuda);
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
                    consulta = _modelRepository.Consultar(p => p.Item == IdItem && p.TipoOperacion.Contains(filtro.ToLower()) && (FechaIni <= p.Fecha && p.Fecha <= FechaFin));
                }
                else
                {
                    consulta = _modelRepository.Consultar(i => i.TipoOperacion.ToLower().Contains(filtro.ToLower()) && (FechaIni <= i.Fecha && i.Fecha <= FechaFin));
                }

                var check = _mapper.Map<List<InventarioDTO>>(await consulta.ToListAsync());

                if (check != null) return check;
                else throw new TaskCanceledException("No se encontraron coincidencias");
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
                var movimiento = await _modelRepository.Consultar(i => i.IdTransaccion == IdTransaccion).AsNoTracking().FirstOrDefaultAsync();
                if (movimiento == null) throw new TaskCanceledException("No se encontraron coincidencias");

                var item = await _ItemRepository.Consultar(i => i.IdItem == movimiento.Item).AsNoTracking().FirstOrDefaultAsync();
                if (item == null) throw new TaskCanceledException("No se encontraron coincidencias");

                var consulta = _mapper.Map<InventarioDTO>(movimiento);
                consulta.Item = _mapper.Map<ItemDTO>(item);

                return consulta;
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
