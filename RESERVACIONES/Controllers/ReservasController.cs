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
    public class ReservasController : ControllerBase
    {
        private readonly ReservasDbContext _context;

        private static readonly HashSet<string> TipoEstanciaP = new() { "CORTA", "LARGA" };

        public ReservasController(ReservasDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            if (_context.Reservas == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron reservas."
                });
            }
            try
            {
                var reservas = await _context.Reservas.Where(e => e.Estado != "N").ToListAsync();
                return Ok(new
                {
                    success = true,
                    message = "Mostrando la lista de reservas:",
                    result = reservas
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al obtener las reservas.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            if (_context.Reservas == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron reservas."
                });
            }

            var reserva = await _context.Reservas.Where(e => e.IdReserva == id).FirstOrDefaultAsync();

            if (reserva == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró la reserva con el id {id}."
                });
            }

            return Ok(new
            {
                success = true,
                message = $"Mostrando la reserva con el id {id}",
                result = reserva
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, [FromBody] ReservaDTO reservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservaExistente = await _context.Reservas.Where(e => e.IdReserva == id).FirstOrDefaultAsync();

            if (reservaExistente == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró reserva con el id {id}"
                });
            }

            var reservaSuperpuesta = await _context.Reservas.Where(e => e.UnidadId == reservaDto.UnidadId && ((e.FInicio < reservaDto.FFin && e.FFin > reservaDto.FInicio))).FirstOrDefaultAsync();

            if (reservaSuperpuesta != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede registrar reserva. La unidad que intenta reservar no está disponible."
                });
            }

            if (!string.IsNullOrEmpty(reservaDto.TipoEstancia) && !TipoEstanciaP.Contains(reservaDto.TipoEstancia.ToUpper()))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede actualizar la reserva. Tipo de estancia no permitida."
                });
            }

            reservaExistente.UnidadId = reservaDto.UnidadId ?? reservaExistente.UnidadId;
            reservaExistente.FInicio = reservaDto.FInicio ?? reservaExistente.FInicio;
            reservaExistente.FFin = reservaDto.FFin ?? reservaExistente.FFin;
            reservaExistente.TipoEstancia = reservaDto.TipoEstancia ?? reservaExistente.TipoEstancia;
            reservaExistente.Descuento = reservaDto.Descuento ?? reservaExistente.Descuento;
            reservaExistente.Total = reservaDto.Total ?? reservaExistente.Total;
            reservaExistente.Estado = reservaDto.Estado ?? reservaExistente.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al actualizar la reserva."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Reserva Actualizado.",
                result = reservaDto
            });
        }


        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva([FromBody] ReservaDTO reservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(reservaDto.TipoEstancia) && !TipoEstanciaP.Contains(reservaDto.TipoEstancia.ToUpper()))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede actualizar la reserva. Tipo de estancia no permitida."
                });
            }

            var reservaExistente = await _context.Reservas
                .Where(e => e.UnidadId == reservaDto.UnidadId && ((e.FInicio < reservaDto.FFin && e.FFin > reservaDto.FInicio))).FirstOrDefaultAsync();

            if (reservaExistente != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede registrar reserva. La unidad que intenta reservar no está disponible."
                });
            }

            try
            {
                var reserva = new Reserva
                {
                    UsuarioId = (int)reservaDto.UsuarioId,
                    UnidadId = (int)reservaDto.UnidadId,
                    FInicio = (DateTime)reservaDto.FInicio,
                    FFin = (DateTime)reservaDto.FFin,
                    TipoEstancia = reservaDto.TipoEstancia,
                    Descuento = 0,
                    Estado = "A"
                };
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReserva", new { id = reserva.IdReserva }, reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al crear la reserva.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            if (_context.Reservas == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron reservas."
                });
            }

            var reserva = await _context.Reservas.Where(e => e.IdReserva == id).FirstOrDefaultAsync();

            if (reserva == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró reserva con el id {id}."
                });
            }

            reserva.Estado = "N";
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Elemento Eliminado."
            });
        }

    }
}
