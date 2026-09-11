using LibrosAPI.Controllers;
using LibrosAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrosAPI.Tests;

public class LibrosControllerTest
{
    [Fact]
    public async Task PostLibro_AgregarLibro_CuandoLibroEsValido()
    {
        // Arrange
        var context = Setup.GetDatabaseContext();
        var controller = new LibrosController(context);
        var nuevoLibro = new Libro
        {
            Titulo = "Libro de prueba",
            Autor = "Autor de prueba",
            AnioPublicacion = DateTime.Now.Year
        };

        // Act
        var result = await controller.PostLibro(nuevoLibro);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var libro = Assert.IsType<Libro>(createdResult.Value);
        Assert.Equal("Libro de prueba", libro.Titulo);
    }

    [Fact]
    public async Task GetLibro_RetornaLibro_CuandoIdEsValido()
    {
        // Arrange
        var context = Setup.GetDatabaseContext();
        var controller = new LibrosController(context);
        var nuevoLibro = new Libro
        {
            Titulo = "Libro de prueba",
            Autor = "Autor de prueba",
            AnioPublicacion = DateTime.Now.Year
        };
        context.Libros.Add(nuevoLibro);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetLibro(nuevoLibro.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Libro>>(result);
        var returnValue = Assert.IsType<Libro>(actionResult.Value);
        Assert.Equal("Libro de prueba", returnValue.Titulo);
    }

    [Fact]
    public async Task GetLibro_RetornaNotFound_CuandoIdNoExiste()
    {
        // Arrange
        var context = Setup.GetDatabaseContext();
        var controller = new LibrosController(context);

        // Act
        var result = await controller.GetLibro(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostLibro_NoAgregarLibro_CuandoNoTieneTitulo()
    {
        // Arrange
        var context = Setup.GetDatabaseContext();
        var controller = new LibrosController(context);
        var nuevoLibro = new Libro
        {
            Titulo = null,
            Autor = "Autor de prueba",
            AnioPublicacion = DateTime.Now.Year
        };

        // Act
        var result = await controller.PostLibro(nuevoLibro);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task PostLibro_IncrementaConteo_CuandoSeAgregaNuevoLibro()
    {
        // Arrange
        var context = Setup.GetDatabaseContext();
        var controller = new LibrosController(context);

        var nuevoLibro1 = new Libro
        {
            Titulo = "Libro de prueba 1",
            Autor = "Autor de prueba 1",
            AnioPublicacion = DateTime.Now.Year
        };
        await controller.PostLibro(nuevoLibro1);

        var nuevoLibro2 = new Libro
        {
            Titulo = "Libro de prueba 2",
            Autor = "Autor de prueba 2",
            AnioPublicacion = DateTime.Now.Year
        };

        // Act
        await controller.PostLibro(nuevoLibro2);
        var libros = context.Libros.ToList();

        // Assert
        // Son 3 libros iniciales del Seed + 2 nuevos que agregamos = 5 en total
        Assert.Equal(5, libros.Count);
    }
}