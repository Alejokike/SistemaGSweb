using Microsoft.AspNetCore.WebUtilities;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;
using System.Reflection;
//using System.Reflection.Metadata.Ecma335;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class InventarioService : IInventarioService
    {
        private readonly HttpClient _httpClient;
        public InventarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO<List<InventarioDTO>>> Lista(InventarioQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["IdItem"] = filtro.IdItem.ToString(),
                ["Movimiento"] = filtro.Movimiento,
                ["Unidad"] = filtro.Unidad,
                ["FechaIni"] = filtro.FechaIni.HasValue ? filtro.FechaIni.Value.ToString("yyyy-MM-dd") : null,
                ["FechaFin"] = filtro.FechaFin.HasValue ? filtro.FechaFin.Value.ToString("yyyy-MM-dd") : null
            };
            var url = QueryHelpers.AddQueryString("Inventario/Listar", queryparams);
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<InventarioDTO>>>(url))!;
        }
        public async Task<ResponseDTO<InventarioDTO>> Obtener(int IdTransaccion)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<InventarioDTO>>($"Inventario/Obtener/{IdTransaccion}"))!;
        }
        public async Task<ResponseDTO<List<ItemDTO>>> ListarInventario(ItemQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["ID"] = filtro.ID.ToString(),
                ["Nombre"] = filtro.Nombre,
                ["Categoria"] = filtro.Categoria,
                ["Unidad"] = filtro.Unidad,
                ["Activo"] = filtro.Activo.HasValue ? filtro.Activo.Value.ToString() : null,
                ["FechaIni"] = filtro.FechaIni.HasValue ? filtro.FechaIni.Value.ToString("yyyy-MM-dd") : null,
                ["FechaFin"] = filtro.FechaFin.HasValue ? filtro.FechaFin.Value.ToString("yyyy-MM-dd") : null
            };
            var url = QueryHelpers.AddQueryString("Inventario/ListarInventario", queryparams);
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<ItemDTO>>>(url))!;
        }
        public async Task<ResponseDTO<ItemDTO>> ObtenerItem(int IdItem, string nombre = "")
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<ItemDTO>>($"Inventario/ObtenerItem/{IdItem}/{nombre}"))!;
        }
        public async Task<ResponseDTO<InventarioDTO>> Registrar(InventarioDTO mov)
        {
            var response = await _httpClient.PostAsJsonAsync("Inventario/Registrar", mov);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<InventarioDTO>>();
            return result!;
        }
        public async Task<ResponseDTO<bool>> Desbloquear(List<InventarioDTO> movimientos, int idAyuda)
        {
            var response = await _httpClient.PostAsJsonAsync($"Inventario/Desbloquear/{idAyuda}", movimientos);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }
        public async Task<ResponseDTO<bool>> Editar(ItemDTO item)
        {
            var response = await _httpClient.PutAsJsonAsync("Inventario/EditarItem", item);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }
    }
}