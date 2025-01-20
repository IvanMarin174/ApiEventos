using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIEventos.Context;
using WebAPIEventos.Models;

namespace WebAPIEventos.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")] // Aquí se permite CORS explícitamente
    public class EventosController : ControllerBase
    {
        private readonly DataContext _context;

        public EventosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Eventoes
        [HttpGet("Usuario")]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEvents()
        {
            // Obtener el ID del usuario autenticado desde los claims
            var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
            {
                return Unauthorized(new { Message = "No se pudo obtener el ID del usuario autenticado." });
            }

            // Validar y convertir el claim en un entero
            if (!int.TryParse(usuarioIdClaim.Value, out int usuarioId))
            {
                return BadRequest(new { Message = "El ID del usuario no es válido." });
            }

            // Buscar al usuario en la base de datos
            var usuario = await _context.Usuarios
                .Include(u => u.Rol) // Incluir el rol si está relacionado
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound(new { Message = "Usuario no encontrado." });
            }
            if (usuario.Rol?.Nombre == "admin")
            {
                return Forbid();
            }

            // Obtener los eventos relacionados al usuario (puedes ajustar esta lógica)
            var eventos = await _context.Events
             .Where(e => e.UsuarioId == usuarioId)
             .Select(e => new
             {
                 e.Id,
                 e.Titulo,
                 e.Descripcion,
                 e.Lugar,
                 e.Fecha
             })
             .ToListAsync();

            // Devolver los eventos si existen
            if (eventos == null || !eventos.Any())
            {
                return NotFound(new { Message = "No se encontraron eventos para este usuario." });
            }

            return Ok(eventos);
        }


        // GET: api/Eventoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Events.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        // PUT: api/Eventoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest(new { Message = "El ID del evento no coincide con el solicitado." });
            }


            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "Evento actualizado correctamente"});
        }

        // POST: api/Eventoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Usuario/Registrar")]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {

            // Obtén el ID del usuario desde los claims
            var usuarioIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null)
            {
                return Unauthorized(new { Message = "No se pudo obtener el ID del usuario autenticado." });
            }
            if (!int.TryParse(usuarioIdClaim.Value, out int usuarioId))
            {
                return BadRequest(new { Message = "El ID del usuario no es válido." });
            }
            if (!evento.Fecha.HasValue)
            {
                return BadRequest(new { Message = "La fecha proporcionada no es válida." });
            }

            // Crear el rango del día usando la fecha proporcionada
            var inicioDia = evento.Fecha.Value.Date;
            var finDia = inicioDia.AddDays(1);

            // Comparar el rango de fechas directamente
            var eventoBd = await _context.Events
                .Where(e => e.Lugar == evento.Lugar && e.Fecha.HasValue && e.Fecha.Value >= inicioDia && e.Fecha.Value < finDia)
                .FirstOrDefaultAsync();

            if (eventoBd != null)
            {
                return BadRequest(new { Message = "Ya existe un evento con la misma fecha y lugar." });
            }

            evento.UsuarioId = usuarioId;

            _context.Events.Add(evento);
            
            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error al guardar el evento.", Details = ex.Message });
            }
            var eventoConUser = await _context.Events
                .Include(u => u.Usuario) // Incluir la relación
                .FirstOrDefaultAsync(u => u.Id == evento.Id);
            return CreatedAtAction("GetEvento", new { id = evento.Id}, new
            {
                Message = "Evento registrado correctamente",
                Evento = eventoConUser
            });
        }

        // DELETE: api/Eventoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Events.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Events.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
