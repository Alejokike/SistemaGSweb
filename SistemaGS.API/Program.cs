using SistemaGS.Repository.DBContext;
using Microsoft.EntityFrameworkCore;

using SistemaGS.Util;
using SistemaGS.Repository.Contrato;
using SistemaGS.Repository.Implementacion;
using SistemaGS.Service.Contrato;
using SistemaGS.Service.Implementacion;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SistemaGS.API.Extensions;
using SistemaGS.API.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerAuth();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddScoped<DataAccess>();

builder.Services.AddDbContext<DbsistemaGsContext>(options =>
    {
       options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
    }
);

builder.Services.AddScoped<TokenProvider>();

builder.Services.AddTransient(typeof(IGenericoRepository<>), typeof(GenericoRepository<>));
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAyudaRepository, AyudaRepository>();
builder.Services.AddScoped<ISecurityRepository, SecurityRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IAyudaService, AyudaService>();


builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<CorreoFilter>();

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

app.UseMiddleware<AuditMid>();

app.Run();