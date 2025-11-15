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
    public class PersonaController : ControllerBase
    {
        private readonly Appdbcontext _context;

        public PersonaController(Appdbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Personas.ToListAsync());
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var persona = await _context.Personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }
                
            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var persona = new Persona
            {
                Nombre = dto.Nombre,
                Edad = dto.Edad
            };

            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return Ok(persona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
               
            var persona = await _context.Personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }
                
            persona.Nombre = dto.Nombre;
            persona.Edad = dto.Edad;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            
            if (persona == null)
            {
                return NotFound();
            }
            
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
