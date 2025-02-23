using Microsoft.EntityFrameworkCore;
using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Interfaces;

namespace PassionProject.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IngredientDto>> ListIngredients()
        {
            // Fetch ingredients
            List<Ingredient> ingredients = await _context.Ingredients.ToListAsync();

            // Convert to IngredientDto
            List<IngredientDto> ingredientDtos = ingredients.Select(i => new IngredientDto()
            {
                IngredientId = i.IngredientId,
                Name = i.Name,
                Unit = i.Unit,
                CaloriesPerUnit = i.CaloriesPerUnit
            }).ToList();

            return ingredientDtos;
        }

        public async Task<IngredientDto?> FindIngredient(int id)
        {
            // Fetch a single ingredient by ID
            var ingredient = await _context.Ingredients.FindAsync(id);

            // If no ingredient is found, return null
            if (ingredient == null)
            {
                return null;
            }

            // Create an instance of IngredientDto
            IngredientDto ingredientDto = new IngredientDto()
            {
                IngredientId = ingredient.IngredientId,
                Name = ingredient.Name,
                Unit = ingredient.Unit,
                CaloriesPerUnit = ingredient.CaloriesPerUnit
            };

            return ingredientDto;
        }

        public async Task<ServiceResponse> UpdateIngredient(IngredientDto ingredientDto)
        {
            ServiceResponse serviceResponse = new();

            // Check if the ingredient exists
            var existingIngredient = await _context.Ingredients.FindAsync(ingredientDto.IngredientId);
            if (existingIngredient == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Ingredient not found.");
                return serviceResponse;
            }

            // Update Ingredient properties
            existingIngredient.Name = ingredientDto.Name;
            existingIngredient.Unit = ingredientDto.Unit;
            existingIngredient.CaloriesPerUnit = ingredientDto.CaloriesPerUnit;

            // Mark entity as modified
            _context.Entry(existingIngredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred while updating the ingredient.");
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> AddIngredient(IngredientDto ingredientDto)
        {
            ServiceResponse response = new();

            // Create a new Ingredient entity
            Ingredient ingredient = new Ingredient()
            {
                Name = ingredientDto.Name,
                Unit = ingredientDto.Unit,
                CaloriesPerUnit = ingredientDto.CaloriesPerUnit
            };

            // Add the new ingredient to the database
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            // Return success response
            response.Status = ServiceResponse.ServiceStatus.Created;
            response.CreatedId = ingredient.IngredientId;
            return response;
        }

        public async Task<ServiceResponse> DeleteIngredient(int id)
        {
            ServiceResponse response = new();

            // Find the ingredient by ID
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Ingredient not found. Cannot be deleted.");
                return response;
            }

            try
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("An error occurred while deleting the ingredient.");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }

        /*
        public async Task<ServiceResponse> LinkIngredientToRecipe(int ingredientId, int recipeId)
        {
            var serviceResponse = new ServiceResponse();

            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);

            if (recipe == null || ingredient == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (recipe == null)
                {
                    serviceResponse.Messages.Add("Recipe not found.");
                }
                if (ingredient == null)
                {
                    serviceResponse.Messages.Add("Ingredient not found.");
                }
                return serviceResponse;
            }

            try
            {
                recipe.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            }
            catch (Exception ex)
            {
                serviceResponse.Messages.Add("There was an issue linking the ingredient to the recipe.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse> UnlinkIngredientFromRecipe(int ingredientId, int recipeId)
        {
            var serviceResponse = new ServiceResponse();

            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);

            if (recipe == null || ingredient == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (recipe == null)
                {
                    serviceResponse.Messages.Add("Recipe not found.");
                }
                if (ingredient == null)
                {
                    serviceResponse.Messages.Add("Ingredient not found.");
                }
                return serviceResponse;
            }

            try
            {
                recipe.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                serviceResponse.Messages.Add("There was an issue unlinking the ingredient from the recipe.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }

        // Method to get all ingredients for dropdown
        public async Task<IEnumerable<IngredientDto>> GetAllIngredients(int id)
        {
            // Fetching ingredients from the database
            var ingredients = await _context.Ingredients
                .Select(ingredient => new IngredientDto
                {
                    IngredientId = ingredient.IngredientId,
                    Name = ingredient.Name // Assuming you have a Name property
                })
                .ToListAsync();

            return ingredients; // Return the list of IngredientDto
        }

        */


        /*
        public async Task<IEnumerable<IngredientDto>> ListIngredientsForRecipe(int recipeId)
        {
            // Fetch ingredients associated with the specified RecipeId
            List<Ingredient> ingredients = await _context.RecipexIngredients
                .Where(ri => ri.RecipeId == recipeId)
                .Select(ri => ri.Ingredient)
                .ToListAsync();

            // Create a list of IngredientDto
            List<IngredientDto> ingredientDtos = ingredients.Select(i => new IngredientDto()
            {
                IngredientId = i.IngredientId,
                Name = i.Name,
                Unit = i.Unit,
                CaloriesPerUnit = i.CaloriesPerUnit
            }).ToList();

            return ingredientDtos;
        }*/
    }
}
