using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipexIngredientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipexIngredient_Ingredients_IngredientId",
                table: "RecipexIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipexIngredient_Recipes_RecipeId",
                table: "RecipexIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipexIngredient",
                table: "RecipexIngredient");

            migrationBuilder.RenameTable(
                name: "RecipexIngredient",
                newName: "RecipexIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_RecipexIngredient_RecipeId",
                table: "RecipexIngredients",
                newName: "IX_RecipexIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipexIngredient_IngredientId",
                table: "RecipexIngredients",
                newName: "IX_RecipexIngredients_IngredientId");

            migrationBuilder.AlterColumn<int>(
                name: "CaloriesPerUnit",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipexIngredients",
                table: "RecipexIngredients",
                column: "RecipexIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipexIngredients_Ingredients_IngredientId",
                table: "RecipexIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipexIngredients_Recipes_RecipeId",
                table: "RecipexIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipexIngredients_Ingredients_IngredientId",
                table: "RecipexIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipexIngredients_Recipes_RecipeId",
                table: "RecipexIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipexIngredients",
                table: "RecipexIngredients");

            migrationBuilder.RenameTable(
                name: "RecipexIngredients",
                newName: "RecipexIngredient");

            migrationBuilder.RenameIndex(
                name: "IX_RecipexIngredients_RecipeId",
                table: "RecipexIngredient",
                newName: "IX_RecipexIngredient_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipexIngredients_IngredientId",
                table: "RecipexIngredient",
                newName: "IX_RecipexIngredient_IngredientId");

            migrationBuilder.AlterColumn<int>(
                name: "CaloriesPerUnit",
                table: "Ingredients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipexIngredient",
                table: "RecipexIngredient",
                column: "RecipexIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipexIngredient_Ingredients_IngredientId",
                table: "RecipexIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipexIngredient_Recipes_RecipeId",
                table: "RecipexIngredient",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
