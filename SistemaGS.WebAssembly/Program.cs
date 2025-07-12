using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaGS.WebAssembly;

using Blazored.LocalStorage;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5206/api/"/*builder.HostEnvironment.BaseAddress*/) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
