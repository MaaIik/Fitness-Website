using Microsoft.AspNetCore.Mvc;

namespace Fitness4Life.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    public class NutritionController : Controller
    {
        
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        // GainWeight sub-page
        public IActionResult BuildMuscle()
        {
            return View();
        }
        
        // LoseWeight sub-page
        public IActionResult LoseFat()
        {
            return View();
        }
    }
}