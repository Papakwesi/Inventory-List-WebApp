using Inventory_List.Data;
using Inventory_List.Models;
using Inventory_List.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "SuperAdmin,Admin,Employee")]
        public IActionResult Index()
        {
            var lowStock = (from p in _db.Products
                            join rl in _db.ReorderLevels
                                on p.Id equals rl.ProductId
                            where p.Quantity <= rl.Level
                            select new LowStockProductVM
                            {
                                Name = p.Name,
                                Quantity = p.Quantity,
                                ReorderLevel = rl.Level
                            }).ToList();

            var vm = new DashboardViewModel
            {
                TotalCategories = _db.Categories.Count(),
                TotalProducts = _db.Products.Count(),
                TotalSuppliers = _db.Suppliers.Count(),
                TotalCustomers = _db.Customers.Count(),
                TotalTransactions = _db.Transactions.Count(),
                ReorderCount = lowStock.Count,
                LowStockProducts = lowStock,

                //CategoryLabels = _db.Categories.Select(c => c.Name).ToList(),
                //CategoryCounts = _db.Categories
                //                .Select(c => c.Name.Count())
                //                .ToList(),

                //TransactionDates = _db.Transactions
                //.GroupBy(t => t.Date.Date) // Group by Date only
                //.OrderBy(g => g.Key)
                //.Select(g => g.Key.ToString("yyyy-MM-dd"))
                //.ToList(),

                //TransactionCounts = _db.Transactions
                //.GroupBy(t => t.Date.Date)
                //.OrderBy(g => g.Key)
                //.Select(g => g.Count())
                //.ToList(),
            };

            return View(vm);
        }
        [Authorize(Roles = "SuperAdmin,Admin,Employee")]
        public IActionResult Charts()
        {
           
            var vm = new DashboardViewModel
            {
                TotalCategories = _db.Categories.Count(),
                TotalProducts = _db.Products.Count(),
                TotalSuppliers = _db.Suppliers.Count(),
                TotalCustomers = _db.Customers.Count(),

                CategoryLabels = _db.Categories.Select(c => c.Name).ToList(),
                CategoryCounts = _db.Categories
                                .Select(c => _db.Products.Count(p => p.CategoryId == c.Id))
                                .ToList(),

                TransactionDates = _db.Transactions
                .GroupBy(t => t.Date.Date) // Group by Date only
                .OrderBy(g => g.Key)
                .Select(g => g.Key.ToString("yyyy-MM-dd"))
                .ToList(),

                TransactionCounts = _db.Transactions
                .GroupBy(t => t.Date.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList(),
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
