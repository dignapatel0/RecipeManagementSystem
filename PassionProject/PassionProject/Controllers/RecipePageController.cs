using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassionProject.Interfaces;
using PassionProject.Models;
using PassionProject.Services;

namespace PassionProject.Controllers
{
    public class RecipePageController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IMealPlanService _mealPlanService; // Add the meal plan service

        public RecipePageController(IRecipeService recipeService, IMealPlanService mealPlanService)
        {
            _recipeService = recipeService;
            _mealPlanService = mealPlanService; // Assign the meal plan service
        }

        // Redirect to List action
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: RecipePage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<RecipeDto?> recipeDtos = await _recipeService.ListRecipes();
            return View(recipeDtos);
        }

        // GET: RecipePage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            RecipeDto? recipeDto = await _recipeService.FindRecipe(id);

            if (recipeDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find recipe"] });
            }

            return View(recipeDto);
        }

        // GET: RecipePage/New
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> New()
        {
            var mealPlans = await _mealPlanService.ListMealPlans(); // Fetch all meal plans
            var model = new RecipeDto
            {
                MealPlans = mealPlans // Populate the meal plans in the model
            };
            return View(model);
        }

        // POST: RecipePage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(RecipeDto recipeDto)
        {
            // Call your service to add the recipe
            ServiceResponse response = await _recipeService.AddRecipe(recipeDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                // Redirect to the List action to view the updated list
                return RedirectToAction("List");
            }
            else
            {
                // If there's an error, return the error view with messages
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }


        // GET: RecipePage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            RecipeDto? recipeDto = await _recipeService.FindRecipe(id);
            if (recipeDto == null)
            {
                return View("Error"); // Handle not found
            }

            // Fetch the available meal plans
            var mealPlans = await _mealPlanService.ListMealPlans(); // Assuming this service exists
            recipeDto.MealPlans = mealPlans; // Populate MealPlans in the DTO

            return View(recipeDto);
        }


        // POST: RecipePage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, RecipeDto recipeDto)
        {
            if (!ModelState.IsValid)
            {
                // Fetch the available meal plans
                var mealPlans = await _mealPlanService.ListMealPlans(); // Assuming this service exists
                recipeDto.MealPlans = mealPlans; // Populate MealPlans in the DTO
                return View("Edit", recipeDto); 
            }

            ServiceResponse response = await _recipeService.UpdateRecipe(recipeDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "RecipePage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET RecipePage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            RecipeDto? recipeDto = await _recipeService.FindRecipe(id);
            if (recipeDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(recipeDto);
            }
        }

        // POST RecipePage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _recipeService.DeleteRecipe(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "RecipePage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }



    }
}
