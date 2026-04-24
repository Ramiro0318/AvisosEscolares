using AvisosAPI.Mappers;
using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using AvisosAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
