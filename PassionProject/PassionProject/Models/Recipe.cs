using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        public string Name { get; set; }

        public string Cuisine { get; set; }

        // One Recipe belongs to one MealPlan
        [ForeignKey("MealPlans")]
        public int MealPlanId { get; set; } 

        public virtual MealPlan MealPlan { get; set; }

        // Many-to-Many relationship with Ingredients
        
        public ICollection<RecipexIngredient> RecipexIngredients { get; set; }

    }

    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Cuisine { get; set; }
        public int MealPlanId { get; set; }
        public string MealPlanName { get; set; }
    }
}
