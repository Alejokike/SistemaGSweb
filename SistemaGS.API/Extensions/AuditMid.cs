using SistemaGS.DTO.ModelDTO;
using SistemaGS.Repository.DBContext;
using SistemaGS.Service.Contrato;
using System.Security.Claims;

namespace SistemaGS.API.Extensions
{
    public class AuditMid
    {
        private readonly RequestDelegate requestDelegate;
        private readonly ISecurityService securityService;
        public AuditMid(RequestDelegate requestDelegate, ISecurityService securityService)
        {
            this.requestDelegate = requestDelegate;
            this.securityService = securityService;
        }
        public async Task Invoke(HttpContext context)
        {
            if(
                context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method ==  HttpMethods.Delete
                )
            {
                int UsuarioResponsable = Convert.ToInt32((context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? "0"));
                string? usuario = context.User.Identity?.Name ?? "Anónimo";
                PathString accion = context.Request.Path;
                string? metodo = context.Request.Method;
                DateTime fecha = DateTime.Now;

                /*
                try
                {
                    await securityService.Registrar(new RegistroDTO()
                    {
                        IdRegistro = 0,
                        TablaAfectada = ,
                        IdRegistroAfectado = ,
                        Accion = ,
                        UsuarioResponsable = ,
                        Detalle = ,
                        FechaAccion = 
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                */
            }

            await requestDelegate(context);
        }
    }
}
