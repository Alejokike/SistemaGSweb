using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using SistemaGS.Util;

namespace SistemaGS.Service.Implementacion
{
    public class AyudaService : IAyudaService
    {
        private readonly IGenericoRepository<Ayuda> _modelRepository;
        private readonly IAyudaRepository _AyudaRepository;
        private readonly IGenericoRepository<Persona> _PersonaRepository;
        private readonly IGenericoRepository<Item> _InventarioRepository;

        private readonly IMapper _mapper;
        public AyudaService(IGenericoRepository<Ayuda> modelRepository, IAyudaRepository AyudaRepository, IGenericoRepository<Persona> personaRepository, IGenericoRepository<Item> InventarioRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _AyudaRepository = AyudaRepository;
            _PersonaRepository = personaRepository;
            _InventarioRepository = InventarioRepository;

            _mapper = mapper;
        }
        public async Task<AyudaDTO> Crear(AyudaDTO Model)
        {
            try
            {
                Ayuda transform = _mapper.Map<Ayuda>(Model);
                transform.Funcionario = Model.Funcionario == 0 ? null : Model.Funcionario;
                transform.Detalle = JsonConvert.SerializeObject(Model.Detalle);
                transform.ListaItems = JsonConvert.SerializeObject(Model.ListaItems);

                var responseDB = await _modelRepository.Crear(transform);
                if (responseDB != null) return _mapper.Map<AyudaDTO>(responseDB);
                else throw new TaskCanceledException("No se pudo crear");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Editar(AyudaDTO Model)
        {
            try
            {
                if (await _modelRepository.Consultar(a => a.IdAyuda == Model.IdAyuda).AnyAsync())
                {
                    Ayuda transform = _mapper.Map<Ayuda>(Model);
                    transform.Funcionario = Model.Funcionario == 0 ? null : Model.Funcionario;
                    transform.Detalle = JsonConvert.SerializeObject(Model.Detalle);
                    transform.ListaItems = JsonConvert.SerializeObject(Model.ListaItems);
                    return await _modelRepository.Editar(transform);
                } 
                else throw new TaskCanceledException("La ayuda seleccionada no existe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> Eliminar(int idAyuda)
        {
            try
            {
                var model = await _modelRepository.Consultar(a => a.IdAyuda == idAyuda).FirstOrDefaultAsync();

                if(model != null)
                {
                    if (model.Estado != "Por Aprobar") throw new InvalidOperationException("No se puede eliminar una ayuda aprobada");
                    
                    List<ListaItemDTO>? lista = JsonConvert.DeserializeObject<List<ListaItemDTO>>(model.ListaItems!)!;

                    if (lista != null && lista.Any(l => l.CantidadEntregada > 0)) throw new TaskCanceledException("La ayuda tiene stock asignado, devuelvalo al inventario antes de eliminar");

                    return await _modelRepository.Eliminar(model);
                }
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<byte[]> Imprimir(int idAyuda, int option, AyudaQuery filtro)
        {
            try
            {
                byte[] documento;
                switch (option)
                {
                    case 1:
                        {
                            var ayuda = await Obtener(idAyuda);
                            PersonaDTO Solicitante = _mapper.Map<PersonaDTO>(_PersonaRepository.Consultar(p => p.Cedula == ayuda.Solicitante).FirstOrDefault() ?? throw new TaskCanceledException("El solicitante de la ayuda no existe"));
                            PersonaDTO? Funcionario = _mapper.Map<PersonaDTO>(_PersonaRepository.Consultar(p => p.Cedula == ayuda.Funcionario).FirstOrDefault()); //?? throw new TaskCanceledException("El funcionario de la ayuda no existe"));
                            documento = Impresion.GeneratePDFplanilla(ayuda, Solicitante, Funcionario);
                            break;
                        }
                    case 2:
                        {
                            var ayuda = await Obtener(idAyuda);
                            PersonaDTO Solicitante = _mapper.Map<PersonaDTO>(_PersonaRepository.Consultar(p => p.Cedula == ayuda.Solicitante).FirstOrDefault() ?? throw new TaskCanceledException("El solicitante de la ayuda no existe"));
                            PersonaDTO? Funcionario = _mapper.Map<PersonaDTO>(_PersonaRepository.Consultar(p => p.Cedula == ayuda.Funcionario).FirstOrDefault()); //?? throw new TaskCanceledException("El funcionario de la ayuda no existe"));
                            documento = Impresion.GeneratePDFdetalle(ayuda, Solicitante, Funcionario);
                            break;
                        }
                    case 3:
                        {
                            List<AyudaDTO> ayudas = await Lista(filtro);
                            if (!ayudas.Any()) throw new TaskCanceledException("No hay casos de ayudas cerradas en el período seleccionado");
                            documento = Impresion.GeneratePDFreporte(filtro, ayudas);
                            break;
                        }
                    default:
                        {
                            throw new TaskCanceledException("Operación inválida");
                        }
                }
                return documento;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }        
        }
        public async Task<List<AyudaDTO>> Lista(AyudaQuery filtro)
        {
            try
            {
                var model = await _AyudaRepository.Listar(filtro);
                if (model != null) 
                {
                    List<AyudaDTO> transform = new List<AyudaDTO>();
                    foreach (Ayuda ayuda in model)
                    {
                        AyudaDTO t = _mapper.Map<AyudaDTO>(ayuda);
                        t.Detalle = JsonConvert.DeserializeObject<Dictionary<string, string>>(ayuda.Detalle!) ?? new Dictionary<string, string>();
                        t.ListaItems = JsonConvert.DeserializeObject<List<ListaItemDTO>>(ayuda.ListaItems!) ?? new List<ListaItemDTO>();
                        transform.Add(t);
                    }
                    return transform;
                }
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<AyudaDTO> Obtener(int idAyuda)
        {
            try
            {
                var model = await _modelRepository.Consultar(a => a.IdAyuda == idAyuda).FirstOrDefaultAsync();
                if (model != null) 
                {
                    AyudaDTO transform = _mapper.Map<AyudaDTO>(model);
                    transform.Detalle = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Detalle!) ?? new Dictionary<string, string>();
                    transform.ListaItems = JsonConvert.DeserializeObject<List<ListaItemDTO>>(model.ListaItems!) ?? new List<ListaItemDTO>();

                    List<int> ids = transform.ListaItems.Select(li => li.ItemLista.IdItem).ToList();

                    Dictionary<int, ItemDTO> actualizados = _mapper.Map<Dictionary<int, ItemDTO>>(
                        await _InventarioRepository
                        .Consultar(i => ids.Contains(i.IdItem))
                        .ToDictionaryAsync(k => k.IdItem)
                        );

                    foreach (var item in transform.ListaItems)
                    {
                        if(actualizados.TryGetValue(item.ItemLista.IdItem, out ItemDTO? act))
                        {
                            item.ItemLista = act;
                        }
                    }

                    return transform;
                }
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }               

        public async Task<bool> MasiveAttack(List<AyudaDTO> ayudas)
        {
            try
            {
                List<Ayuda> insertar = new List<Ayuda>();
                foreach (var ayuda in ayudas)
                {
                    Ayuda aux = _mapper.Map<Ayuda>(ayuda);
                    aux.Detalle = JsonConvert.SerializeObject(ayuda.Detalle);
                    aux.ListaItems = JsonConvert.SerializeObject(ayuda.ListaItems);
                    insertar.Add(aux);
                }

                return await _AyudaRepository.MasiveAttack(insertar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
