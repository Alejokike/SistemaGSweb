using Blazored.LocalStorage;
using Blazored.Toast.Services;
using SistemaGS.DTO.ModelDTO;
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

        public async Task AgregarLista(UsuarioDTO item)
        {
            try
            {
                List<UsuarioDTO>? lista = await _localStorageService.GetItemAsync<List<UsuarioDTO>>("Usuarios");
                if (lista == null) lista = new List<UsuarioDTO>();

                //var encontrado = lista.FirstOrDefault(i => i.ItemLista!.IdItem == item.ItemLista!.IdItem);
                var encontrado = lista.FirstOrDefault(i => i.Cedula == item.Cedula);

                if (encontrado != null) lista.Remove(encontrado);

                lista.Add(item);
                await _localStorageService.SetItemAsync("Usuarios", lista);

                //if (encontrado != null) _toastService.ShowSuccess("El usuario fué actualizado");
                //else _toastService.ShowSuccess("El usuario fué añadido");

                //MostrarItems.Invoke();
            }
            catch (Exception ex)
            {
                //_toastService.ShowError("No se pudo agregar");
                Console.WriteLine(ex);
            }
        }
        public int CantidadItems()
        {
            var lista = _syncLocalStorageService.GetItem<List<UsuarioDTO>>("Usuarios");
            return lista == null ? 0 : lista.Count();
        }

        public async Task CargarLista(List<UsuarioDTO> lista)
        {
            try
            {
                await _localStorageService.SetItemAsync("Usuarios", lista);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task EliminarLista(int id)
        {
            try
            {
                var lista = await _localStorageService.GetItemAsync<List<UsuarioDTO>>("Usuarios");
                if (lista != null)
                {
                    var item = lista.FirstOrDefault(i => i.Cedula == id);
                    if (item != null)
                    {
                        lista.Remove(item);
                        await _localStorageService.SetItemAsync("Usuarios", lista);
                        //_toastService.ShowSuccess("El usuario fué eliminado");
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
            await _localStorageService.RemoveItemAsync("Usuarios");
            //MostrarItems.Invoke();
        }
        public async Task<List<UsuarioDTO>> Listar()
        {
            var lista = await _localStorageService.GetItemAsync<List<UsuarioDTO>>("Usuarios");
            if (lista == null) lista = new List<UsuarioDTO>();
            return lista;
        }
    }
}
