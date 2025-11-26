using Blazored.LocalStorage;
using Blazored.Toast.Services;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Responses;
using SistemaGS.WebAssembly.Services.Contrato;

namespace SistemaGS.WebAssembly.Services.Implementacion
{
    public class ListaInventarioService : IListaInventarioService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly IToastService _toastService;

        public ListaInventarioService(ILocalStorageService localStorageService, ISyncLocalStorageService syncLocalStorageService, IToastService toastService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
        }
        public async Task Registrar(InventarioDTO mov)
        {
            List<InventarioDTO>? respaldoMov = null;
            List<ItemInventario>? respaldoInv = null;
            try
            {
                List<InventarioDTO>? listaMov = await _localStorageService.GetItemAsync<List<InventarioDTO>>("Movimientos");
                if (listaMov == null) listaMov = new List<InventarioDTO>();
                List<ItemInventario>? listaInv = await _localStorageService.GetItemAsync<List<ItemInventario>>("Inventario");
                if (listaInv == null) listaInv = new List<ItemInventario>();

                respaldoMov = listaMov;
                respaldoInv = listaInv;

                var encontradoMov = listaMov.FirstOrDefault(i => i.IdTransaccion == mov.IdTransaccion);
                var encontradoInv = listaInv.FirstOrDefault(i => i.IdItem == mov.Item.IdItem);

                if (encontradoMov != null) throw new TaskCanceledException("El movimiento ya existe");

                switch (mov.TipoOperacion)
                {
                    case "REC":
                        {
                            if (encontradoInv != null)
                            {
                                listaInv.Remove(encontradoInv);
                                encontradoInv.Cantidad += mov.Cantidad;
                                listaInv.Add(encontradoInv);
                            }
                            else
                            {
                                listaInv.Add(new ItemInventario()
                                {
                                    IdItem = listaInv.Max(i => i.IdItem) + 1,
                                    Nombre = mov.Item.Nombre,
                                    Descripcion = mov.Item.Descripcion,
                                    Unidad = mov.Item.Unidad,
                                    Categoria = mov.Item.Categoria,
                                    Cantidad = mov.Cantidad
                                });
                            }
                                break;
                        }
                    case "DEV":
                        {
                            if(encontradoInv != null)
                            {
                                if (encontradoInv.Cantidad < mov.Cantidad) throw new TaskCanceledException("No hay stock para realizar ese movimiento");
                                listaInv.Remove(encontradoInv);
                                encontradoInv.Cantidad -= mov.Cantidad;
                                listaInv.Add(encontradoInv);
                            }
                            else
                            {
                                throw new TaskCanceledException("El ítem no existe");
                            }
                                break;
                        }
                    default:
                        {
                            throw new TaskCanceledException("Movimiento no válido");
                        }
                }

                listaMov.Add(mov);
                await _localStorageService.SetItemAsync("Movimientos", listaMov);
                await _localStorageService.SetItemAsync("Inventario", listaInv);

                //if (encontrado != null) _toastService.ShowSuccess("El usuario fué actualizado");
                //else _toastService.ShowSuccess("El usuario fué añadido");

                //MostrarItems.Invoke();
            }
            catch (Exception ex)
            {
                if(respaldoMov != null) await _localStorageService.SetItemAsync("Movimientos", respaldoMov);
                if(respaldoInv != null) await _localStorageService.SetItemAsync("Inventario", respaldoInv);
                Console.WriteLine(ex);
            }
        }
        public async Task Desbloquear(List<InventarioDTO> listaMovimientos, int idAyuda)
        {
            List<InventarioDTO>? respaldoMov = null;
            List<ItemInventario>? respaldoInv = null;
            List<AyudaDTO>? respaldoAyu = null;
            try
            {
                List<InventarioDTO>? listaMov = await _localStorageService.GetItemAsync<List<InventarioDTO>>("Movimientos");
                if (listaMov == null) listaMov = new List<InventarioDTO>();
                List<ItemInventario>? listaInv = await _localStorageService.GetItemAsync<List<ItemInventario>>("Inventario");
                if (listaInv == null) listaInv = new List<ItemInventario>();
                List<AyudaDTO>? listaAyu = await _localStorageService.GetItemAsync<List<AyudaDTO>>("Ayudas");
                if (listaInv == null) listaAyu = new List<AyudaDTO>();

                respaldoMov = listaMov;
                respaldoInv = listaInv;
                respaldoAyu = listaAyu;

                AyudaDTO? ayuda = new AyudaDTO();
                ayuda = listaAyu.FirstOrDefault(a => a.IdAyuda == idAyuda);
                if (ayuda == null) throw new TaskCanceledException("La ayuda seleccionada no existe");

                listaAyu.Remove(ayuda);

                foreach (var mov in listaMovimientos)
                {
                    var encontradoInv = listaInv!.FirstOrDefault(i => i.IdItem == mov.Item.IdItem);

                    switch (mov.TipoOperacion)
                    {
                        case "ASI":
                            {
                                if (encontradoInv != null)
                                {
                                    var itemLista = ayuda.ListaItems.FirstOrDefault(i => i.ItemLista!.IdItem == encontradoInv.IdItem);
                                    
                                    if (itemLista == null) throw new TaskCanceledException("No existe ese ítem en la lista");
                                    
                                    if (encontradoInv.Cantidad < mov.Cantidad) throw new TaskCanceledException("No hay stock para desbloquear");
                                    if ((itemLista.CantidadSolicitada - itemLista.CantidadEntregada) < mov.Cantidad) throw new TaskCanceledException("No se puede asignar más stock del solicitado");

                                    ayuda.ListaItems.Remove(itemLista);
                                    itemLista.CantidadEntregada += mov.Cantidad;
                                    ayuda.ListaItems.Add(itemLista);

                                    listaInv!.Remove(encontradoInv);
                                    encontradoInv.Cantidad -= mov.Cantidad;
                                    listaInv.Add(encontradoInv);
                                }
                                else throw new TaskCanceledException("Hay un ítem en la ayuda que no esta en el inventario");
                                break;
                            }
                        case "RET":
                            {
                                if (encontradoInv != null)
                                {
                                    var itemLista = ayuda.ListaItems.FirstOrDefault(i => i.ItemLista!.IdItem == encontradoInv.IdItem);

                                    if (itemLista == null) throw new TaskCanceledException("No existe ese ítem en la lista");

                                    if (itemLista.CantidadEntregada < mov.Cantidad) throw new TaskCanceledException("No se puede retirar más stock del entregado");

                                    ayuda.ListaItems.Remove(itemLista);
                                    itemLista.CantidadEntregada -= mov.Cantidad;
                                    ayuda.ListaItems.Add(itemLista);

                                    listaInv!.Remove(encontradoInv);
                                    encontradoInv.Cantidad += mov.Cantidad;
                                    listaInv.Add(encontradoInv);
                                }
                                else throw new TaskCanceledException("Hay un ítem en la ayuda que no esta en el inventario");
                                break;
                            }
                        default:
                            {
                                throw new TaskCanceledException("Movimiento no válido");
                            }
                    }
                    listaMov.Add(mov);
                }

                listaAyu.Add(ayuda);

                await _localStorageService.SetItemAsync("Movimientos", listaMov);
                await _localStorageService.SetItemAsync("Inventario", listaInv);
                await _localStorageService.SetItemAsync("Ayudas", listaInv);
            }
            catch (Exception ex)
            {
                if (respaldoMov != null) await _localStorageService.SetItemAsync("Movimientos", respaldoMov);
                if (respaldoInv != null) await _localStorageService.SetItemAsync("Inventario", respaldoInv);
                if (respaldoAyu != null) await _localStorageService.SetItemAsync("Ayudas", respaldoAyu);
                Console.WriteLine(ex);
            }
        }
        public int CantidadItems()
        {
            throw new NotImplementedException();
        }
        public async Task<List<ItemInventario>> ListarInventario()
        {
            var lista = await _localStorageService.GetItemAsync<List<ItemInventario>>("Inventario");
            if (lista == null) lista = new List<ItemInventario>();
            return lista;
        }
        public async Task<List<InventarioDTO>> ListarMovimientos()
        {
            var lista = await _localStorageService.GetItemAsync<List<InventarioDTO>>("Movimientos");
            if (lista == null) lista = new List<InventarioDTO>();
            return lista;
        }
    }
}
