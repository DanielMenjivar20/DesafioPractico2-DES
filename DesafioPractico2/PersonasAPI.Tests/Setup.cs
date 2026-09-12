using Microsoft.EntityFrameworkCore;
using PersonasAPI.Models;

namespace PersonasAPI.Tests
{
    public class Setup
    {
        public static PersonasDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<PersonasDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new PersonasDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}