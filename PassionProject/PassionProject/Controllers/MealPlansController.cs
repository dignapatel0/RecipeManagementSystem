using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlansController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public MealPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Meal Plans
        /// </summary>
        /// <returns>A list of MealPlanDto objects</returns>
        /// <example>
        /// GET: api/MealPlans/ListMealPlans
        /// [
        ///   {"MealPlanId":1,"Name":"Weight Loss Plan","Date":"2025-02-01"},
        ///   {"MealPlanId":2,"Name":"High Protein Plan","Date":"2025-02-05"}
        /// ]
        /// </example>
        [HttpGet("ListMealPlans")]
        public async Task<ActionResult<IEnumerable<MealPlanDto>>> ListMealPlans()
        {
            var mealPlans = await _context.MealPlans.ToListAsync();

            // Create a list of MealPlanDto
            var mealPlanDtos = mealPlans.Select(mp => new MealPlanDto
            {
                MealPlanId = mp.MealPlanId,
                Name = mp.Name,
                Date = mp.Date
            }).ToList();

            return Ok(mealPlanDtos);
        }

        /// <summary>
        /// Returns a single Meal Plan by ID
        /// </summary>
        /// <param name="id">The Meal Plan ID</param>
        /// <returns>200 OK with a MealPlanDto or 404 Not Found</returns>
        /// <example>
        /// GET: api/MealPlans/FindMealPlan/1
        /// {"MealPlanId":1,"Name":"Weight Loss Plan","Date":"2025-02-01"}
        /// </example>
        [HttpGet("FindMealPlan/{id}")]
        public async Task<ActionResult<MealPlanDto>> FindMealPlan(int id)
        {
            var mealPlan = await _context.MealPlans.FindAsync(id);

            if (mealPlan == null)
            {
                return NotFound();
            }

            var mealPlanDto = new MealPlanDto
            {
                MealPlanId = mealPlan.MealPlanId,
                Name = mealPlan.Name,
                Date = mealPlan.Date
            };

            return Ok(mealPlanDto);
        }

        /// <summary>
        /// Updates a Meal Plan
        /// </summary>
        /// <param name="id">The ID of the Meal Plan</param>
        /// <param name="mealPlanDto">Updated Meal Plan details</param>
        /// <returns>204 No Content or 400/404</returns>
        /// <example>
        /// PUT: api/MealPlans/UpdateMealPlan/1
        /// Body: { "MealPlanId": 1, "Name": "Diet Plan", "Date": "2025-02-02" }
        /// </example>
        [HttpPut("UpdateMealPlan/{id}")]
        public async Task<IActionResult> UpdateMealPlan(int id, MealPlanDto mealPlanDto)
        {
            if (id != mealPlanDto.MealPlanId)
            {
                return BadRequest("ID mismatch");
            }

            var mealPlan = await _context.MealPlans.FindAsync(id);
            if (mealPlan == null)
            {
                return NotFound();
            }

            mealPlan.Name = mealPlanDto.Name;
            mealPlan.Date = mealPlanDto.Date;

            _context.Entry(mealPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealPlanExists(id))
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

        private bool MealPlanExists(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new Meal Plan
        /// </summary>
        /// <param name="mealPlanDto">New Meal Plan details</param>
        /// <returns>201 Created or 400</returns>
        /// <example>
        /// POST: api/MealPlans/AddMealPlan
        /// Body: { "Name": "Diet Plan", "Date": "2025-02-20" }
        /// </example>
        [HttpPost("AddMealPlan")]
        public async Task<ActionResult<MealPlan>> AddMealPlan(MealPlanDto mealPlanDto)
        {
            MealPlan mealPlan = new MealPlan
            {
                Name = mealPlanDto.Name,
                Date = mealPlanDto.Date
            };

            _context.MealPlans.Add(mealPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("FindMealPlan", new { id = mealPlan.MealPlanId }, mealPlanDto);
        }

        /// <summary>
        /// Deletes a Meal Plan by ID
        /// </summary>
        /// <param name="id">The Meal Plan ID</param>
        /// <returns>204 No Content or 404 Not Found</returns>
        /// <example>
        /// DELETE: api/MealPlans/DeleteMealPlan/1
        /// </example>
        [HttpDelete("DeleteMealPlan/{id}")]
        public async Task<IActionResult> DeleteMealPlan(int id)
        {
            var mealPlan = await _context.MealPlans.FindAsync(id);

            if (mealPlan == null)
            {
                return NotFound();
            }

            _context.MealPlans.Remove(mealPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a Meal Plan exists
        /// </summary>
        private bool MealPlanExist(int id)
        {
            return _context.MealPlans.Any(mp => mp.MealPlanId == id);
        }
    }
}
