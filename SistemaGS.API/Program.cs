using SistemaGS.Repository.DBContext;
using Microsoft.EntityFrameworkCore;

using SistemaGS.Util;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.Implementacion;
using SistemaGS.Service.Contrato;
using SistemaGS.Service.Implementacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbsistemaGsContext>(options =>
    {
       options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
    }
);

builder.Services.AddTransient(typeof(IGenericoRepository<>), typeof(GenericoRepository<>));
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAyudaRepository, AyudaRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IAyudaService, AyudaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Nueva Politica",app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Nueva Politica");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();