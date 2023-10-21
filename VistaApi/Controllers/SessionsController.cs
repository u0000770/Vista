using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VistaApi.Data;
using VistaApi.Domain;
using VistaApi.DTO;

namespace VistaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly TrainersDbContext _context;

        public SessionsController(TrainersDbContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionBookingDTO>>> GetSessions()
        {
          if (_context.Sessions == null)
          {
              return NotFound();
          }
            var sessions = await _context.Sessions.ToListAsync();

            var dto = sessions.Select(s => new  SessionBookingDTO
            {
                 SessionId = s.SessionId,
                  TrainerId = s.TrainerId,
                  SessionDate = s.SessionDate,
                   BookingReference = s.BookingReference,
            }).ToList();

            return Ok(dto);
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionBookingDTO>> GetSession(int id)
        {
          if (_context.Sessions == null)
          {
              return StatusCode(StatusCodes.Status500InternalServerError);
          }
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            var dto = new SessionBookingDTO
            {
                 SessionId = session.SessionId,
                  TrainerId = session.TrainerId,
                  SessionDate = session.SessionDate,
                   BookingReference = session.BookingReference,
            };
            return dto;
        }

        // PUT: api/Sessions/5
        // /sessions/{sessionId}/bookings
        [HttpPut("{sessionId}/book")]
        public async Task<IActionResult> BookSession(int sessionId, SessionBookingDTO session)
        {
            if (sessionId != session.SessionId)
            {
                return BadRequest();
            }

            var DbSessions = await _context.Sessions.FindAsync(sessionId);

            if (DbSessions == null)
            {
                return NotFound();
            }


            DbSessions.BookingReference = GenerateBookingRefrence(7);

            _context.Entry(DbSessions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPut("{sessionId}/cancel")]
        public async Task<IActionResult> CancelSession(int sessionId, SessionBookingDTO session)
        {
            if (sessionId != session.SessionId)
            {
                return BadRequest();
            }

            var DbSessions = await _context.Sessions.FindAsync(sessionId);

            if (DbSessions == null)
            {
                return NotFound();
            }


            DbSessions.BookingReference = null;

            _context.Entry(DbSessions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }


        [HttpPut("{sessionId}/edit")]
        public async Task<IActionResult> EditSession(int sessionId, SessionBookingDTO session)
        {
            if (sessionId != session.SessionId)
            {
                return BadRequest();
            }

            if (SessionClash(session))
            {
                return BadRequest();
            }

            var DbSessions = await _context.Sessions.FindAsync(sessionId);

            if (DbSessions == null)
            {
                return NotFound();
            }

            DbSessions.BookingReference = session.BookingReference;
            DbSessions.TrainerId = session.TrainerId;
            DbSessions.SessionDate = session.SessionDate;

            _context.Entry(DbSessions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(sessionId))
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


        // POST: api/Sessions
        [HttpPost]
        public async Task<ActionResult<SessionBookingDTO>> NewSession(SessionBookingDTO session)
        {
          if (_context.Sessions == null)
          {
              return Problem("Entity set 'TrainersDbContext.Sessions'  is null.");
          }

          if (SessionClash(session))
          {
               return BadRequest();
          }

            Session newSession = new Session
            {
                TrainerId = session.TrainerId,
                SessionDate = session.SessionDate,
            };
          
            _context.Sessions.Add(newSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSession", new { id = newSession.SessionId }, newSession);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(int id)
        {
            return (_context.Sessions?.Any(e => e.SessionId == id)).GetValueOrDefault();
        }

        private bool SessionClash(SessionBookingDTO booking)
        {
            return  _context.Sessions.Any(e => e.TrainerId == booking.TrainerId && e.SessionDate == booking.SessionDate);
        }

        static string GenerateBookingRefrence(int length)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                result.Append(characters[index]);
            }

            return result.ToString();
        }


    }
}
