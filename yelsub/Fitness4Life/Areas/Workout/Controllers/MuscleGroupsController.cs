using Microsoft.AspNetCore.Mvc;

namespace Fitness4Life.Areas.Workout.Controllers
{
    [Area("Workout")]
    public class MuscleGroupsController : Controller
    {
        // Index / Frontpage of sub-page "Muscle Groups"
        public IActionResult Index()
        {
            return View();
        }
        
        // Traps sub-page
        public IActionResult Lats()
        {
            return View();
        }
        
        // Back sub-page
        public IActionResult Back()
        {
            return View();
        }
        
        // Shoulders sub-page
        public IActionResult Shoulders()
        {
            return View();
        }
        
        // Chest sub-page
        public IActionResult Chest()
        {
            return View();
        }
         
        // Biceps sub-page
        public IActionResult Biceps()
        {
            return View();
        }
         
        // Triceps sub-page
        public IActionResult Triceps()
        {
            return View();
        }
         
        // Forearms sub-page
        public IActionResult Forearms()
        {
            return View();
        }
         
        // Abs sub-page
        public IActionResult Abs()
        {
            return View();
        }
         
        // Glutes sub-page
        public IActionResult Glutes()
        {
            return View();
        }
         
        // Hamstrings sub-page
        public IActionResult Hamstrings()
        {
            return View();
        }
         
        // Calves sub-page
        public IActionResult Calves()
        {
            return View();
        }
         
        // Quads sub-page
        public IActionResult Quads()
        {
            return View();
        }
    }
}