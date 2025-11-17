using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<List<InventarioDTO>> Lista(InventarioQuery filtro)
        {
            try
            {
                var consulta = from m in _modelRepository.Consultar()
                               join i in _ItemRepository.Consultar() on m.Item equals i.IdItem
                               where
                               filtro.FechaIni <= m.Fecha && m.Fecha <= filtro.FechaFin &&
                               (i.IdItem == 0 || i.IdItem.Equals(filtro.IdItem)) &&
                               ((filtro.filtro ?? "").ToLower().Contains(m.TipoOperacion.ToLower()) || filtro.filtro == string.Empty)
                               select new InventarioDTO
                               {
                                   IdTransaccion = m.IdTransaccion,
                                   TipoOperacion = m.TipoOperacion,
                                   Item = _mapper.Map<ItemDTO>(i),
                                   Unidad = m.Unidad,
                                   Cantidad = m.Cantidad,
                                   Concepto = m.Concepto,
                                   Fecha = m.Fecha
                               };
                if (!(await consulta.AnyAsync())) return new List<InventarioDTO>();
                return await consulta.ToListAsync();
                /*
                var listaRepository = await _InventarioRepository.Listar(JsonConvert.SerializeObject(filtro));
                List<InventarioDTO> lista = new List<InventarioDTO>();

                foreach(var t in listaRepository)
                {
                    InventarioDTO movimiento = _mapper.Map<InventarioDTO>(t.inventario);
                    movimiento.Item = _mapper.Map<ItemDTO>(t.item);
                    lista.Add(movimiento);
                }

                return lista;
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<(string, int)> ListarInventario(ItemQuery filtro)
        {
            try
            {
                return await _InventarioRepository.ListarInventario(JsonConvert.SerializeObject(filtro));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ItemDTO> ObtenerItem(int IdItem)
        {
            try
            {
                var respuesta = await _ItemRepository.Consultar(i => i.IdItem == IdItem).FirstOrDefaultAsync();
                if (respuesta == null) throw new TaskCanceledException("El ítem no fue encontrado");

                return _mapper.Map<ItemDTO>(respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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
                else throw new TaskCanceledException("No se pudo realizar la transacción");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
