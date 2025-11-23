using Blazored.LocalStorage;
using Blazored.Toast.Services;
using SistemaGS.DTO.Responses;
using SistemaGS.WebAssembly.Services.Contrato;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class ListaService : IListaService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly IToastService _toastService;

        public ListaService(ILocalStorageService localStorageService, ISyncLocalStorageService syncLocalStorageService, IToastService toastService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
        }

        //public event Action MostrarItems;

        public async Task AgregarLista(ItemInventario item)
        {
            try
            {
                List<ItemInventario>? lista = await _localStorageService.GetItemAsync<List<ItemInventario>>("Lista");
                if (lista == null) lista = new List<ItemInventario>();

                //var encontrado = lista.FirstOrDefault(i => i.ItemLista!.IdItem == item.ItemLista!.IdItem);
                var encontrado = lista.FirstOrDefault(i => i.IdItem == item.IdItem);

                if (encontrado != null) lista.Remove(encontrado);

                lista.Add(item);
                await _localStorageService.SetItemAsync("Lista", lista);

                if (encontrado != null) _toastService.ShowSuccess("El ítem fué actualizado en la lista");
                else _toastService.ShowSuccess("El ítem fué añadido en la lista");

                //MostrarItems.Invoke();
            }
            catch (Exception ex)
            {
                _toastService.ShowError("No se pudo agregar a la lista");
                Console.WriteLine(ex);
            }

        }
        public int CantidadItems()
        {
            var lista = _syncLocalStorageService.GetItem<List<ItemInventario>>("Lista");
            return lista == null ? 0 : lista.Count();
        }
        public async Task EliminarLista(int idLista)
        {
            try
            {
                var lista = await _localStorageService.GetItemAsync<List<ItemInventario>>("Lista");
                if (lista != null)
                {
                    var item = lista.FirstOrDefault(i => i.IdItem == idLista);
                    if (item != null)
                    {
                        lista.Remove(item);
                        await _localStorageService.SetItemAsync("Lista", lista);
                        _toastService.ShowSuccess("El ítem fué eliminado de la lista");
                        //MostrarItems.Invoke();
                    } 
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError("No se pudo eliminar de la lista");
                Console.WriteLine(ex);
            }
        }
        public async Task LimpiarLista()
        {
            await _localStorageService.RemoveItemAsync("Lista");
            //MostrarItems.Invoke();
        }
        public async Task<List<ItemInventario>> Listar()
        {
            var lista = await _localStorageService.GetItemAsync<List<ItemInventario>>("Lista");
            if (lista == null) lista = new List<ItemInventario>();
            return lista;
        }
    }
}
