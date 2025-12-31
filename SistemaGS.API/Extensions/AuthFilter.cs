using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SistemaGS.DTO;
using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;
using System.Text.Json;

namespace SistemaGS.API.Extensions
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly IServiceScopeFactory scopeFactory;
        public AuthFilter(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();
                try
                {
                    JsonDummy bodyJson = new JsonDummy();

                    bodyJson.Accion = "POST";
                    bodyJson.status = context.HttpContext.Response.StatusCode;

                    var resultContext = await next();

                    if(resultContext.Result is ObjectResult objectResult)
                    {
                        var dto = objectResult.Value as ResponseDTO<SesionDTO>;
                        bodyJson.Detalle = JsonConvert.SerializeObject(dto);

                        RegistroDTO registro = new RegistroDTO()
                        {
                            TablaAfectada = "Usuario",
                            IdRegistroAfectado = dto?.Resultado?.Cedula ?? 0,
                            Accion = "Autorizacion",
                            UsuarioResponsable = dto?.Resultado?.Cedula ?? 0,
                            Detalle = JsonConvert.SerializeObject(bodyJson),
                            FechaAccion = DateTime.Now,
                        };

                        await securityService.Registrar(registro);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
