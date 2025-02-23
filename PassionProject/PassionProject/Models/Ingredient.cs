using System.ComponentModel.DataAnnotations;

namespace PassionProject.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public int CaloriesPerUnit { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

        // Many-to-Many relationship with Recipes
        public ICollection<RecipexIngredient> RecipexIngredients { get; set; }

    }
    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int CaloriesPerUnit { get; set; }
    }
}