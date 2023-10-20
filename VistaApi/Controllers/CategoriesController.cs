using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VistaApi.Data;
using VistaApi.Domain;

namespace VistaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly TrainersDbContext _context;

        public CategoriesController(TrainersDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.CategoryItemDTO>>> GetCategories()
        {
            try
            {
                var categories =  await _context.Categories.ToListAsync();
                List<DTO.CategoryItemDTO> dto = categories.Select(c => new DTO.CategoryItemDTO
                {
                  CategoryCode = c.CategoryCode,
                  CategoryName = c.CategoryName,
                }
                ).ToList();
                return Ok(dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to Provide Food Items at this time");
            }

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DTO.CategoryItemDTO>> GetCategory(string id)
        {
            // not really required but its the way i role
            if (String.IsNullOrEmpty(id) || id.Length > 15)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                var dto = new DTO.CategoryItemDTO
                {
                    CategoryCode = category.CategoryCode,
                    CategoryName = category.CategoryName,
                };

                return Ok(dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            

    
        }

        // PUT: api/Categories/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(string id, DTO.CategoryItemDTO category)
        {
            if ( String.IsNullOrEmpty(id) || id.Length > 15 || id != category.CategoryCode )
            {
                return BadRequest();
            }

            var oldCategory = await _context.Categories.FindAsync(id);

            if (oldCategory == null)
            {
                return NotFound();
            }

            oldCategory.CategoryName = category.CategoryName;
            _context.Entry(oldCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(DTO.CategoryItemDTO category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                Category newCategory = new Category
                {
                    CategoryCode = category.CategoryCode,
                    CategoryName = category.CategoryName,

                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (CategoryExists(category.CategoryCode))
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return CreatedAtAction("GetCategory", new { id = category.CategoryCode }, category);
        }

        // DELETE: api/Categories/ER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            // not really required but its the way i role
            if (String.IsNullOrEmpty(id))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        private bool CategoryExists(string id)
        {
            return (_context.Categories?.Any(e => e.CategoryCode == id)).GetValueOrDefault();
        }
    }
}
