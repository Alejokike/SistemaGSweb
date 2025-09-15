using AutoMapper;
using SistemaGS.DTO;
using SistemaGS.Model;

/*
 * Aquí convertimos nuestros Modelos en Data Transfer Object (DTO) y viceversa mediante AutoMapper. Cabe destacar que
Las propiedades deben ser iguales para que hagan match
Tambien en dado caso se puede por medio de .ForMember(Destino  => Destino.PropiedadAIgnorar, opt => opt.Ignore())
indicar las propiedades a ignorar
*/
namespace SistemaGS.Util
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<Usuario, SesionDTO>();
            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<Persona, PersonaDTO>();
            CreateMap<PersonaDTO, Persona>();
            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();
        }
    }
}
