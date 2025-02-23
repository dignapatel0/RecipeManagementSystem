using PassionProject.Models;

namespace PassionProject.Interfaces
{
    public interface IRecipeService
    {
        // base CURD
        Task<IEnumerable<RecipeDto>> ListRecipes();

        Task<RecipeDto?> FindRecipe(int id);

        Task<ServiceResponse> UpdateRecipe(RecipeDto recipeDto);

        Task<ServiceResponse> AddRecipe(RecipeDto recipeDto);

        Task<ServiceResponse> DeleteRecipe(int id);

        // related methods
        Task<IEnumerable<RecipeDto>> ListRecipesForMealPlan(int mealPlanId);


    }
}
