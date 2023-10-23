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
        /// <summary>
        /// Returns a List of CategoriesItemDTOs
        /// </summary>
        /// <returns>List of CatergoryDTO</returns>
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
        /// <summary>
        /// Returns a specific CategoryItemDTO for specific provided code > 15 char
        /// </summary>
        /// <param name="code"></param>
        /// <returns>200</returns>
        [HttpGet("{code}")]
        public async Task<ActionResult<DTO.CategoryItemDTO>> GetCategory(string code)
        {
            // not really required but its the way i role
            if (String.IsNullOrEmpty(code) || code.Length > 15)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                var category = await _context.Categories.FindAsync(code);
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
        /// <summary>
        /// Allows update of Category Name for a specified Code > 15 char
        /// </summary>
        /// <param name="code"></param>
        /// <param name="category"></param>
        /// <returns>204</returns>
        [HttpPut("{code}")]
        public async Task<IActionResult> PutCategory(string code, DTO.CategoryItemDTO category)
        {
            if ( String.IsNullOrEmpty(code) || code.Length > 15 || code != category.CategoryCode  || !ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var oldCategory = await _context.Categories.FindAsync(code);

            if (oldCategory == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
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

            return StatusCode(StatusCodes.Status204NoContent);
        }

        // POST: api/Categories
        /// <summary>
        /// Uses a CategoryItem DTO to ADD a Category to the underlying collection. 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
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

            return CreatedAtAction("GetCategory", new { code = category.CategoryCode }, category);
        }

        // DELETE: api/Categories/ER
        /// <summary>
        /// Uses Category Code to select and Remove a Category from the underlying Collection
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteCategory(string code)
        {
            // not really required but its the way i role
            if (String.IsNullOrEmpty(code))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                var category = await _context.Categories.FindAsync(code);
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
