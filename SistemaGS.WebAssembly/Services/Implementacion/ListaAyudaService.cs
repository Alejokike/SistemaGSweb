using Blazored.LocalStorage;
using Blazored.Toast.Services;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.WebAssembly.Services.Contrato;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class ListaAyudaService : IListaAyudaService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly IToastService _toastService;

        public ListaAyudaService(ILocalStorageService localStorageService, ISyncLocalStorageService syncLocalStorageService, IToastService toastService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
        }

        //public event Action MostrarItems;

        public async Task AgregarLista(AyudaDTO item)
        {
            try
            {
                List<AyudaDTO>? lista = await _localStorageService.GetItemAsync<List<AyudaDTO>>("Ayudas");
                if (lista == null) lista = new List<AyudaDTO>();

                //var encontrado = lista.FirstOrDefault(i => i.ItemLista!.IdItem == item.ItemLista!.IdItem);
                var encontrado = lista.FirstOrDefault(i => i.IdAyuda == item.IdAyuda);

                if (encontrado != null) lista.Remove(encontrado);

                lista.Add(item);
                await _localStorageService.SetItemAsync("Ayudas", lista);

                if (encontrado != null) _toastService.ShowSuccess("La ayuda fué actualizada");
                else _toastService.ShowSuccess("La ayuda fué creada");

                //MostrarItems.Invoke();
            }
            catch (Exception ex)
            {
                _toastService.ShowError("No se pudo crear");
                Console.WriteLine(ex);
            }

        }
        public int CantidadItems()
        {
            var lista = _syncLocalStorageService.GetItem<List<AyudaDTO>>("Ayudas");
            return lista == null ? 0 : lista.Count();
        }
        public async Task EliminarLista(int id)
        {
            try
            {
                var lista = await _localStorageService.GetItemAsync<List<AyudaDTO>>("Ayudas");
                if (lista != null)
                {
                    var item = lista.FirstOrDefault(i => i.IdAyuda == id);
                    if (item != null)
                    {
                        lista.Remove(item);
                        await _localStorageService.SetItemAsync("Ayudas", lista);
                        _toastService.ShowSuccess("La Ayuda fué eliminada");
                        //MostrarItems.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError("No se pudo eliminar");
                Console.WriteLine(ex);
            }
        }
        public async Task LimpiarLista()
        {
            await _localStorageService.RemoveItemAsync("Ayudas");
            //MostrarItems.Invoke();
        }
        public async Task<List<AyudaDTO>> Listar()
        {
            var lista = await _localStorageService.GetItemAsync<List<AyudaDTO>>("Ayudas");
            if (lista == null) lista = new List<AyudaDTO>();
            return lista;
        }
    }
}
