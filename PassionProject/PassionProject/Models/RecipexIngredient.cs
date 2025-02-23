
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class RecipexIngredient
    {
        [Key]
        public int RecipexIngredientId { get; set; } // Primary Key for the junction table

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; } // Foreign Key to Recipe
        public virtual Recipe Recipe { get; set; }

        [ForeignKey("IngredientId")]
        public int IngredientId { get; set; } // Foreign Key to Ingredient
        public virtual Ingredient Ingredient { get; set; }

        public decimal Quantity { get; set; } // Quantity of the ingredient used in the recipe

        public string Unit { get; set; } // Measurement unit (e.g., grams, cups)
        
    }
}
