using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiculosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Configuración del DbContext con SQL Server (Paso 6 de la guía)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configuración de Autenticación y Autorización (Parte 2 de la guía)
builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Middlewares de Autenticación y Autorización (Deben ir antes de MapControllers y MapIdentityApi)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 4. Mapeo de los endpoints de Identity para /register, /login, etc.
app.MapIdentityApi<Usuario>();

// Creación automática de la base de datos y tablas si no existen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();