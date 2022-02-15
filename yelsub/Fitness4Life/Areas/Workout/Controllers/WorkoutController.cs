using Microsoft.AspNetCore.Mvc;


namespace Fitness4Life.Areas.Workout.Controllers
{
    [Area("Workout")]
    public class WorkoutController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        // HomeWorkouts
        public IActionResult HomeWorkouts()
        {
            return View();
        }
        
        // GymWorkouts
        public IActionResult GymWorkouts()
        {
            return View();
        }
    }
}