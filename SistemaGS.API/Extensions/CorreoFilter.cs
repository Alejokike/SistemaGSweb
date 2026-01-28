using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaGS.DTO;
using SistemaGS.DTO.Email;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SistemaGS.API.Extensions
{
    public class CorreoFilter : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ISecurityService _securityService;
        private readonly IAyudaService _ayudaService;

        private SesionDTO UsuarioAccion;
        private AyudaDTO? Ayuda { get; set; } = null;

        public CorreoFilter(IConfiguration configuration, IEmailService emailService, ISecurityService securityService, IAyudaService ayudaService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _securityService = securityService;
            _ayudaService = ayudaService;

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
                                <html><body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#9B2873; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - ¡Solicitud Enviada! </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Se ha abierto una solicitud de ayuda a su nombre bajo el Nro: <span style=""font-weight:bold; color:#00B3CB;"">{ayuda.IdAyuda}</span> </p> <p style=""margin-top:15px; line-height:1.5;""> Ingrese al sistema para obtener más detalles sobre su solicitud. En caso de no haber realizado ninguna solicitud, contacte con soporte. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FF315F; color:#f8f9fa; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
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
                                <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#FF315F; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - Aprobación Revocada </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Lamentamos informarle que la aprobación de su solicitud de ayuda Nro <span style=""font-weight:bold; color:#00B3CB;"">{ayuda.IdAyuda}</span> fue revocada. </p> <p style=""margin-top:15px; line-height:1.5;""> Ingrese al sistema para obtener más detalles sobre su solicitud. En caso de no haber realizado ninguna solicitud, contacte con soporte. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FFD515; color:#333; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem; font-weight:bold;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                                "
                            };
                        }
                        break;
                    }
                case "En Proceso":
                    {
                        if(UsuarioAccion.Rol.Nombre == "Administrador")
                        {
                            email = new EmailDTO()
                            {
                                To = solicitante?.Correo ?? "",
                                From = "sistema@sistemags.gov",
                                Subject = "Sistema GS - ¡Su Solicitud ha sido Aprobada!",
                                Body = $@"
                                <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#00B3CB; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - ¡Solicitud Aprobada! </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Nos complace informarle que la solicitud de ayuda a su nombre bajo el Nro <span style=""font-weight:bold; color:#FFD515;"">{ayuda.IdAyuda}</span> fue aprobada. </p> <p style=""margin-top:15px; line-height:1.5;""> Comuníquese a la brevedad con el funcionario a cargo. Ingrese al sistema para obtener más detalles sobre su solicitud. En caso de no haber realizado ninguna solicitud, contacte con soporte. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FF315F; color:#f8f9fa; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                                "
                            };
                        }
                        else
                        {
                            email = new EmailDTO()
                            {
                                To = "administrador@sistemags.gov",
                                From = "sistema@sistemags.gov",
                                Subject = $"Sistema GS - Stock Retirado Ayuda {ayuda.IdAyuda}",
                                Body = $@"
                                <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#FFD515; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#9B2873;""> Sistema GS - Stock Retirado Ayuda {ayuda.IdAyuda} </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Se ha retirado todo el stock de la ayuda bajo el Nro <span style=""font-weight:bold; color:#00B3CB;"">{ayuda.IdAyuda}</span>. Por lo que su estado de proceso vuelve al punto anterior. </p> <p style=""margin-top:15px; line-height:1.5;""> Ingrese al sistema para obtener más detalles sobre la solicitud. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FF315F; color:#f8f9fa; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                                "
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
                            Body = $@"
                            <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#9B2873; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - ¡Su ayuda está lista! </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Su solicitud de ayuda ya se encuentra lista. Comuníquese con el funcionario a cargo para culminar los detalles de la entrega. </p> <p style=""margin-top:15px; line-height:1.5;""> Verifique los detalles de la solicitud en el sistema. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#00B3CB; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FFD515; color:#333; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem; font-weight:bold;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                            "
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
                            Body = $@"
                            <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#FF315F; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - Solicitud Cerrada </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Se ha cerrado su solicitud bajo el Nro <span style=""font-weight:bold; color:#00B3CB;"">{ayuda.IdAyuda}</span>, le agradecemos por su confianza a lo largo de este proceso. </p> <p style=""margin-top:15px; line-height:1.5;""> Verifique los detalles de la solicitud en el sistema. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FFD515; color:#333; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem; font-weight:bold;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                            "

                        };
                        break;
                    }
                case "Rechazada":
                    {
                        email = new EmailDTO()
                        {
                            To = solicitante?.Correo ?? "",
                            From = "sistema@sistemags.gov",
                            Subject = "Sistema GS - Solicitud Rechazada",
                            Body = $@"
                            <html> <body style=""font-family: Open Sans, sans-serif; color:#333; margin:0; padding:0; background-color:#f4f4f4;""> <div style=""max-width:600px; margin:30px auto; padding:20px; border:1px solid #ddd; border-radius:10px; background-color:#fff;""> <!-- Header --> <div style=""background-color:#FF315F; padding:15px; border-radius:8px 8px 0 0; text-align:center;""> <h2 style=""margin:0; font-family: Montserrat, sans-serif; color:#f8f9fa;""> Sistema GS - Solicitud Rechazada </h2> </div> <!-- Body --> <div style=""padding:20px;""> <h3 style=""font-family: Montserrat, sans-serif; color:#9B2873; margin-top:0;""> ¡Hola {solicitante?.NombreUsuario}! </h3> <p style=""font-size:1.1rem; line-height:1.5; margin:0;""> Lamentamos informarle que su solicitud bajo el Nro <span style=""font-weight:bold; color:#00B3CB;"">{ayuda.IdAyuda}</span> ha sido rechazada. </p> <p style=""margin-top:15px; line-height:1.5;""> En caso de creer que esta resolución es un error, comuníquese con nosotros. Verifique los detalles de la solicitud en el sistema. </p> <!-- Botón --> <div style=""margin-top:25px; text-align:center;""> <a href=""https://wa.me/584147913306"" style=""background-color:#9B2873; color:#f8f9fa; padding:12px 24px; text-decoration:none; border-radius:5px; font-weight:bold; display:inline-block;""> Contactar soporte </a> </div> </div> <!-- Footer --> <div style=""background-color:#FFD515; color:#333; text-align:center; padding:12px; border-radius:0 0 8px 8px; font-size:0.9rem; font-weight:bold;""> © 2025 Sistema GS - Todos los derechos reservados </div> </div> </body> </html>
                            "
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
                            Body = "Este correo no debería existir, verifique los logs en caso de error."
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
                //if (context.ActionArguments.TryGetValue("IdAyuda", out object? ayudaIdObj) && ayudaIdObj is not null && ayudaIdObj is int idAyuda)
                try
                {
                    Ayuda = await _ayudaService.Obtener(Ayuda.IdAyuda);
                }
                catch (Exception)
                {
                    if(okResult.Value is ResponseDTO<AyudaDTO> ayudacreada)
                    {
                        var idAyuda = ayudacreada.Resultado?.IdAyuda ?? 0;

                        Ayuda = await _ayudaService.Obtener(idAyuda);
                    }
                    else Ayuda = context.ActionArguments.TryGetValue("ayuda", out var parametro) ? (AyudaDTO?) parametro : new AyudaDTO();
                }

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

                SesionDTO? solicitante = new SesionDTO();
                SesionDTO? funcionario = new SesionDTO();

                try
                {
                    solicitante = await UsuarioAyuda(Ayuda!.Solicitante);
                    funcionario = await UsuarioAyuda(Ayuda!.Funcionario);
                }
                catch (Exception)
                {
                    Console.WriteLine("El error esta aca");
                }

                try
                {
                    await _emailService.EnviarCorreo(smtphost, CuerpoCorreo(Ayuda!, solicitante, context.HttpContext.Request.Method, funcionario));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
