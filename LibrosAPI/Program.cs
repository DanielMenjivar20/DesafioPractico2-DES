using Microsoft.EntityFrameworkCore;
using LibrosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Registrar el servicio con Base de Datos en Memoria
builder.Services.AddDbContext<LibrosDbContext>(options =>
    options.UseInMemoryDatabase("LibrosInMemoryDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Bloque para asegurar que los datos iniciales se carguen en memoria al iniciar la app
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibrosDbContext>();
    if (!context.Libros.Any())
    {
        context.Libros.AddRange(
            new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", AnioPublicacion = 1967 },
            new Libro { Id = 2, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", AnioPublicacion = 1605 },
            new Libro { Id = 3, Titulo = "El amor en los tiempos del cólera", Autor = "Gabriel García Márquez", AnioPublicacion = 1985 }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();