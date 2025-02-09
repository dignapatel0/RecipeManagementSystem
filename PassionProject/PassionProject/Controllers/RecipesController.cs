using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;


namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Recipes, each represented by a RecipeDto with their associated Meal Plan
        /// </summary>
        /// <returns>
        /// A list of recipes Dto objects
        /// </returns>
        /// <example>
        /// GET: api/Recipes/ListRecipes -> 
        /// [
        /// {"RecipeId":1,"Name":"Avocado Salad","Cuisine":"Mexican","MealPlanId":1,"MealPlanName":"Weight Loss Plan"},
        /// {"RecipeId":2,"Name":"Quinoa Bowl","Cuisine":"American","MealPlanId":1,"MealPlanName":"Weight Loss Plan"},
        /// {"RecipeId":3,"Name":"Protein Smoothie","Cuisine":"Mixed","MealPlanId":2,"MealPlanName":"High Protein Plan"}
        /// ]
        /// </example>
        [HttpGet(template: "ListRecipes")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> ListRecipes()
        {
            // Fetch (r)ecipes including their associated meal plan
            List<Recipe> recipes = await _context.Recipes
                .Include(r => r.MealPlan)
                .ToListAsync();

            // Create a list of RecipeDto
            List<RecipeDto> recipeDtos = recipes.Select(r => new RecipeDto()
            {
                RecipeId = r.RecipeId,
                Name = r.Name,
                Cuisine = r.Cuisine,
                MealPlanId = r.MealPlanId,
                MealPlanName = r.MealPlan.Name
            }).ToList();

            // Return 200 OK with RecipeDtos
            return Ok(recipeDtos);
        }

        /// <summary>
        /// Returns a single Recipe specified by its {id}, represented by a RecipeDto with its associated Meal Plan
        /// </summary>
        /// <param name="id">The recipe id</param>
        /// <returns>
        /// 200 OK with a RecipeDto object
        /// or
        /// 404 Not Found if the recipe does not exist
        /// </returns>
        /// <example>
        /// GET: api/Recipes/FindRecipes/1 -> 
        /// {"RecipeId":1,"Name":"Avocado Salad","Cuisine":"Mexican","MealPlanId":1,"MealPlanName":"Weight Loss Plan"}
        /// GET: api/Recipes/FindRecipes/2 -> 
        /// {"RecipeId":2,"Name":"Quinoa Bowl","Cuisine":"American","MealPlanId":1,"MealPlanName":"Weight Loss Plan"}
        /// </example>
        [HttpGet(template: "FindRecipe/{id}")]
        public async Task<ActionResult<RecipeDto>> FindRecipe(int id)
        {
            // Fetch a single recipe by ID, including its associated meal plan
            var recipe = await _context.Recipes
                .Include(r => r.MealPlan)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            // If the recipe could not be found, return 404 Not Found
            if (recipe == null)
            {
                return NotFound();
            }

            // Create an instance of RecipeDto
            RecipeDto recipeDto = new RecipeDto()
            {
                RecipeId = recipe.RecipeId,
                Name = recipe.Name,
                Cuisine = recipe.Cuisine,
                MealPlanId = recipe.MealPlanId,
                MealPlanName = recipe.MealPlan.Name
            };

            // Return 200 OK with the RecipeDto
            return Ok(recipeDto);
        }

        /// <summary>
        /// Updates a Recipe
        /// </summary>
        /// <param name="id">The ID of the Recipe to update</param>
        /// <param name="recipeDto">The updated recipe details (RecipeId, Name, Cuisine, MealPlanId. MealPlanName)</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Recipes/UpdateRecipe/1
        /// Body: { "RecipeId": 1,"Name": "Chickpea Paneer Curry","Cuisine": "Indian","MealPlanId": 2, "MealPlanName": "High Protein Plan" }
        /// </example>
        [HttpPut(template: "UpdateRecipe/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeDto recipeDto)
        {
            // Ensure the provided ID in URL matches the RecipeId in the request body
            if (id != recipeDto.RecipeId)
            {
                return BadRequest("ID in the URL does not match the Recipe ID in the body.");
            }

            // Attempt to find the associated MealPlan in the database
            var mealPlan = await _context.MealPlans.FindAsync(recipeDto.MealPlanId);

            if (mealPlan == null)
            {
                return NotFound();
            }

            // Create an instance of Recipe with updated data
            Recipe recipe = new Recipe()
            {
                RecipeId = recipeDto.RecipeId,
                Name = recipeDto.Name,
                Cuisine = recipeDto.Cuisine,
                MealPlanId = recipeDto.MealPlanId,
                MealPlan = mealPlan
            };

            // Flag the object as modified in the database
            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(r => r.RecipeId == id);
        }

        /// <summary>
        /// Adds a new Recipe to the system
        /// </summary>
        /// <param name="recipeDto">The required information to add the recipe (Name, Cuisine, MealPlanId, )</param>
        /// <returns>
        /// 201 Created
        /// Location: api/OrderItems/AddRecipe
        /// {RecipeDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/Recipes/AddRecipe ->
        /// { "RecipeId": 1,"Name": "Chickpea Paneer","Cuisine": "Indian","MealPlanId": 2, "MealPlanName": "High Protein Plan" }
        /// Body: { "RecipeId": 1,"Name": "Chickpea Paneer","Cuisine": "Indian","MealPlanId": 2, "MealPlanName": "High Protein Plan" }
        /// </example>
        [HttpPost(template: "AddRecipe")]
        public async Task<ActionResult<Recipe>> AddRecipe(RecipeDto recipeDto)
        {
            // Attempt to find associated MealPlan in DB by looking up MealPlanId
            var mealPlan = await _context.MealPlans.FindAsync(recipeDto.MealPlanId);
            if (mealPlan == null)
            {
                // 404 Not Found if the MealPlan does not exist
                return NotFound();
            }

            // Create new Recipe entity using the data from RecipeDto
            Recipe recipe = new Recipe()
            {
                Name = recipeDto.Name,
                Cuisine = recipeDto.Cuisine,
                MealPlanId = recipeDto.MealPlanId,
                MealPlan = mealPlan
            };

            // SQL Equivalent: Insert into recipes (..) values (..)
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            // Return 201 Created with Location header pointing to the newly created recipe
            return CreatedAtAction("FindRecipe", new { id = recipe.RecipeId }, recipeDto);
        
        }

        /// <summary>
        /// Deletes a Recipe by ID
        /// </summary>
        /// <param name="id">The ID of the recipe to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Recipes/DeleteRecipes/7
        /// </example>
        [HttpDelete("DeleteRecipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            // Find the recipe by ID
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a recipe exists in the database
        /// </summary>
        /// <param name="id">The ID of the recipe</param>
        /// <returns>True if recipe exists, otherwise false</returns>

        private bool RecipeExist(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }

    }
}
