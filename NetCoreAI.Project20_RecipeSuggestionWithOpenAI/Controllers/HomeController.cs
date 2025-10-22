using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project20_RecipeSuggestionWithOpenAI.Models;

namespace NetCoreAI.Project20_RecipeSuggestionWithOpenAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly OpenAiService _openAiService;
        public HomeController(OpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        [HttpGet]
        public IActionResult CreateRecipe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(string ingredients)
        {
            var result = await _openAiService.GetRecipeAsync(ingredients);
            ViewBag.recipe = result;
            return View();
        }
    }
}