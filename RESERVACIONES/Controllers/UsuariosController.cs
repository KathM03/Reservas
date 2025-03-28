using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVACIONES.DTO;
using RESERVACIONES.Models;

namespace RESERVACIONES.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ReservasDbContext _context;
        private static readonly HashSet<string> RolesPermitidos = new() { "RECEPCIONISTA", "CLIENTE", "GERENTE" };


        public UsuariosController(ReservasDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "RecepcionistaOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron usuarios."
                });
            }
            try
            {
                var usuarios = await _context.Usuarios.Where(e => e.Estado != "N").ToListAsync();
                return Ok(new
                {
                    success = true,
                    message = "Mostrando la lista de usuarios:",
                    result = usuarios
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al obtener los usuarios.",
                    error = ex.Message
                });
            }
        }

        [Authorize(Policy = "RecepcionistaOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron usuarios."
                });
            }
            var usuario = await _context.Usuarios.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró el usuario con el id {id}."
                });
            }

            return Ok(new
            {
                success = true,
                message = $"Mostrando el usuario con id {id}",
                result = usuario
            });
        }

        [Authorize(Policy = "ClienteOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioExistente = await _context.Usuarios.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (usuarioExistente == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró el usuario con el id {id}."
                });
            }

            if (!string.IsNullOrEmpty(usuarioDto.Rol) && !RolesPermitidos.Contains(usuarioDto.Rol.ToUpper()))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se puede actualizar usuario. El rol no existe."
                });
            }

            usuarioExistente.Nombre = usuarioDto.Nombre ?? usuarioExistente.Nombre;
            usuarioExistente.Apellido = usuarioDto.Apellido ?? usuarioExistente.Apellido;

            usuarioExistente.Contraseña = usuarioDto.Contraseña ?? usuarioDto.Contraseña;

            usuarioExistente.Email = usuarioDto.Email ?? usuarioExistente.Email;
            usuarioExistente.Rol = usuarioDto.Rol ?? usuarioExistente.Rol;
            usuarioExistente.Estado = usuarioDto.Estado ?? usuarioExistente.Estado;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = $"Error al actualizar el usuario {id}",
                    error = ex.Message
                });
            }

            return Ok(new
            {
                success = true,
                message = "Usuario Actualizado.",
                result = usuarioDto
            });
        }

        [Authorize(Roles = "Cliente, Recepcionista")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioDTO usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrEmpty(usuarioDto.Rol) && !RolesPermitidos.Contains(usuarioDto.Rol.ToUpper()))
            {
                return BadRequest("No se puede crear usuario. El rol no existe.");
            }

            var usuarioExistente = await _context.Usuarios.Where(e => e.Email == usuarioDto.Email).FirstOrDefaultAsync();

            if (usuarioExistente != null)
            {
                return BadRequest("El usuario ya está registrado.");
            }
            try
            {
                var usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Apellido = usuarioDto.Apellido,
                    Email = usuarioDto.Email,
                    Contraseña = usuarioDto.Contraseña,
                    Rol = usuarioDto.Rol,
                    Estado = "A"
                };


                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = $"Error al crear el usuario.",
                    error = ex.Message
                });
            }
        }

        [Authorize(Policy = "GerenteOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontraron usuarios."
                });
            }

            var usuario = await _context.Usuarios.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"No se encontró el usuario con el id {id}."
                });
            }

            usuario.Estado = "N";

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Elemento Eliminado.",
            });
        }


    }
}
