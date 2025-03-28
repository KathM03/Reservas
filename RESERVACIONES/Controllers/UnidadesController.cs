using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVACIONES.Models;

namespace RESERVACIONES.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesController : ControllerBase
    {
        private readonly ReservasDbContext _context;

        public UnidadesController(ReservasDbContext context)
        {
            _context = context;
        }

        // GET: api/Unidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unidade>>> GetUnidades()
        {
          if (_context.Unidades == null)
          {
              return NotFound();
          }
            return await _context.Unidades.ToListAsync();
        }

        // GET: api/Unidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unidade>> GetUnidade(int id)
        {
          if (_context.Unidades == null)
          {
              return NotFound();
          }
            var unidade = await _context.Unidades.FindAsync(id);

            if (unidade == null)
            {
                return NotFound();
            }

            return unidade;
        }

        // PUT: api/Unidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidade(int id, Unidade unidade)
        {
            if (id != unidade.IdUnidad)
            {
                return BadRequest();
            }

            _context.Entry(unidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadeExists(id))
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

        // POST: api/Unidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unidade>> PostUnidade(Unidade unidade)
        {
          if (_context.Unidades == null)
          {
              return Problem("Entity set 'ReservasDbContext.Unidades'  is null.");
          }
            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidade", new { id = unidade.IdUnidad }, unidade);
        }

        // DELETE: api/Unidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidade(int id)
        {
            if (_context.Unidades == null)
            {
                return NotFound();
            }
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null)
            {
                return NotFound();
            }

            _context.Unidades.Remove(unidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadeExists(int id)
        {
            return (_context.Unidades?.Any(e => e.IdUnidad == id)).GetValueOrDefault();
        }
    }
}
