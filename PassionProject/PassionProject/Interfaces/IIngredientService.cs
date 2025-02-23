using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IIngredientService
    {
        // Base CRUD operations
        Task<IEnumerable<IngredientDto>> ListIngredients();

        Task<IngredientDto?> FindIngredient(int id);

        Task<ServiceResponse> UpdateIngredient(IngredientDto ingredientDto);

        Task<ServiceResponse> AddIngredient(IngredientDto ingredientDto);

        Task<ServiceResponse> DeleteIngredient(int id);

        // Related methods

        /*
        Task<ServiceResponse> LinkIngredientToRecipe(int ingredientId, int recipeId);
        Task<ServiceResponse> UnlinkIngredientFromRecipe(int ingredientId, int recipeId);
        

        Task<IEnumerable<IngredientDto>> GetAllIngredients(int id);
        */
    }
}
