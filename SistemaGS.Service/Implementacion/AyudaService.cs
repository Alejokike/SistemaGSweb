using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.DTO.Query;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;

namespace SistemaGS.Service.Implementacion
{
    public class AyudaService : IAyudaService
    {
        private readonly IGenericoRepository<Ayuda> _modelRepository;
        private readonly IAyudaRepository _AyudaRepository;

        private readonly IMapper _mapper;
        public AyudaService(IGenericoRepository<Ayuda> modelRepository, IAyudaRepository AyudaRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _AyudaRepository = AyudaRepository;
            _mapper = mapper;
        }
        public async Task<AyudaDTO> Crear(AyudaDTO Model)
        {
            try
            {
                var responseDB = await _modelRepository.Crear(_mapper.Map<Ayuda>(Model));
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
                if (await _modelRepository.Consultar(a => a.IdAyuda == Model.IdAyuda).AnyAsync()) return await _modelRepository.Editar(_mapper.Map<Ayuda>(Model));
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
        public async Task<List<AyudaDTO>> Lista(AyudaQuery filtro)
        {
            try
            {
                var model = await _AyudaRepository.Listar(filtro);

                if (model != null) return _mapper.Map<List<AyudaDTO>>(model);
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
                if (model != null) return _mapper.Map<AyudaDTO>(model);
                else throw new TaskCanceledException("No existen coincidencias");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }               
    }
}
