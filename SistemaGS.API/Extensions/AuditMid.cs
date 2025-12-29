using SistemaGS.DTO.ModelDTO;
using SistemaGS.Repository.DBContext;
using SistemaGS.Service.Contrato;
using System.Security.Claims;

namespace SistemaGS.API.Extensions
{
    public class AuditMid
    {
        private readonly RequestDelegate requestDelegate;
        private readonly IServiceScopeFactory scopeFactory;
        public AuditMid(RequestDelegate requestDelegate, IServiceScopeFactory scopeFactory)
        {
            this.requestDelegate = requestDelegate;
            this.scopeFactory = scopeFactory;
        }
        public async Task Invoke(HttpContext context)
        {
            if(
                context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method ==  HttpMethods.Delete
                )
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();

                    try
                    {
                        //bool? autenticado = context.User.Identity?.IsAuthenticated;
                        RegistroDTO registro = new RegistroDTO()
                        {
                            TablaAfectada = context.Request.Path.Value?.Split('/')[2] ?? "",
                            IdRegistroAfectado = int.TryParse(context.Request.Path.Value?.Split('/').Last(), out int result) ? result : 0,
                            Accion = context.Request.Path.Value?.Split('/')[3] ?? "",
                            UsuarioResponsable = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                            Detalle = "Prueba",
                            FechaAccion = DateTime.Now,
                        };
                        await securityService.Registrar(registro);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            await requestDelegate(context);
        }
    }
}

//int UsuarioResponsable = Convert.ToInt32((context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? "0"));
//string? usuario = context.User.Identity?.Name ?? "Anónimo";
//PathString accion = context.Request.Path;
//string? metodo = context.Request.Method;
//DateTime fecha = DateTime.Now;