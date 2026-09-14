using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosAPI.Models;
using StackExchange.Redis;

namespace ProductosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductosDbContext _context;
        private readonly IConnectionMultiplexer _redis;

        public ProductosController(
            ProductosDbContext context,
            IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var dbRedis = _redis.GetDatabase();
            var cacheKey = "productos_list";
            var productosCache = await dbRedis.StringGetAsync(cacheKey);

            if (!productosCache.IsNullOrEmpty)
            {
                var listaCache = JsonSerializer.Deserialize<IEnumerable<Producto>>(productosCache!);
                return Ok(listaCache);
            }

            var productos = await _context.Productos.AsNoTracking().ToListAsync();

            await dbRedis.StringSetAsync(cacheKey, JsonSerializer.Serialize(productos), TimeSpan.FromMinutes(5));

            return Ok(productos);
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var dbRedis = _redis.GetDatabase();
            var cacheKey = $"producto_{id}";
            var productoCache = await dbRedis.StringGetAsync(cacheKey);

            if (!productoCache.IsNullOrEmpty)
            {
                var productoDeserializado = JsonSerializer.Deserialize<Producto>(productoCache!);
                if (productoDeserializado == null)
                {
                    return NotFound();
                }
                return productoDeserializado;
            }

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            await dbRedis.StringSetAsync(cacheKey, JsonSerializer.Serialize(producto), TimeSpan.FromMinutes(5));

            return producto;
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                var dbRedis = _redis.GetDatabase();
                var cacheKeyProducto = $"producto_{id}";
                var cacheKeyLista = "productos_list";

                await dbRedis.KeyDeleteAsync(cacheKeyProducto);
                await dbRedis.KeyDeleteAsync(cacheKeyLista);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var dbRedis = _redis.GetDatabase();
            var cacheKeyLista = "productos_list";
            await dbRedis.KeyDeleteAsync(cacheKeyLista);

            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            var dbRedis = _redis.GetDatabase();
            var cacheKeyProducto = $"producto_{id}";
            var cacheKeyLista = "productos_list";

            await dbRedis.KeyDeleteAsync(cacheKeyProducto);
            await dbRedis.KeyDeleteAsync(cacheKeyLista);

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}