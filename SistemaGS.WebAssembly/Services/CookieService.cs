using Microsoft.JSInterop;

namespace SistemaGS.WebAssembly.Services
{
    public class CookieService
    {
        private readonly IJSRuntime JS;
        public CookieService(IJSRuntime JS)
        {
            this.JS = JS;
        }
        public async Task<string> Get(string key)
        {
            try
            {
                return await JS.InvokeAsync<string>("getCookie", key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
        public async Task Remove(string key)
        {
            await JS.InvokeVoidAsync("deleteCookie", key);
        }
        public async Task Set(string key, string name, int days)
        {
            await JS.InvokeVoidAsync("setCookie", key, name, days);
        }
    }
}
