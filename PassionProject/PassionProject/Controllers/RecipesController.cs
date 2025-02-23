using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassionProject.Interfaces;
using PassionProject.Models;


namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        // dependency injection of service interfaces
        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
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
            // empty list of data transfer object RecipeDto
            IEnumerable<RecipeDto> recipeDtos = await _recipeService.ListRecipes();
            // return 200 OK with RecipeDto
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
            var recipeDto = await _recipeService.FindRecipe(id);

            // if the recipe could not be located, return 404 Not Found
            if (recipeDto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(recipeDto);
            }

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
        [Authorize]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeDto recipeDto)
        {
            // Ensure ID in the URL matches the ID in the request body
            if (id != recipeDto.RecipeId)
            {
                return BadRequest("ID in the URL does not match the Recipe ID in the body.");
            }

            ServiceResponse response = await _recipeService.UpdateRecipe(recipeDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
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
        [Authorize]
        public async Task<ActionResult<Recipe>> AddRecipe(RecipeDto recipeDto)
        {
            ServiceResponse response = await _recipeService.AddRecipe(recipeDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            // Returns 201 Created with Location header
            return Created($"api/Recipes/FindRecipe/{response.CreatedId}", recipeDto);
        
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
        [HttpDelete(template: "DeleteRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            ServiceResponse response = await _recipeService.DeleteRecipe(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
        }


        /// <summary>
        /// Returns a list of recipes associated with a specific MealPlan using its {mealPlanId}.
        /// </summary>
        /// <param name="mealPlanId">The ID of the MealPlan.</param>
        /// <returns>
        /// 200 OK: A list of recipes in the format [{RecipeDto}, {RecipeDto}, ...]
        /// 404 Not Found: If no recipes are found for the given MealPlan ID.
        /// </returns>
        /// <example>
        /// GET: api/Recipes/ListRecipesForMealPlan/3 -> [{RecipeDto}, {RecipeDto}, ...]
        /// </example>
        [HttpGet(template: "ListRecipesForMealPlan/{mealPlanId}")]
        public async Task<IActionResult> ListRecipesForMealPlan(int mealPlanId)
        {
            // Call the service to get the list of RecipeDtos
            IEnumerable<RecipeDto> recipeDtos = await _recipeService.ListRecipesForMealPlan(mealPlanId);

            // Return 200 OK with the list of RecipeDtos
            return Ok(recipeDtos);
        }


    }
}
