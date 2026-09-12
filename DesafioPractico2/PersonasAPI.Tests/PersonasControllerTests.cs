using Microsoft.AspNetCore.Mvc;
using PersonasAPI.Controllers;
using PersonasAPI.Models;
using Xunit;

namespace PersonasAPI.Tests
{
    public class PersonasControllerTests
    {
        [Fact]
        public async Task PostPersona_GuardaCorrectamente_CuandoDatosSonValidos()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PersonasController(context);

            var nuevaPersona = new Persona
            {
                Nombre = "Carlos Orellana",
                Dui = "02847192-4", // Formato correcto 00000000-0
                FechaNacimiento = new DateTime(2000, 5, 15)
            };

            // Act
            var result = await controller.PostPersona(nuevaPersona);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var personaGuardada = Assert.IsType<Persona>(createdResult.Value);
            Assert.Equal("Carlos Orellana", personaGuardada.Nombre);
            Assert.Equal("02847192-4", personaGuardada.Dui);
        }

        [Fact]
        public async Task PostPersona_RetornaBadRequest_CuandoDuiEsInvalido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PersonasController(context);

            var personaInvalida = new Persona
            {
                Nombre = "Carlos Orellana",
                Dui = "123456789", // Formato incorrecto (no tiene el guión)
                FechaNacimiento = new DateTime(2000, 5, 15)
            };

            // Simulamos el fallo de validación del modelo
            controller.ModelState.AddModelError("Dui", "El formato del DUI debe ser 00000000-0.");

            // Act
            var result = await controller.PostPersona(personaInvalida);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostPersona_RetornaBadRequest_CuandoFaltaNombre()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PersonasController(context);

            var personaInvalida = new Persona
            {
                Nombre = null, // Obligatorio
                Dui = "02847192-4",
                FechaNacimiento = new DateTime(2000, 5, 15)
            };

            controller.ModelState.AddModelError("Nombre", "El nombre es obligatorio.");

            // Act
            var result = await controller.PostPersona(personaInvalida);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}