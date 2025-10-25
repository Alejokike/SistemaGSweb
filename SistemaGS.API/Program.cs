using SistemaGS.Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using SistemaGS.Util;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.Implementacion;
using SistemaGS.Service.Contrato;
using SistemaGS.Service.Implementacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbsistemaGsContext>(options =>
    {
       options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
    }
);

builder.Services.AddTransient(typeof(IGenericoRepository<>), typeof(GenericoRepository<>));
builder.Services.AddScoped<IPlanillaRepository, PlanillaRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
/*
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
*/
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Nueva Politica");

app.UseAuthorization();

app.MapControllers();

app.Run();
