using Microsoft.EntityFrameworkCore;

namespace PersonasAPI.Models
{
    public class PersonasDbContext : DbContext
    {
        public PersonasDbContext(DbContextOptions<PersonasDbContext> options) : base(options) { }
        public DbSet<Persona> Personas { get; set; }
    }
}