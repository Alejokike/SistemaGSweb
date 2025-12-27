using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SistemaGS.DTO;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;

namespace SistemaGS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public EmailController(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpPost("EnviarCorreo")]
        public async Task<IActionResult> EnviarCorreo([FromBody] EmailDTO correo)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                SMTPhost config = new SMTPhost()
                {
                    host = _configuration["SMTP:host"]!,
                    port = _configuration.GetValue<int>("SMTP:port"),
                    auth = new Auth()
                    {
                        user = _configuration["SMTP:auth:user"]!,
                        pass = _configuration["SMTP:auth:pass"]!
                    }
                };

                /*
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("germaine96@ethereal.email"));
                email.To.Add(MailboxAddress.Parse("germaine96@ethereal.email"));
                email.Subject = correo.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = correo.Body.ToString() };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_configuration["SMTP:host"], _configuration.GetValue<int>("SMTP:port"), SecureSocketOptions.StartTls);
                    smtp.Authenticate(_configuration["SMTP:auth:user"], _configuration["SMTP:auth:pass"]);
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                }
                */

                response.EsCorrecto = true;
                response.Resultado = await _emailService.EnviarCorreo(config, correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.EsCorrecto = true;
                response.Resultado = true;
            }
            return Ok(response);
        }
        [HttpGet("ObtenerUsuarioByCorreo/{Correo}")]
        public async Task<IActionResult> ObtenerUsuarioByCorreo(string Correo)
        {
            var response = new ResponseDTO<UsuarioDTO>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _emailService.ObtenerUsuarioByCorreo(Correo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
        [HttpPut("CambiarClave")]
        public async Task<IActionResult> CambiarClave([FromBody] UsuarioPersistent model)
        {
            var response = new ResponseDTO<bool>();
            try
            {
                response.EsCorrecto = true;
                response.Resultado = await _emailService.CambiarClave(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.EsCorrecto = false;
                response.Mensaje = ex.Message;
            }
            return Ok(response);
        }
    }
}
