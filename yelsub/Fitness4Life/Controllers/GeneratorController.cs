using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitness4Life.Data;
using Fitness4Life.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Fitness4Life.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly ILogger<GeneratorController> _logger;
        private readonly ApplicationDbContext _db;


        public GeneratorController(ILogger<GeneratorController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        

        [Authorize]
       
        // GET: Authors
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View("_GeneratePartial",await _db.WorkoutProgrammes.ToListAsync());
        }

        // GET: Authors/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _db.WorkoutProgrammes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View("_GeneratePartial");
        }
        
        public IActionResult Generator()
        {
            return View();
        }
        
        public ActionResult Generate(WorkoutProgramme workoutProgramme)
        {
            return PartialView("_GeneratePartial");
        }
        
        public IActionResult WorkoutProgramme()
        {
            return View();
        }
    }
}