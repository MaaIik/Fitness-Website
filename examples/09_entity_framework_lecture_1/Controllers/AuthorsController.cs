using System.Linq;
using Example.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Example.Models;

namespace Example.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly ApplicationDbContext _db;

        public AuthorsController(ILogger<AuthorsController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            _logger.LogDebug("GET Authors/Index");

            // The view expects a list of authors, so fetch that from the database
            return View(_db.Authors.ToList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Author());
        }

        [HttpPost]
        public IActionResult Add(Author author)
        {
            _logger.LogDebug("POST Authors/Index");

            // You can check if the model requirements have been fulfilled using ModelState
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Received invalid model");

                // If the state is invalid just display the view again, with the partially filled model
                return View(author);
            }

            // Model is valid, add it to the database
            _db.Authors.Add(author);
            _db.SaveChanges();

            // Redirect back to the Index view which will show the current list of authors
            return RedirectToAction(nameof(Index));
        }
    }
}