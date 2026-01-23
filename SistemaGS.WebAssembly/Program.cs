using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaGS.WebAssembly;

using Blazored.LocalStorage;
using Blazored.Toast;
using CurrieTechnologies.Razor.SweetAlert2;

using SistemaGS.WebAssembly.Services.Contrato;
using SistemaGS.WebAssembly.Services.Implementacion;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaGS.WebAssembly.Extensiones;
using SistemaGS.WebAssembly.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5006/api/") });

builder.Services.AddScoped<AuthMessageHandler>();
builder.Services.AddScoped(sp => 
{
    var handler = sp.GetRequiredService<AuthMessageHandler>();
    handler.InnerHandler = new HttpClientHandler();
    //return new HttpClient(handler) { BaseAddress = new Uri("https://4s5mmlnk-5005.use2.devtunnels.ms/api/") };
    //return new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5006/api/") };
    return new HttpClient(handler) { BaseAddress = new Uri("https://localhost:5005/api/") };
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddSweetAlert2(options => {
    options.Theme = SweetAlertTheme.Default;
    options.SetThemeForColorSchemePreference(ColorScheme.Light, SweetAlertTheme.Bootstrap4);
});
builder.Services.AddSingleton<LoadingService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IAyudaService, AyudaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddScoped<AuthenticationStateProvider, AutExt>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();