using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TrainersController : ControllerBase
    {
        private readonly TrainersDbContext _context;

        public TrainersController(TrainersDbContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        /// <summary>
        /// Returns a List of TrainerItemDTO
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.TrainerItemDTO>>> GetTrainers()
        {
            try
            {
                var trainers = await _context.Trainers.ToListAsync();
                List<DTO.TrainerItemDTO> dto = trainers.Select(c => new DTO.TrainerItemDTO
                {
                      TrainerId = c.TrainerId,
                       Name = c.Name,
                        Location = c.Location,
                }
                ).ToList();
                return Ok(dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to Provide Data at this time");
            }
        }

        // GET: api/Trainers/5
        /// <summary>
        /// Returns a specific of TrainerCategoryDTO when provided with a valid id.
        /// TrainerCategoryDTO contains Core Trainer Info plus a list of CategoryItemDTO that Trainer is assocaited with.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Categories")]
        public async Task<ActionResult<TrainerCategoryDTO>> GetTrainerCategories(int id)
        {

            if (!int.TryParse(id.ToString(), out _))
            {
                return BadRequest();
            }


            var trainerDetails = await _context.Trainers.Include(c => c.TrainerCategories).ThenInclude(c => c.Category).SingleOrDefaultAsync(t =>t.TrainerId == id);

            List<DTO.CategoryItemDTO> categories = null;
            try
            {
                categories = trainerDetails.TrainerCategories.Select(c => new CategoryItemDTO
                {
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.Category.CategoryName
                }).ToList();
                if (trainerDetails != null)
                {
                    TrainerCategoryDTO dto = new TrainerCategoryDTO
                    {
                        TrainerId = id,
                        Name = trainerDetails.Name,
                        Location = trainerDetails.Location,
                        Categories = categories
                    };

                    return Ok(dto);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent,"No Categories for this Trainer");
                }
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            
        }

        /// <summary>
        /// Returns a specific of TrainerSessionDTO when provided with a valid id.
        /// TrainerSessionDTO contains Core Trainer Info plus a list of SessionBookingDTO that Trainer is assocaited with.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Sessions")]
        public async Task<ActionResult<TrainerSessionDTO>> GetTrainerSessions(int id)
        {

            if (!int.TryParse(id.ToString(), out _))
            {
                return BadRequest();
            }

            var trainerDetails = await _context.Trainers.Include(c => c.Sessions).SingleOrDefaultAsync(t => t.TrainerId == id);

            List<DTO.SessionBookingDTO> sessions = null;
            try
            {
                sessions = trainerDetails.Sessions.Select(c => new SessionBookingDTO
                {
                     SessionId = c.SessionId,
                      BookingReference = c.BookingReference,
                         SessionDate = c.SessionDate,

                }).ToList();
                if (trainerDetails != null)
                {
                    TrainerSessionDTO dto = new TrainerSessionDTO
                    {
                        TrainerId = id,
                        Name = trainerDetails.Name,
                        Location = trainerDetails.Location,
                         Sessions = sessions
                    };

                    return Ok(dto);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent, "No Sessions for this Trainer");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }



        // PUT: api/Trainers/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trainer"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(int id, Trainer trainer)
        {
            if (id != trainer.TrainerId)
            {
                return BadRequest();
            }

            _context.Entry(trainer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
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


        [HttpPut("Catergory/{TrainerId}/edit")]
        public async Task<IActionResult> EditCatergories(int TrainerId, [FromBody] TrainerCategoryEditModel EditModel)
        {
            var trainer = await _context.Trainers.FindAsync(TrainerId);
            if (trainer == null)
            {
                return NotFound();
            }

            
            var newCats = EditModel.Categories.ToList();

            var currentCats = _context.TrainerCategories.Where(m => m.TrainerId == trainer.TrainerId);

            // Delete items NOT in the inputlist
            var itemsToDelete = currentCats.Where(item => !newCats.Any(i => i.Equals(item.CategoryCode) && item.TrainerId == trainer.TrainerId)).ToList();

            if (itemsToDelete.Any())
            {
                _context.TrainerCategories.RemoveRange(itemsToDelete);
            }

            // Add items IN the input list
            foreach (var newItem in newCats)
            {
                var exists = currentCats.Any(e => e.CategoryCode.Equals(newItem) && e.TrainerId == trainer.TrainerId);

                if (!exists)
                {
                    _context.TrainerCategories.Add(new TrainerCategory{  TrainerId = trainer.TrainerId, CategoryCode = newItem });
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            };


            return Ok();

        }


        // POST: api/Trainers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainerDTO>> PostTrainer(TrainerDTO trainer)
        {

          if  (!ModelState.IsValid)
          {
                return StatusCode(StatusCodes.Status400BadRequest);
          }

            Trainer newTrainer = new Trainer
            {
                Name = trainer.Name,
                Location = trainer.Location,
            };

            try
            {
                _context.Trainers.Add(newTrainer);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
          

            return CreatedAtAction("GetTrainer", new { id = newTrainer.TrainerId }, newTrainer);
        }

        // DELETE: api/Trainers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            if (_context.Trainers == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainerExists(int id)
        {
            return (_context.Trainers?.Any(e => e.TrainerId == id)).GetValueOrDefault();
        }
    }
}
