using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonasAPI.Models;

namespace PersonasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly PersonasDbContext _context;

        public PersonasController(PersonasDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Persona>> PostPersona([FromBody] Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return persona;
        }
    }
}