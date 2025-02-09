using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class mealplanrecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealPlanId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_MealPlanId",
                table: "Recipes",
                column: "MealPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_MealPlans_MealPlanId",
                table: "Recipes",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "MealPlanId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_MealPlans_MealPlanId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_MealPlanId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "MealPlanId",
                table: "Recipes");
        }
    }
}
