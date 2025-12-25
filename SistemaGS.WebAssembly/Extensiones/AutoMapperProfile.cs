using AutoMapper;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Util
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UsuarioDTO, UsuarioPersistent>();
            CreateMap<UsuarioPersistent, UsuarioDTO>();
        }
    }
}
