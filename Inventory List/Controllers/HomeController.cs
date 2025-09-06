using Inventory_List.Data;
using Inventory_List.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory_List.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                TotalCategories = _db.Categories.Count(),
                TotalProducts = _db.Products.Count(),
                TotalSuppliers = _db.Suppliers.Count(),
                TotalCustomers = _db.Customers.Count(),
                TotalRecords = _db.ReorderLevels.Count()
            };

            return View(vm);
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
