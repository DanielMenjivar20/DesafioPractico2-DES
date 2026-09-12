using Microsoft.EntityFrameworkCore;
using PersonasAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos en memoria para pruebas y desarrollo
builder.Services.AddDbContext<PersonasDbContext>(options =>
    options.UseInMemoryDatabase("PersonasDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();