using System.Linq;
using Example.Data;
using Example.Models;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RecentAdditions()
        {
            // Create and fill the view model with data
            var vm = new RecentAdditionsViewModel
            {
                Authors = _db.Authors.OrderByDescending(a => a.Id).Take(3).ToList(),
                Books = _db.Books.OrderByDescending(a => a.Id).Take(3).ToList()
            };

            // Send the view model to the view
            return View(vm);
        }
    }
}