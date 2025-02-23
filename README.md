# Recipe Management System

The Recipe Management System is an application designed to help users manage recipes, meal plans, and ingredients. It allows the creation, editing, and deletion of recipes while organizing them into meal plans.

## Features

- **Recipe Management**
  - Create, edit, and delete recipes.
- **Meal Planning**
  - Assign recipes to meal plans for better organization.
- **Ingredient Tracking**
  - Manage ingredients associated with recipes.
- **One-to-Many & Many-to-Many Relationships**
  - **MealPlan → Recipes (One-to-Many):** A MealPlan can have multiple Recipes, but each Recipe belongs to only one MealPlan.
  - **Recipes ↔ Ingredients (Many-to-Many):** A Recipe can have many Ingredients, and an Ingredient can be used in many Recipes. This is handled through the RecipeIngredients table.
- **RESTful API**
  - Supports Create, Read, Update, and Delete (CRUD) operations.
  - **Link Recipe to Ingredient:** Links a specific ingredient to a recipe, associating the ingredient with the recipe in the `RecipeIngredients` table.
  - **Unlink Recipe from Ingredient:** Unlinks a specific ingredient from a recipe, removing the association between them.
  - **List Ingredients for Recipe:** Retrieves a list of all ingredients associated with a specific recipe.
  - **List Recipes for Meal Plan:** Fetches all recipes associated with a specific meal plan using `ListRecipeForMealPlan`.

- **Data Validation**
  - Ensures input integrity with required fields and constraints.
- **Authorization**
  - Implement user authentication and authorization using ASP.NET Core Identity.
  - Users can register, log in, and manage their accounts, ensuring secure access to the application features.

## Database Relationships

### One-to-Many: MealPlan → Recipes
- A **MealPlan** can have multiple **Recipes**, but each **Recipe** belongs to only one **MealPlan**.

### Many-to-Many: Recipes ↔ Ingredients
- A **Recipe** can have many **Ingredients**, and an **Ingredient** can be used in many **Recipes**. This relationship is handled through the **RecipeIngredients** join table.

## Database Integration
- Built with **ASP.NET Core**, **C#**, and **Entity Framework**.
- Database: **Microsoft SQL Server**

## Service Interfaces
- **RecipeService:** Provides methods for CRUD operations on recipes, including linking and unlinking ingredients.
- **MealPlanService:** Manages meal plans and their associated recipes.
- **IngredientService:** Handles the management of ingredients and their associations with recipes.

## MVC Architecture
- **Models:** Defines the structure of data entities (e.g., Recipe, MealPlan, Ingredient).
- **Views:** Razor views for user interactions (e.g., Create, Edit, Delete pages for recipes, meal plans, and ingredients).
- **Controllers:** Handles the user requests and responses, connecting the views with the service interfaces.

## CRUD Functionality
- Full Create, Read, Update, and Delete (CRUD) operations for Recipes, Meal Plans, and Ingredients.
- Fetch a list of recipes by meal plan ID.
- Fetch a list of ingredients by recipe ID.
- Link and unlink recipes with ingredients.

## Technologies Used
- **Backend:** ASP.NET Core, C#
- **Frontend:** MVC (Model-View-Controller)
- **Database:** Microsoft SQL Server, Entity Framework
- **Authentication:** Identity Framework

## GitHub Repository

To clone the repository:
```sh
git clone https://github.com/dignapatel0/RecipeManagementSystem.git
```
## To Run This Project

1. **Open Package Manager Console:**
   - Navigate to **Tools** > **NuGet Package Manager** > **Package Manager Console**.

2. **Run Database Migrations:**
   - In the console, type:
     ```
     update-database
     ```

3. **Add Data to Database:**
   - Go to **Tools** > **SQL Server Object Explorer** > **Database**.
   - Add records to the following tables:
     - **MealPlans**
     - **Recipes**
     - **Ingredients**
     - **RecipeXIngredients** (junction table for many-to-many relationship between Recipes and Ingredients)

4. **Interact with API:**
   - Use API requests to interact with the **MealPlans**, **Recipes**, **Ingredients**, and **RecipeIngredients** tables.

