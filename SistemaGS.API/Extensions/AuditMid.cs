using SistemaGS.DTO.ModelDTO;
using SistemaGS.Service.Contrato;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

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
                        JsonDummy bodyJson = new JsonDummy();
                        bodyJson.Accion = context.Request.Method;
                        bodyJson.status = context.Response.StatusCode;

                        if (
                            context.Request.Method != HttpMethods.Delete &&
                            context.Request.ContentType != null && 
                            context.Request.ContentType.Contains("application/json")
                            )
                        {
                            context.Request.EnableBuffering();

                            using (
                                var reader = new StreamReader(
                                    context.Request.Body,
                                    encoding: Encoding.UTF8,
                                    detectEncodingFromByteOrderMarks: false,
                                    bufferSize: 1024,
                                    leaveOpen: true
                                )
                            )
                            bodyJson.Detalle = await reader.ReadToEndAsync();

                            context.Request.Body.Position = 0;

                            try
                            {
                                JsonDocument.Parse(bodyJson.Detalle);
                            } catch (System.Text.Json.JsonException)
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync("Body no es JSON válido");
                                return;
                            }
                        }

                        RegistroDTO registro = new RegistroDTO()
                        {
                            TablaAfectada = context.Request.Path.Value?.Split('/')[2] ?? "",
                            IdRegistroAfectado = int.TryParse(context.Request.Path.Value?.Split('/').Last(), out int result) ? result : 0,
                            Accion = context.Request.Path.Value?.Split('/')[3] ?? "",
                            UsuarioResponsable = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
                            Detalle = JsonConvert.SerializeObject(bodyJson),
                            FechaAccion = DateTime.Now,
                        };

                        //RegistroDTO registro = new RegistroDTO();
                        //registro.TablaAfectada = context.Request.Path.Value?.Split('/')[2] ?? "";
                        //registro.IdRegistroAfectado = int.TryParse(context.Request.Path.Value?.Split('/').Last(), out int result) ? result : 0;
                        //registro.Accion = context.Request.Path.Value?.Split('/')[3] ?? "";
                        //registro.UsuarioResponsable = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
                        //registro.Detalle = body;
                        //registro.FechaAccion = DateTime.Now;

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
    internal class JsonDummy()
    {
        public string Accion { get; set; } = "Dummy";
        public string Detalle { get; set; } = "Eliminación de recurso";
        public int status { get; set; } = 0;
    }
}

//int UsuarioResponsable = Convert.ToInt32((context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value ?? "0"));
//string? usuario = context.User.Identity?.Name ?? "Anónimo";
//PathString accion = context.Request.Path;
//string? metodo = context.Request.Method;
//DateTime fecha = DateTime.Now;