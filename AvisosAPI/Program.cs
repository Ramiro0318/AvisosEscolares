using AvisosAPI.Mappers;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using AvisosAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey") ?? ""));
    x.TokenValidationParameters.ValidateAudience = true;
    x.TokenValidationParameters.ValidateIssuer = true;
    x.TokenValidationParameters.ValidateLifetime = true;
    x.TokenValidationParameters.ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience");
    x.TokenValidationParameters.ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
    x.TokenValidationParameters.RoleClaimType = "Rol";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AvisosescolaresContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));

builder.Services.AddScoped<AlumnosService>();
builder.Services.AddScoped<AvisosService>();
builder.Services.AddScoped<ClasesService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IValidator<AgregarAlumnoDTO>, AgregarAlumnoValidator>();
builder.Services.AddScoped<IValidator<AgregarAvisoGeneralDTO>, AgregarAvisoGeneralValidator>();
builder.Services.AddScoped<IValidator<AgregarAvisoPersonalDTO>, AgregarAvisoPersonalValidator>();
builder.Services.AddScoped<IValidator<AgregarClaseDTO>, AgregarClaseValidator>();
builder.Services.AddScoped<IValidator<EditarAlumnoDTO>, EditarAlumnoValidator>();
builder.Services.AddScoped<IValidator<EditarClaseDTO>, EditarClaseValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginValidator>();

builder.Services.AddAutoMapper(x => { }, typeof(Program).Assembly);

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AvisosescolaresContext>();
//    db.Database.EnsureCreated();
//}

app.Run();
