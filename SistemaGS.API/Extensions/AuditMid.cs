using SistemaGS.Repository.DBContext;

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
                string? usuario = context.User.Identity?.Name ?? "Anónimo";
                PathString accion = context.Request.Path;
                string? metodo = context.Request.Method;
                DateTime fecha = DateTime.Now;
                /*
                using (var scope = context.RequestServices.CreateScope())
                {
                    var bd = scope.ServiceProvider.GetRequiredService<DbsistemaGsContext>();
                    bd.Registros.Add(
                        new
                        )
                }
                */
            }

            await requestDelegate(context);
        }
    }
}
