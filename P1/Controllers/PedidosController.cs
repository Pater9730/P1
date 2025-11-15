using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1.Models;

namespace P1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly Appdbcontext _context;

        public PedidosController(Appdbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
             return Ok(await _context.Pedidos.ToListAsync());
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }
               
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CrearPedidoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!await _context.Personas.AnyAsync(p => p.Id == dto.PersonaId))
            {
                return BadRequest("La persona no existe.");
            }
                
            var pedido = new Pedido
            {
                PersonaId = dto.PersonaId,
                Total = dto.Total,
                Fecha = DateTime.UtcNow
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return Ok(pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }
                
            if (!await _context.Personas.AnyAsync(p => p.Id == dto.PersonaId))
            {
                return BadRequest("La persona no existe.");
            }
                
            pedido.PersonaId = dto.PersonaId;
            pedido.Total = dto.Total;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }
                
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Top5")]
        public async Task<IActionResult> GetTop5()
        {
            var fecha = DateTime.UtcNow.AddDays(-30);

            var resultado = await _context.Pedidos.Where(p => p.Fecha >= fecha).GroupBy(p => p.Persona).Select(g => new PersonaTopDto
            {
                Nombre = g.Key!.Nombre,
                CantidadPedidos = g.Count(),
                Total = g.Sum(x => x.Total)

            }) 
                .OrderByDescending(x => x.Total).Take(5).ToListAsync();

            return Ok(resultado);
        }
    }
}