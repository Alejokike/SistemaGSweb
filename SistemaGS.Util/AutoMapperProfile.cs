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
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.IdRolNavigation!));
            CreateMap<Usuario, SesionDTO>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.IdRolNavigation));
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Rol.IdRol));

            CreateMap<Persona, PersonaDTO>();
            CreateMap<PersonaDTO, Persona>();

            CreateMap<Rol, RolDTO>();
            CreateMap<RolDTO, Rol>();

            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();

            CreateMap<Inventario, InventarioDTO>();
            CreateMap<InventarioDTO, Inventario>();

            CreateMap<Ayuda, AyudaDTO>();
            CreateMap<AyudaDTO, Ayuda>();
        }
    }
}
