using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence; // Namespace i DbContext-it
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// 1. Shtimi i konfigurimeve nga appsettings.json
var configuration = builder.Configuration;

// 2. Shtimi i DbContext dhe lidhjes me SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// 3. Regjistrimi i MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// 4. Regjistrimi i AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 5. Regjistrimi i FluentValidation
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
});

// 6. Shtimi i Controllerave
builder.Services.AddControllers();

// 7. Aktivizimi i Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware për Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
