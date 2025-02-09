using System.ComponentModel.DataAnnotations;

namespace PassionProject.Models
{
    public class MealPlan
    {
        [Key]
        public int MealPlanId { get; set; }

        public string Name { get; set; } 

        public DateTime Date { get; set; }

        // One MealPlan can have many Recipes
        public ICollection<Recipe> Recipes { get; set; }
    }

    public class MealPlanDto
    {
        public int MealPlanId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

}
