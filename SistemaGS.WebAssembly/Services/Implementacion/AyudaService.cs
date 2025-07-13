using Blazored.LocalStorage;
using Blazored.Toast.Services;
using SistemaGS.DTO;
using SistemaGS.WebAssembly.Services.Contrato;
using System.IO.Pipelines;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class AyudaService : IAyudaService
    {
        private ILocalStorageService _localStorageService;
        private ISyncLocalStorageService _syncLocalStorageService;
        private IToastService _toastService;

        public AyudaService(
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService,
            IToastService toastService
            )
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
        }

        public event Action MostrarItems;

        public async Task AgregarItem(ItemDTO model)
        {
            try
            {
                var ayuda = await _localStorageService.GetItemAsync<List<ItemDTO>>("ayuda");
                if (ayuda == null) ayuda = new List<ItemDTO>();

                var encontrada = ayuda.FirstOrDefault(i => i.IdItem == model.IdItem);

                if(encontrada != null) ayuda.Remove(encontrada);
                
                ayuda.Add(model);

                await _localStorageService.SetItemAsync("ayuda", ayuda);

                if (encontrada != null) _toastService.ShowSuccess("El item fue actualizado en la ayuda");
                else _toastService.ShowSuccess("El item fue agredado a la ayuda");

                MostrarItems.Invoke();
            }
            catch (Exception)
            {
                _toastService.ShowError("No se pudo agregar a la ayuda");
            }
        }

        public int CantidadItems()
        {
            var ayuda = _syncLocalStorageService.GetItem<List<ItemDTO>>("Ayuda");
            return (ayuda == null) ? 0 : ayuda.Count;
         }

        public async Task<List<ItemDTO>> DevolverItems()
        {
            var ayuda = await _localStorageService.GetItemAsync<List<ItemDTO>>("ayuda");
            if (ayuda == null) ayuda = new List<ItemDTO>();

            return ayuda;
        }

        public async Task EliminarItem(int IdItem)
        {
            try
            {
                var ayuda = await _localStorageService.GetItemAsync<List<ItemDTO>>("ayuda");

                if(ayuda != null)
                {
                    var itemeliminar = ayuda.FirstOrDefault(i => i.IdItem == IdItem);
                    if(itemeliminar != null)
                    {
                        ayuda.Remove(itemeliminar);
                        await _localStorageService.SetItemAsync("ayuda", ayuda);
                        MostrarItems.Invoke();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Limpiar()
        {
            await _localStorageService.RemoveItemAsync("ayuda");
            MostrarItems.Invoke();
        }
    }
}
