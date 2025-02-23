using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Interfaces;

namespace PassionProject.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecipeDto>> ListRecipes()
        {
            // Fetch recipes including their associated meal plan
            List<Recipe> recipes = await _context.Recipes
                .Include(r => r.MealPlan)
                .ToListAsync();

            // Convert to RecipeDto
            List<RecipeDto> recipeDtos = recipes.Select(r => new RecipeDto()
            {
                RecipeId = r.RecipeId,
                Name = r.Name,
                Cuisine = r.Cuisine,
                MealPlanId = r.MealPlanId,
                MealPlanName = r.MealPlan.Name
            }).ToList();

            return recipeDtos;
        }

        public async Task<RecipeDto?> FindRecipe(int id)
        {
            // Fetch a single recipe by ID, including its associated meal plan
            var recipe = await _context.Recipes
                .Include(r => r.MealPlan) // Join with MealPlan
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            // If no recipe is found, return null
            if (recipe == null)
            {
                return null;
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

            return recipeDto;
        }

        public async Task<ServiceResponse> UpdateRecipe(RecipeDto recipeDto)
        {
            ServiceResponse serviceResponse = new();

            // Check if the recipe exists
            var existingRecipe = await _context.Recipes.FindAsync(recipeDto.RecipeId);
            if (existingRecipe == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Recipe not found.");
                return serviceResponse;
            }

            // Check if the MealPlan exists
            var mealPlan = await _context.MealPlans.FindAsync(recipeDto.MealPlanId);
            if (mealPlan == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Associated Meal Plan not found.");
                return serviceResponse;
            }

            // Update Recipe properties
            existingRecipe.Name = recipeDto.Name;
            existingRecipe.Cuisine = recipeDto.Cuisine;
            existingRecipe.MealPlanId = recipeDto.MealPlanId;
            existingRecipe.MealPlan = mealPlan;

            // Mark entity as modified
            _context.Entry(existingRecipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred while updating the recipe.");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> AddRecipe(RecipeDto recipeDto)
        {
            ServiceResponse response = new();

            // Find associated MealPlan
            var mealPlan = await _context.MealPlans.FindAsync(recipeDto.MealPlanId);
            if (mealPlan == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Meal Plan not found.");
                return response;
            }

            // Create a new Recipe entity
            Recipe recipe = new Recipe()
            {
                Name = recipeDto.Name,
                Cuisine = recipeDto.Cuisine,
                MealPlanId = recipeDto.MealPlanId,
                MealPlan = mealPlan
            };

            // Add the new recipe to the database
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            // Return success response
            response.Status = ServiceResponse.ServiceStatus.Created;
            response.CreatedId = recipe.RecipeId;
            return response;
        }

        public async Task<ServiceResponse> DeleteRecipe(int id)
        {
            ServiceResponse response = new();

            // Find the recipe by ID
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Recipe not found. Cannot be deleted.");
                return response;
            }

            try
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("An error occurred while deleting the recipe.");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }

        public async Task<IEnumerable<RecipeDto>> ListRecipesForMealPlan(int mealPlanId)
        {
            // Fetch recipes associated with the specified MealPlanId
            List<Recipe> recipes = await _context.Recipes
                .Where(rm => rm.MealPlanId == mealPlanId)
                .ToListAsync();

            // Create a list of RecipeDto
            List<RecipeDto> recipeDtos = new List<RecipeDto>();

            // Map Recipe to RecipeDto
            foreach (Recipe recipe in recipes)
            {
                recipeDtos.Add(new RecipeDto()
                {
                    RecipeId = recipe.RecipeId,
                    Name = recipe.Name,
                    Cuisine = recipe.Cuisine,
                    MealPlanId = recipe.MealPlanId,
                    MealPlanName = recipe.MealPlan?.Name // Assuming MealPlan has a Name property
                });
            }

            // Return the list of RecipeDto
            return recipeDtos;
        }
    }
}
