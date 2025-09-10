using Inventory_List.Data;
using Inventory_List.Models;
using Inventory_List.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory_List.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: Index
        public IActionResult Index()
        {
            var transactions = _db.Transactions
                .Select(t => new Transaction
                {
                    Id = t.Id,
                    Type = t.Type,
                    Quantity = t.Quantity,
                    Date = t.Date,
                    Product = t.Product,
                    UserId = t.UserId
                })
                .ToList();

            return View(transactions);
        }

        // GET: Create
        public IActionResult Create()
        {
            var vm = new TransactionViewModel
            {
                Products = _db.Products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),
                Types = new List<SelectListItem>
                {
                    new SelectListItem("Purchase", "Purchase"),
                    new SelectListItem("Sale", "Sale"),
                    new SelectListItem("Return", "Return"),
                    new SelectListItem("Adjustment", "Adjustment")
                }
            };

            return View(vm);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    Type = vm.Type,
                    ProductId = vm.ProductId,
                    Quantity = vm.Quantity,
                    Date = DateTime.Now,
                    UserId = vm.UserId

                    //UserId = _userManager.GetUserId(User)
                };

                _db.Transactions.Add(transaction);

                // Adjust stock quantity if needed
                var product = _db.Products.Find(vm.ProductId);
                if (transaction.Type == "Purchase" || transaction.Type == "Return")
                    product.Quantity += vm.Quantity;
                else if (transaction.Type == "Sale" || transaction.Type == "Adjustment")
                    product.Quantity -= vm.Quantity;

                _db.SaveChanges();

                TempData["success"] = "Transaction recorded successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns
            vm.Products = _db.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
            vm.Types = new List<SelectListItem>
            {
                new SelectListItem("Purchase", "Purchase"),
                new SelectListItem("Sale", "Sale"),
                new SelectListItem("Return", "Return"),
                new SelectListItem("Adjustment", "Adjustment")
            };

            return View(vm);
        }
    }
}
