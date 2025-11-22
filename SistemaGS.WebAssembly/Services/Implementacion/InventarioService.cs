using Microsoft.AspNetCore.WebUtilities;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.DTO.Responses;
using SistemaGS.WebAssembly.Services.Contrato;
using System.Net.Http.Json;

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
                ["IdItem"] = filtro.IdItem.ToString(),// ?? string.Empty,
                ["FechaIni"] = filtro.FechaIni.HasValue ? filtro.FechaIni.Value.ToString("yyyy-MM-dd") : null,// ?? string.Empty,
                ["FechaFin"] = filtro.FechaFin.HasValue ? filtro.FechaFin.Value.ToString("yyyy-MM-dd") : null,// ?? string.Empty,
                ["filtro"] = filtro.filtro,// ?? string.Empty,
                ["OrdenarPor"] = filtro.OrdenarPor,// ?? string.Empty,
                ["Ascendente"] = filtro.Ascendente.ToString(),
                ["Pagina"] = filtro.Pagina.ToString(),
                ["PageSize"] = filtro.PageSize.ToString()
            };

            var url = QueryHelpers.AddQueryString("Inventario/Listar", queryparams);

            return (await _httpClient.GetFromJsonAsync<ResponseDTO<List<InventarioDTO>>>(url))!;
        }
        
        public async Task<ResponseDTO<InventarioDTO>> Obtener(int IdTransaccion)
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<InventarioDTO>>($"Inventario/Obtener/{IdTransaccion}"))!;
        }
        public async Task<ResponseDTO<InventarioRespuesta>> ListarInventario(ItemQuery filtro)
        {
            var queryparams = new Dictionary<string, string?>
            {
                ["Nombre"] = filtro.Nombre,// ?? string.Empty,
                ["Categoria"] = filtro.Categoria,// ?? string.Empty,
                ["Buscar"] = filtro.Buscar,// ?? string.Empty,
                ["Unidad"] = filtro.Unidad,// ?? string.Empty,
                ["OrdenarPor"] = filtro.OrdenarPor,// ?? string.Empty,
                ["Ascendente"] = filtro.Ascendente.ToString(),
                ["Pagina"] = filtro.Pagina.ToString(),
                ["PageSize"] = filtro.PageSize.ToString()
            };

            var url = QueryHelpers.AddQueryString("Inventario/ListarInventario", queryparams);

            return (await _httpClient.GetFromJsonAsync<ResponseDTO<InventarioRespuesta>>(url))!;
        }
        public async Task<ResponseDTO<ItemDTO>> ObtenerItem(int IdItem, string nombre = "NA")
        {
            return (await _httpClient.GetFromJsonAsync<ResponseDTO<ItemDTO>>($"Inventario/ObtenerItem/{IdItem}/{nombre}"))!;
        }
        public async Task<ResponseDTO<InventarioDTO>> Registrar(InventarioDTO Transaccion)
        {
            var response = await _httpClient.PostAsJsonAsync("Inventario/Registrar", Transaccion);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<InventarioDTO>>();
            return result!;
        }
        public async Task<ResponseDTO<bool>> Desbloquear(List<InventarioDTO> movimientos, int idAyuda)
        {
            var response = await _httpClient.PostAsJsonAsync($"Inventario/Desbloquear/{idAyuda:int}", movimientos);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();
            return result!;
        }
    }
}
