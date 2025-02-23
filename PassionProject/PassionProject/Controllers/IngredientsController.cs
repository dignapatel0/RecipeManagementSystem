using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;
using static PassionProject.Models.Ingredient;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
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
        [HttpGet(template: "ListIngredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> ListIngredients()
        {
            // empty list of data transfer object IngredientDto
            IEnumerable<IngredientDto> ingredientDtos = await _ingredientService.ListIngredients();
            // return 200 OK with IngredientDto
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
        [HttpGet(template: "FindIngredient/{id}")]
        public async Task<ActionResult<IngredientDto>> FindIngredient(int id)
        {
            var ingredient = await _ingredientService.FindIngredient(id);

            // if the ingredient could not be located, return 404 Not Found
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
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
        [HttpPut(template: "UpdateIngredient/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateIngredient(int id, IngredientDto ingredientDto)
        {
            // Ensure the ID in the URL matches the ID in the request body
            if (id != ingredientDto.IngredientId)
            {
                return BadRequest("ID in the URL does not match the Ingredient ID in the body.");
            }

            // Call the service to update the ingredient
            ServiceResponse response = await _ingredientService.UpdateIngredient(ingredientDto);

            // Check the status of the response to determine the appropriate action
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages); // Return 404 if the ingredient was not found
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages); // Return 500 if there was an error
            }

            return NoContent(); // Return 204 No Content on successful update
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
        [HttpPost(template: "AddIngredient")]
        [Authorize]
        public async Task<ActionResult<IngredientDto>> AddIngredient(IngredientDto ingredientDto)
        {
            // Call the service to add the ingredient
            ServiceResponse response = await _ingredientService.AddIngredient(ingredientDto);

            // Check the status of the response to determine the appropriate action
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages); // Return 404 if the associated data was not found
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages); // Return 500 if there was an error
            }

            // Return 201 Created with the location of the new ingredient
            return Created($"api/Ingredients/FindIngredient/{response.CreatedId}", ingredientDto);
        }


        /// <summary>
        /// Deletes an ingredient by ID.
        /// </summary>
        /// <param name="id">The ID of the ingredient to delete.</param>
        /// <returns>204 No Content or 404 Not Found.</returns>
        /// <example>
        /// DELETE: api/Ingredients/DeleteIngredient/1
        /// </example>
        [HttpDelete(template: "DeleteIngredient/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            // Call the service to delete the ingredient by ID
            ServiceResponse response = await _ingredientService.DeleteIngredient(id);

            // Check the status of the response to determine the appropriate action
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(); // Return 404 if the ingredient was not found
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages); // Return 500 if there was an error
            }

            return NoContent(); // Return 204 No Content if the deletion was successful
        }

        /*
        // POST IngredientPage/LinkToRecipe
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LinkToRecipe([FromForm] int recipeId, [FromForm] int ingredientId)
        {
            await _ingredientService.LinkIngredientToRecipe(ingredientId, recipeId);
            return RedirectToAction("Details", new { id = recipeId });
        }

        // POST IngredientPage/UnlinkFromRecipe
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnlinkFromRecipe([FromForm] int recipeId, [FromForm] int ingredientId)
        {
            await _ingredientService.UnlinkIngredientFromRecipe(ingredientId, recipeId);
            return RedirectToAction("Details", new { id = recipeId });
        }

        // Method to get all ingredients for dropdown
        public async Task<IActionResult> GetAllIngredients(int id)
        {
            // empty list of data transfer object IngredientDto
            IEnumerable<IngredientDto> IngredientDto = await _ingredientService.GetAllIngredients(id);
            // return 200 OK with IngredientDto
            return Ok(IngredientDto);
        }
        */
        /*
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
        [HttpGet(template: "ListIngredientsForRecipe/{recipeId}")]

        public async Task<IActionResult> ListIngredientsForRecipe(int recipeId)
        {
            var ingredients = await _ingredientService.ListIngredientsForRecipe(recipeId);
            if (!ingredients.Any())
            {
                return NotFound($"No ingredients found for recipe with ID {recipeId}.");
            }
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
        */

    }
}
