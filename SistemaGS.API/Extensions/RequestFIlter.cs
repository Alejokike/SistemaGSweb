using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SistemaGS.API.Extensions
{
    public class RequestFIlter : IAsyncActionFilter
    {
        private static readonly HashSet<string> _processed = new();
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-Request-ID", out var requestId)) 
            { 
                context.Result = new BadRequestObjectResult("Falta el X-Request-ID"); 
                return; 
            }

            lock (_processed)
            {
                if (_processed.Contains(requestId!))
                {
                    context.Result = new ConflictResult();
                } 
                _processed.Add(requestId!); 
            } 
            
            await next(); 
        }
    }
}
