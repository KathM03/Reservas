using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVACIONES.DTO;
using RESERVACIONES.Models;

namespace RESERVACIONES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturacionsController : ControllerBase
    {
        private readonly ReservasDbContext _context;
        private static readonly HashSet<string> EstadoFact = new() { "PAGADO", "PENDIENTE" };

        public FacturacionsController(ReservasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetFacturacions()
        {
          if (_context.Facturacions == null)
          {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron facturas."
                });
            }
            try
            {
                var facturas = await _context.Facturacions.Where(e => e.Estado != "N").ToListAsync();
                return Ok(new
                {
                    success = true,
                    message = "Mostrando la lista de facturas:",
                    result = facturas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al obtener las facturas.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Facturacion>> GetFacturacion(int id)
        {
            if (_context.Facturacions == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron facturas."
                });
            }
            var facturacion = await _context.Facturacions.Where(e => e.IdFact == id).FirstOrDefaultAsync();

            if (facturacion == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró la factura con el id {id}."
                });
            }

            return Ok(new
            {
                success = true,
                message = $"Mostrando la factura con el id {id}",
                result = facturacion
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacturacion(int id, [FromBody] FacturacionDTO facturacionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facturaExistente = await _context.Facturacions.Where(e => e.IdReserva == id).FirstOrDefaultAsync();

            if (facturaExistente == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró factura con el id {id}"
                });
            }

            if (!string.IsNullOrEmpty(facturacionDto.Estado) && !EstadoFact.Contains(facturacionDto.Estado.ToUpper()))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede actualizar la factura. El estado no es permitido."
                });
            }

            facturaExistente.IdReserva = facturacionDto.IdReserva ?? facturaExistente.IdReserva;
            facturaExistente.Total = facturacionDto.Total ?? facturaExistente.Total;
            facturaExistente.Fecha = facturacionDto.Fecha ?? facturaExistente.Fecha;
            facturaExistente.Estado = facturacionDto.Estado ?? facturaExistente.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al actualizar la factura.",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                success = true,
                message = "Factura Actualizada.",
                result = facturacionDto
            });
        }


        [HttpPost]
        public async Task<ActionResult<Facturacion>> PostFacturacion([FromBody] FacturacionDTO facturacionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(facturacionDto.Estado) && !EstadoFact.Contains(facturacionDto.Estado.ToUpper()))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede crear la factura. El estado no es permitido."
                });
            }

            try
            {
                var facturacion = new Facturacion
                {
                    IdReserva = (int)facturacionDto.IdReserva,
                    Total = facturacionDto.Total,
                    Fecha = facturacionDto.Fecha,
                    Estado = facturacionDto.Estado
                };

                _context.Facturacions.Add(facturacion);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFacturacion", new { id = facturacion.IdFact }, facturacion);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al crear la factura.",
                    error = ex.Message
                });
            }
            
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacturacion(int id)
        {
            if (_context.Facturacions == null)
            {
                return NotFound();
            }
            var facturacion = await _context.Facturacions.FindAsync(id);
            if (facturacion == null)
            {
                return NotFound();
            }

            _context.Facturacions.Remove(facturacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturacionExists(int id)
        {
            return (_context.Facturacions?.Any(e => e.IdFact == id)).GetValueOrDefault();
        }
    }
}
