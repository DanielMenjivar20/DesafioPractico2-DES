using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehiculosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración del DbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configuración de Autenticación y Autorización
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

app.Run();