# Recipe Management System

Recipe Management System is an application designed to help users manage recipes, meal plans, and ingredients. It allows the creation, editing, and deletion of recipes while organizing them into meal plans.

## Features
- **Recipe Management**
  - Create, edit, and delete recipes.
- **Meal Planning**
  - Assign recipes to meal plans for better organization.
- **Ingredient Tracking**
  - Manage ingredients associated with recipes.
- **One-to-Many & Many-to-Many Relationships**
  - MealPlan → Recipes (One-to-Many): A MealPlan can have multiple Recipes, but each Recipe belongs to only one MealPlan.
  - Recipes ↔ Ingredients (Many-to-Many): A Recipe can have many Ingredients, and an Ingredient can be used in many Recipes. This is handled through the RecipeIngredients table.
- **RESTful API**
  - Supports Create, Read, Update, and Delete (CRUD) operations.
- **Data Validation**
  - Ensures input integrity with required fields and constraints.

## Database Relationships
### One-to-Many: MealPlan → Recipes
- A **MealPlan** can have multiple **Recipes**, but each **Recipe** belongs to only one **MealPlan**.

### Many-to-Many: Recipes ↔ Ingredients
- A **Recipe** can have many **Ingredients**, and an **Ingredient** can be used in many **Recipes**. This relationship is handled through the **RecipeIngredients** join table.

## Database Integration
- Built with **ASP.NET Core**, **C#**, and **Entity Framework**.
- Database: **Microsoft SQL Server**

## CRUD Functionality
- Full Create, Read, Update, and Delete (CRUD) operations for Recipes, Meal Plans, and Ingredients.
- Fetch a list of recipes by meal plan ID.
- Fetch a list of ingredients by recipe ID.
- Link and unlink recipes with ingredients.

## Technologies Used
- Backend: **ASP.NET Core**, **C#**
- Frontend: **MVC** (To be executed in future)
- Database: **Microsoft SQL Server**, **Entity Framework**
- Authentication: **Identity Framework** (To be executed in future)

## GitHub Repository

To clone the repository:
```sh
git clone https://github.com/dignapatel0/RecipeManagementSystem.git
```
