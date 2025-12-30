using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Model;
using SistemaGS.Repository.Contrato;
using SistemaGS.Service.Contrato;
using SistemaGS.Util;

namespace SistemaGS.Service.Implementacion
{
    public class EmailService : IEmailService
    {
        private readonly IGenericoRepository<Usuario> _UsuarioRepository;
        private readonly IMapper _mapper;
        public EmailService(IGenericoRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _UsuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<bool> CambiarClave(UsuarioPersistent usuario)
        {
            try
            {
                if(!await _UsuarioRepository.Consultar(u => u.Cedula == usuario.Cedula && u.Activo == true).AnyAsync()) throw new TaskCanceledException("El usuario no existe");

                Usuario model = _mapper.Map<Usuario>(usuario);
                model.Clave = Ferramentas.ConvertToSha256(model.Clave + model.Cedula);

                bool responseDB = await _UsuarioRepository.Editar(model);

                if (responseDB) return true;
                else throw new TaskCanceledException("No se pudo editar");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<bool> EnviarCorreo(SMTPhost smtphost,EmailDTO correo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(correo.From));
                email.To.Add(MailboxAddress.Parse(correo.To));
                email.Subject = correo.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = correo.Body.ToString() };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(smtphost.host, smtphost.port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(smtphost.auth.user, smtphost.auth.pass);
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<UsuarioDTO> ObtenerUsuarioByCorreo(string email)
        {
            try
            {
                var usuario = await _UsuarioRepository.Consultar(u => u.Correo == email && u.Activo == true).FirstOrDefaultAsync();
                if (usuario != null) 
                {
                    string[] roles = { "Administrador", "Funcionario", "Solicitante" };
                    UsuarioDTO enviar = _mapper.Map<UsuarioDTO>(usuario);
                    enviar.Rol = new RolDTO()
                    {
                        IdRol = usuario.IdRol.HasValue ? usuario.IdRol.Value : 3,
                        Nombre = roles[usuario.IdRol.HasValue ? usuario.IdRol.Value : 3]
                    };
                    enviar.Persona = new PersonaDTO()
                    {
                        Cedula = usuario.Cedula
                    };
                    return enviar;
                } 
                else throw new TaskCanceledException("El correo no pertenece a ningún usuario");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
