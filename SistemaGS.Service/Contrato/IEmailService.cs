using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;

namespace SistemaGS.Service.Contrato
{
    public interface IEmailService
    {
        Task<bool> EnviarCorreo(SMTPhost smtphost, EmailDTO correo);
        Task<UsuarioDTO> ObtenerUsuarioByCorreo(string email);
        Task<bool> CambiarClave(UsuarioPersistent usuario);
    }
}
