using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Example.Models;
using Microsoft.AspNetCore.Authorization;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Allow anonymous access. This is the default
        [AllowAnonymous]
        public IActionResult Anonymous()
        {
            return View();
        }

        // Allow access by all logged in users
        [Authorize]
        public IActionResult Users()
        {
            return View();
        }

        // Allow access to admins only
        [Authorize(Roles="Admin")]
        public IActionResult Admins()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
