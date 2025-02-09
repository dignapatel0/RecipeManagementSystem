using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;
using static PassionProject.Models.Ingredient;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all ingredients.
        /// </summary>
        /// <returns>A list of ingredient DTO objects.</returns>
        /// <example>
        /// GET: api/Ingredients/ListIngredients ->
        /// [
        ///  {"IngredientId":1, "Name":"Chicken Breast", "Unit":"grams", "CaloriesPerUnit":165},
        ///  {"IngredientId":2, "Name":"Avocado", "Unit":"grams", "CaloriesPerUnit":160}
        /// ]
        /// </example>
        [HttpGet("ListIngredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> ListIngredients()
        {
            List<Ingredient> ingredients = await _context.Ingredients.ToListAsync();

            List<IngredientDto> ingredientDtos = ingredients.Select(i => new IngredientDto
            {
                IngredientId = i.IngredientId,
                Name = i.Name,
                Unit = i.Unit,
                CaloriesPerUnit = i.CaloriesPerUnit
            }).ToList();

            return Ok(ingredientDtos);
        }

        /// <summary>
        /// Returns a single ingredient by ID.
        /// </summary>
        /// <param name="id">The ingredient ID.</param>
        /// <returns>Ingredient DTO or 404 Not Found.</returns>
        /// <example>
        /// GET: api/Ingredients/FindIngredient/1 ->
        /// {"IngredientId":1, "Name":"Chicken Breast", "Unit":"grams", "CaloriesPerUnit":165}
        /// </example>
        [HttpGet("FindIngredient/{id}")]
        public async Task<ActionResult<IngredientDto>> FindIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            IngredientDto ingredientDto = new IngredientDto
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                CaloriesPerUnit = ingredient.CaloriesPerUnit
            };

            return Ok(ingredientDto);
        }

        /// <summary>
        /// Updates an existing ingredient.
        /// </summary>
        /// <param name="id">The ID of the ingredient to update.</param>
        /// <param name="ingredientDto">Updated ingredient details.</param>
        /// <returns>204 No Content or 404 Not Found.</returns>
        /// <example>
        /// PUT: api/Ingredients/UpdateIngredient/1
        /// Body: { "IngredientId": 1, "Name": "Chicken Thigh", "Unit": "grams", "CaloriesPerUnit": 175 }
        /// </example>
        [HttpPut("UpdateIngredient/{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, IngredientDto ingredientDto)
        {
            if (id != ingredientDto.IngredientId)
            {
                return BadRequest("ID in the URL does not match the Ingredient ID in the body.");
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            // Update fields
            ingredient.Name = ingredientDto.Name;
            ingredient.Unit = ingredientDto.Unit;
            ingredient.CaloriesPerUnit = ingredientDto.CaloriesPerUnit;

            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        private bool IngredientExists(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new ingredient to the system.
        /// </summary>
        /// <param name="ingredientDto">Ingredient details.</param>
        /// <returns>201 Created with Ingredient details.</returns>
        /// <example>
        /// POST: api/Ingredients/AddIngredient ->
        /// { "IngredientId": 9, "Name": "Broccoli", "Unit": "grams", "CaloriesPerUnit": 55 }
        /// </example>
        [HttpPost("AddIngredient")]
        public async Task<ActionResult<Ingredient>> AddIngredient(IngredientDto ingredientDto)
        {
            Ingredient ingredient = new Ingredient
            {
                Name = ingredientDto.Name,
                Unit = ingredientDto.Unit,
                CaloriesPerUnit = ingredientDto.CaloriesPerUnit
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("FindIngredient", new { id = ingredient.IngredientId }, ingredientDto);
        }

        /// <summary>
        /// Deletes an ingredient by ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete.</param>
        /// <returns>204 No Content or 404 Not Found.</returns>
        /// <example>
        /// DELETE: api/Ingredients/DeleteIngredient/1
        /// </example>
        [HttpDelete("DeleteIngredient/{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if an ingredient exists in the database.
        /// </summary>
        /// <param name="id">The ID of the ingredient.</param>
        /// <returns>True if exists, otherwise false.</returns>
        private bool IngredientExist(int id)
        {
            return _context.Ingredients.Any(e => e.IngredientId == id);
        }

        /// <summary>
        /// Returns a list of ingredients associated with a specific recipe using its {id}.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe.</param>
        /// <returns>
        /// 200 OK: A list of ingredients in the format [{IngredientDto}, {IngredientDto}, ...]
        /// 404 Not Found: If no ingredients are found for the given recipe ID.
        /// </returns>
        /// <example>
        /// GET: api/Ingredients/ListIngredientsForRecipe/5 -> [{IngredientDto}, {IngredientDto}, ...]
        /// </example>
        [HttpGet("ListIngredientsForRecipe/{recipeId}")]
        public async Task<IActionResult> ListIngredientsForRecipe(int recipeId)
        {
            // Get ingredients related to the specified recipe ID
            var ingredients = await _context.RecipexIngredients
                .Where(ri => ri.RecipeId == recipeId) // Filter by RecipeId
                .Select(ri => new IngredientDto
                {
                    IngredientId = ri.Ingredient.IngredientId,
                    Name = ri.Ingredient.Name,
                    Unit = ri.Unit, // Quantity unit stored in RecipexIngredient
                    CaloriesPerUnit = ri.Ingredient.CaloriesPerUnit
                })
                .ToListAsync();

            // Check if any ingredients were found
            if (!ingredients.Any())
            {
                return NotFound($"No ingredients found for recipe with ID {recipeId}.");
            }

            // Return 200 OK with list of ingredients
            return Ok(ingredients);
        }

        /// <summary>
        /// Links a recipe to an ingredient (Adds an ingredient to a recipe)
        /// </summary>
        /// <param name="recipeId">The ID of the recipe</param>
        /// <param name="ingredientId">The ID of the ingredient</param>
        /// <param name="quantity">The quantity of the ingredient</param>
        /// <param name="unit">The unit of measurement for the ingredient (e.g., grams, cups)</param>
        /// <returns>204 No Content or 404 Not Found</returns>
        /// <example>
        /// POST: api/RecipexIngredient/Link?recipeId=1&ingredientId=3&quantity=200&unit=grams
        /// Response Code: 204 No Content
        /// </example>

        [HttpPost("Link")]
        public async Task<ActionResult> Link(int recipeId, int ingredientId, decimal quantity, string unit)
        {
            // Check if Recipe Exists
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            // Check if Ingredient Exists
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
            if (ingredient == null)
            {
                return NotFound("Ingredient not found.");
            }

            // Check if Recipe is Already Linked to the Ingredient
            bool alreadyLinked = await _context.RecipexIngredients
                .AnyAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId);

            if (alreadyLinked)
            {
                return BadRequest("This ingredient is already linked to the recipe.");
            }

            // Create and Save the New RecipexIngredient Record
            var recipexIngredient = new RecipexIngredient
            {
                RecipeId = recipeId,
                IngredientId = ingredientId,
                Quantity = quantity,
                Unit = unit
            };

            _context.RecipexIngredients.Add(recipexIngredient);
            await _context.SaveChangesAsync();

            return NoContent();  // 204 No Content (Successful, but no response body)
        }

        /// <summary>
        /// Unlinks a recipe from an ingredient (Removes an ingredient from a recipe)
        /// </summary>
        /// <param name="recipeId">The ID of the recipe</param>
        /// <param name="ingredientId">The ID of the ingredient</param>
        /// <returns>204 No Content or 404 Not Found</returns>
        /// <example>
        /// DELETE: api/RecipexIngredient/Unlink?recipeId=1&ingredientId=3
        /// Response Code: 204 No Content
        /// </example>

        [HttpDelete("Unlink")]
        public async Task<ActionResult> Unlink(int recipeId, int ingredientId)
        {
            // Find the RecipexIngredient record
            var recipexIngredient = await _context.RecipexIngredients
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId);

            if (recipexIngredient == null)
            {
                return NotFound("Ingredient is not linked to this recipe.");
            }

            // Remove the Ingredient from the Recipe
            _context.RecipexIngredients.Remove(recipexIngredient);
            await _context.SaveChangesAsync();

            return NoContent();  // 204 No Content (Successful, but no response body)
        }


    }
}
