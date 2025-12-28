using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Repository.DBContext;
using SistemaGS.Service.Contrato;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly DbsistemaGsContext _dbContext;
        private readonly IInventarioService _inventarioService;
        public InventarioController(IInventarioService inventarioService, DbsistemaGsContext dbContext)
        {
            _inventarioService = inventarioService;
            _dbContext = dbContext;
        }
        [HttpGet("Listar")]
        public async Task<IActionResult> Lista([FromQuery] InventarioQuery filtro)
        {
            var response = new ResponseDTO<List<InventarioDTO>>();
            try
            {
                filtro.FechaIni ??= new DateTime(DateTime.Today.Year, 1, 1);
                filtro.FechaFin ??= new DateTime(DateTime.Today.Year, 12, 31);
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Lista(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("Obtener/{idTransaccion:int}")]
        public async Task<IActionResult> Obtener(int idTransaccion)
        {
            var response = new ResponseDTO<InventarioDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Obtener(idTransaccion);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("ListarInventario")]
        public async Task<IActionResult> ListarInventario([FromQuery] ItemQuery filtro)
        {
            var response = new ResponseDTO<List<ItemDTO>>();
            try
            {
                filtro.FechaIni ??= new DateTime(DateTime.Today.Year, 1, 1);
                filtro.FechaFin ??= new DateTime(DateTime.Today.Year, 12, 31);
                response.EsCorrecto = true;                
                response.Resultado = await _inventarioService.ListarInventario(filtro);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpGet("ObtenerItem/{idItem:int}/{nombre?}")]
        public async Task<IActionResult> ObtenerItem(int idItem, string nombre = "N/A")
        {
            var response = new ResponseDTO<ItemDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.ObtenerItem(idItem, nombre);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] InventarioDTO Transaccion)
        {
            var response = new ResponseDTO<InventarioDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Registrar(Transaccion);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("Desbloquear/{IdAyuda:int}")]
        public async Task<IActionResult> Desbloquear(int IdAyuda, [FromBody] List<InventarioDTO> movimientos)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _inventarioService.Desbloquear(movimientos, IdAyuda);
            }
            catch (Exception ex)
            {
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        

        //Descartado temporalmente
        [HttpPost("ItemsAjax")]
        public async Task<IActionResult> ItemsAjax([FromBody] DataTablesRequest request)
        {
            var query = _dbContext.Items.AsQueryable();
            var total = _dbContext.Items.Count();

            if (!string.IsNullOrEmpty(request.search?.value))
            {
                var searchValue = request.search.value.ToLower();
                query = query.Where(i =>
                    (i.IdItem.ToString() + i.Nombre + i.Categoria + i.Unidad)
                    .ToLower()
                    .Contains(searchValue));
            }

            var filtrados = query.Count();

            if (request.order != null && request.order.Count > 0)
            {
                var orden = request.order.First();
                var columna = request.columns[orden.column].data;

                if (orden.dir == "asc")
                    query = query.OrderBy(e => EF.Property<object>(e, columna));
                else
                    query = query.OrderByDescending(e => EF.Property<object>(e, columna));
            }

            List<ItemDTO> datos = new List<ItemDTO>();
            if(request.length == -1)
            {
                datos = await query
                .Select(i => new ItemDTO
                {
                    IdItem = i.IdItem,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Unidad = i.Unidad,
                    Descripcion = i.Descripcion,
                    Cantidad = i.Cantidad,
                    Activo = i.Activo ?? false,
                    FechaCreacion = i.FechaCreacion ?? DateTime.Today
                })
                .ToListAsync();
            }
            else
            {
                datos = await query
                .Skip(request.start)
                .Take(request.length)
                .Select(i => new ItemDTO
                {
                    IdItem = i.IdItem,
                    Nombre = i.Nombre,
                    Categoria = i.Categoria,
                    Unidad = i.Unidad,
                    Descripcion = i.Descripcion,
                    Cantidad = i.Cantidad,
                    Activo = i.Activo ?? false,
                    FechaCreacion = i.FechaCreacion ?? DateTime.Today
                })
                .ToListAsync();
            }

            var response = new DataTablesResponse<ItemDTO>
            {
                draw = request.draw,
                recordsTotal = total,
                recordsFiltered = filtrados,
                data = datos
            };

            return Ok(response);
        }
    }
}
