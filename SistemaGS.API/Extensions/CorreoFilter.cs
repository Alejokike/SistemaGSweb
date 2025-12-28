using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaGS.DTO;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;
using System.Security.Claims;

namespace SistemaGS.API.Extensions
{
    public class CorreoFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ISecurityService _securityService;

        private SesionDTO UsuarioAccion;
        private AyudaDTO? Ayuda { get; set; } = null;

        public CorreoFilter(IConfiguration configuration, IEmailService emailService, ISecurityService securityService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _securityService = securityService;

            UsuarioAccion = new SesionDTO() { Rol = new RolDTO() };
        }
        /*
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult okResult && Ayuda is not null)
            {
                SMTPhost smtphost = new SMTPhost()
                {
                    host = _configuration["SMTP:host"]!,
                    port = _configuration.GetValue<int>("SMTP:port"),
                    auth = new Auth()
                    {
                        user = _configuration["SMTP:auth:user"]!,
                        pass = _configuration["SMTP:auth:pass"]!
                    }
                };

                var solicitante = UsuarioAyuda(Ayuda.Solicitante).GetAwaiter().GetResult();
                var funcionario = UsuarioAyuda(Ayuda.Funcionario).GetAwaiter().GetResult();

                _emailService.EnviarCorreo(smtphost, CuerpoCorreo(Ayuda, solicitante, context.HttpContext.Request.Method, funcionario)).GetAwaiter().GetResult();                
            }
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Se esta ejecutando el método");
            ClaimsPrincipal? user = context.HttpContext.User;
            if (user?.Identity is not null && user.Identity.IsAuthenticated)
            {
                UsuarioAccion.Cedula = int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int cedula) ? cedula : 0;
                UsuarioAccion.NombreUsuario = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anónimo";
                UsuarioAccion.Rol.Nombre = user.FindFirst(ClaimTypes.Role)?.Value ?? "N/A";
                UsuarioAccion.Correo = user.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";
            }
            if (context.ActionArguments.TryGetValue("ayuda", out object? ayudaObj) && ayudaObj is not null && ayudaObj is AyudaDTO ayuda) Ayuda = ayuda;            
        }
        */
        private async Task<SesionDTO?> UsuarioAyuda(int cedula)
        {
            try
            {
                return await _securityService.ObtenerById(cedula);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private EmailDTO CuerpoCorreo(AyudaDTO ayuda, SesionDTO? solicitante, string httpMethod, SesionDTO? funcionario)
        {
            EmailDTO email;
            switch (ayuda.Estado)
            {
                case "Por Aprobar":
                    {
                        if(httpMethod == HttpMethods.Post)
                        {
                            email = new EmailDTO()
                            {
                                To = solicitante?.Correo ?? "",
                                From = "sistema@sistemags.gov",
                                Subject = "Sistema GS - ¡Solicitud Enviada!",
                                Body = $@"
                                <html>
                                    <body style='font-family: Open Sans, sans-serif; color:#333;'>
                                        <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #ddd; border-radius:10px;'>
                                            <h2 style='font-family: Montserrat, sans-serif; color:#9B2873;'>¡Hola {solicitante?.NombreUsuario}!</h2>
                                            <p style='font-size:1.1rem;'>
                                                Se ha abierto una solicitud de ayuda a su nombre bajo el Nro:
                                                <span style='font-weight:bold; color:#00B3CB;'>{ayuda.IdAyuda}</span>
                                            </p>
                                            <p style='margin-top:15px;'>
                                                Ingrese al sistema para obtener más detalles sobre su solicitud, en caso de no haber realizado ninguna solicitud contacte con soporte
                                            </p>
                                            <div style='margin-top:25px; text-align:center;'>
                                                <a href='https://wa.me/584147913306'
                                                   style='background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold;'>
                                                   Contactar soporte
                                                </a>
                                            </div>
                                        </div>
                                    </body>
                                </html>
                                "
                            };
                        }
                        else
                        {
                            email = new EmailDTO()
                            {
                                To = solicitante?.Correo ?? "",
                                From = "sistema@sistemags.gov",
                                Subject = "Sistema GS - Aprobación Revocada",
                                Body = $@"
                                <html>
                                    <body style='font-family: Open Sans, sans-serif; color:#333;'>
                                        <div style='max-width:600px; margin:auto; padding:20px; border:1px solid #ddd; border-radius:10px;'>
                                            <h2 style='font-family: Montserrat, sans-serif; color:#9B2873;'>¡Hola {solicitante?.NombreUsuario}!</h2>
                                            <p style='font-size:1.1rem;'>
                                                Lamentamos informarle que la aprobación de su solicitud de ayuda Nro {ayuda.IdAyuda} fué revocada
                                            </p>
                                            <p style='margin-top:15px;'>
                                                Ingrese al sistema para obtener más detalles sobre su solicitud, en caso de no haber realizado ninguna solicitud contacte con soporte
                                            </p>
                                            <div style='margin-top:25px; text-align:center;'>
                                                <a href='https://wa.me/584147913306'
                                                   style='background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold;'>
                                                   Contactar soporte
                                                </a>
                                            </div>
                                        </div>
                                    </body>
                                </html>
                                "
                            };
                        }

                        break;
                    }
                case "Por Procesar":
                    {
                        if(UsuarioAccion.Rol.Nombre == "Administrador")
                        {
                            email = new EmailDTO()
                            {
                                To = solicitante?.Correo ?? "",
                                From = "sistema@sistemags.gov",
                                Subject = "Sistema GS - ¡Su Solicitud ha sido Aprobada!",
                                Body = "Este correo no debería existir"
                            };
                        }
                        else
                        {
                            email = new EmailDTO()
                            {
                                To = "administrador@sistemags.gov",
                                From = "sistema@sistemags.gov",
                                Subject = $"Sistema GS - Stock Retirado Ayuda {ayuda.IdAyuda}",
                                Body = "Este correo no debería existir"
                            };
                        }
                            break;
                    }
                case "Lista Para Entregar":
                    {
                        email = new EmailDTO()
                        {
                            To = solicitante?.Correo ?? "",
                            From = "sistema@sistemags.gov",
                            Subject = "Sistema GS - ¡Su ayuda esta lista!",
                            Body = ""
                        };
                        break;
                    }
                case "Cerrada":
                    {
                        email = new EmailDTO()
                        {
                            To = solicitante?.Correo ?? "",
                            From = "sistema@sistemags.gov",
                            Subject = "Sistema GS - Solicitud Cerrada",
                            Body = "Este correo no debería existir"
                        };
                        break;
                    }
                case "Rechazada":
                    {
                        email = new EmailDTO()
                        {
                            To = solicitante?.Correo ?? "",
                            From = "sistema@sistemags.gov",
                            Subject = "Solicitud Rechazada",
                            Body = "Este correo no debería existir"
                        };
                        break;
                    }
                default:
                    {
                        email = new EmailDTO()
                        {
                            To = "admin@soporteit.sial",
                            From = "sistema@sistemags.gov",
                            Subject = "Error",
                            Body = "Este correo no debería existir"
                        };
                        break;
                    }
            }

            return email;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ClaimsPrincipal? user = context.HttpContext.User;
            if (user?.Identity is not null && user.Identity.IsAuthenticated)
            {
                UsuarioAccion.Cedula = int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int cedula) ? cedula : 0;
                UsuarioAccion.NombreUsuario = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anónimo";
                UsuarioAccion.Rol.Nombre = user.FindFirst(ClaimTypes.Role)?.Value ?? "N/A";
                UsuarioAccion.Correo = user.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";
            }

            if (context.ActionArguments.TryGetValue("ayuda", out object? ayudaObj) && ayudaObj is not null && ayudaObj is AyudaDTO ayuda) Ayuda = ayuda;

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okResult && Ayuda is not null)
            {
                SMTPhost smtphost = new SMTPhost()
                {
                    host = _configuration["SMTP:host"]!,
                    port = _configuration.GetValue<int>("SMTP:port"),
                    auth = new Auth()
                    {
                        user = _configuration["SMTP:auth:user"]!,
                        pass = _configuration["SMTP:auth:pass"]!
                    }
                };

                var solicitante = await UsuarioAyuda(Ayuda.Solicitante);
                var funcionario = await UsuarioAyuda(Ayuda.Funcionario);

                await _emailService.EnviarCorreo(smtphost, CuerpoCorreo(Ayuda, solicitante, context.HttpContext.Request.Method, funcionario));
            }
        }
    }
}
