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
        public async Task<ActionResult<TrainerCategoryDTO>> GetCategories(int id)
        {
            Trainer? trainer = null;
            try // lets get the Trainers Categories from the Database
            {
                trainer = await GetTrainerCategories(id);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (trainer != null) // if we have a trainer construct a DTO
            {
                var dto = TrainerCategoryDTO.buildDTO(trainer);
                if (dto != null) // if its null we have no Categories for this trainer
                {
                    return Ok(dto);
                }
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            } 
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }           
        }

        private async Task<Trainer?> GetTrainerCategories(int id)
        {
            return await _context.Trainers.Include(c => c.TrainerCategories).ThenInclude(c => c.Category).SingleOrDefaultAsync(t => t.TrainerId == id);
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

            try
            {
                Trainer? trainerDetails = await GetTrainerSessionById(id);
                List<DTO.SessionBookingDTO> sessions = trainerDetails.Sessions.Select(c => new SessionBookingDTO
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

        private async Task<Trainer?> GetTrainerSessionById(int id)
        {
            return await _context.Trainers.Include(c => c.Sessions).SingleOrDefaultAsync(t => t.TrainerId == id);
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

        /// <summary>
        /// Uses a list of CategoryCodes to either Delete items NOT in the input list OR Add items IN the input list
        /// </summary>
        [HttpPut("Catergory/{TrainerId}/Edit")]
        public async Task<IActionResult> EditCatergories(int TrainerId, [FromBody] TrainerCategoryEditModel EditModel)
        {
            if (!ModelState.IsValid || TrainerId != EditModel.TrainerId)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var trainer = await _context.Trainers.FindAsync(TrainerId);
                if (trainer == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var newCats = EditModel.Categories.ToList();
                if (newCats.Count == 0 || AnyInputIsNotValid(newCats))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                // So what are this Trainers current Categories?
                var currentCats = _context.TrainerCategories.Where(m => m.TrainerId == trainer.TrainerId).ToList();

                // Deletes items in the input list
                if (currentCats.Count() > 0)
                {
                    var itemsToDelete = currentCats.Where(item => !newCats.Any(i => i == item.CategoryCode)).ToList();
                    if (itemsToDelete.Any())
                    {
                        _context.TrainerCategories.RemoveRange(itemsToDelete);
                    }
                }

                // Add items IN the input list
                foreach (var newItem in newCats)
                {
                    var exists = currentCats.Any(e => e.CategoryCode.Equals(newItem) && e.TrainerId == trainer.TrainerId);

                    if (!exists)
                    {
                        _context.TrainerCategories.Add(new TrainerCategory { TrainerId = trainer.TrainerId, CategoryCode = newItem });
                    }
                }

                _context.SaveChanges();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }       
            return Ok();
        }

        //public async Task<IActionResult> SimplerEditCategories(int TrainerId, [FromBody] TrainerCategoryEditModel EditModel)
        //{
        //    if (!ModelState.IsValid || TrainerId != EditModel.TrainerId)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var trainer = await _context.Trainers.FindAsync(TrainerId);
        //        if (trainer == null)
        //        {
        //            return NotFound();
        //        }

        //        var newCats = EditModel.Categories.ToList();
        //        if (newCats.Count == 0 || AnyInputIsNotValid(newCats))
        //        {
        //            return BadRequest();
        //        }

        //        var currentCats = _context.TrainerCategories
        //            .Where(m => m.TrainerId == trainer.TrainerId)
        //            .ToList();

        //        // Delete items not in the input list
        //        var itemsToDelete = currentCats
        //            .Where(item => !newCats.Contains(item.CategoryCode))
        //            .ToList();

        //        if (itemsToDelete.Any())
        //        {
        //            _context.TrainerCategories.RemoveRange(itemsToDelete);
        //        }

        //        // Add items in the input list
        //        foreach (var newItem in newCats)
        //        {
        //            var exists = currentCats.Any(e => e.CategoryCode == newItem);

        //            if (!exists)
        //            {
        //                _context.TrainerCategories.Add(new TrainerCategory { TrainerId = trainer.TrainerId, CategoryCode = newItem });
        //            }
        //        }

        //        _context.SaveChanges();
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //    return Ok();
        //}



        // POST: api/Trainers
        /// <summary>
        /// Add a Trainer to the Collection
        /// </summary>
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

        private bool CategoryDoesNotExist(string id)
        {
            return !_context.Categories.Any(e => e.CategoryCode == id);
        }

        private bool AnyInputIsNotValid(List<string> inputCodes)
        {
            return inputCodes.Any(code => !_context.Categories.Any(e => e.CategoryCode == code));
        }
    }
}
