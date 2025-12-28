using SistemaGS.DTO.ModelDTO;
using SistemaGS.Repository.DBContext;
using SistemaGS.Service.Contrato;
using System.Security.Claims;

namespace SistemaGS.API.Extensions
{
    public class AuditMid
    {
        private readonly RequestDelegate requestDelegate;
        public AuditMid(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            if(
                context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method ==  HttpMethods.Delete
                )
            {
                bool? autenticado = context.User.Identity?.IsAuthenticated;

                //RegistroDTO registro = new RegistroDTO()
                //{
                var TablaAfectada = context.Request.Path.Value?.Split('/')[2] ?? "";
                var IdRegistroAfectado = int.TryParse(context.Request.Path.Value?.Split('/').Last(), out int result) ? result : 0;
                var Accion = context.Request.Path.Value?.Split('/')[3] ?? "";
                var UsuarioResponsable = autenticado.HasValue && autenticado.Value ? Convert.ToInt32((context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? "0")) : 0;
                var Detalle = "Prueba";
                var FechaAccion = DateTime.Now;
                //};

                //Console.WriteLine(registro.ToString());

                //int UsuarioResponsable = Convert.ToInt32((context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? "0"));
                //string? usuario = context.User.Identity?.Name ?? "Anónimo";
                //PathString accion = context.Request.Path;
                //string? metodo = context.Request.Method;
                //DateTime fecha = DateTime.Now;

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
