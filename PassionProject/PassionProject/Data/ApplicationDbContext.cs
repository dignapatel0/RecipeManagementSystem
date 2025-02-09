using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;

namespace PassionProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //create a Recipes table from the model
        public DbSet<Recipe> Recipes { get; set; }

        //create a MealPlans table from the model
        public DbSet<MealPlan> MealPlans { get; set; }

        //create a Ingredients table from the model
        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipexIngredient> RecipexIngredients { get; set; }
    }
}
