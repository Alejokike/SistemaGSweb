using SistemaGS.DTO;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.WebAssembly.Services.Contrato
{
    public interface IEmailService
    {
        Task<ResponseDTO<bool>> EnviarCorreo(EmailDTO correo);
        Task<ResponseDTO<UsuarioDTO>> ObtenerUsuarioByCorreo(string email);
        Task<ResponseDTO<bool>> CambiarClave(UsuarioDTO model);
    }
}
