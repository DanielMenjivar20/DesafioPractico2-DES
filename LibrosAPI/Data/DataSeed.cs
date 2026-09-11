using LibrosAPI.Models;

namespace LibrosAPI.Data
{
    public static class DataSeed
    {
        public static void Initialize(LibrosDbContext context)
        {
            // Asegura que la base de datos esté creada
            context.Database.EnsureCreated();

            // Si ya hay libros registrados, no hace nada para evitar duplicados
            if (context.Libros.Any())
            {
                return;
            }

            // Agrega los libros solicitados exactamente con los datos de tu guía
            context.Libros.AddRange(
                new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Marquez", AnioPublicacion = 1967 },
                new Libro { Id = 2, Titulo = "Don Quijote de la Mancha", Autor = "Miguel de Cervantes", AnioPublicacion = 1605 },
                new Libro { Id = 3, Titulo = "El amor en los tiempos del cólera", Autor = "Gabriel García Marquez", AnioPublicacion = 1985 }
            );

            // Guarda los cambios en la base de datos
            context.SaveChanges();
        }
    }
}