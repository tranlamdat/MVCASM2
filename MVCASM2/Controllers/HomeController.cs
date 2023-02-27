using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCASM2.Data;
using MVCASM2.Models;
using System.Diagnostics;

namespace MVCASM2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
			IEnumerable<Product> lstPro = _context.Products.ToList();
			return View(lstPro);
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