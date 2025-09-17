using Inventory_List.Data;
using Inventory_List.Models;
using Inventory_List.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Inventory_List.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Employee")]
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
                .Include(t => t.User)      // Load the User navigation property
                .Include(t => t.Product)   // Load Product if you need it in the view
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
                    UserId = _userManager.GetUserId(User)
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

        public IActionResult Edit(int id)
        {
            var transaction = _db.Transactions.FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var vm = new TransactionViewModel
            {
                Id = transaction.Id,
                Type = transaction.Type,
                ProductId = transaction.ProductId,
                Quantity = transaction.Quantity,
                Date = transaction.Date,
                UserId = transaction.UserId,

                Products = _db.Products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = (p.Id == transaction.ProductId)
                }).ToList(),

                Types = new List<SelectListItem>
        {
            new SelectListItem("Purchase", "Purchase", transaction.Type == "Purchase"),
            new SelectListItem("Sale", "Sale", transaction.Type == "Sale"),
            new SelectListItem("Return", "Return", transaction.Type == "Return"),
            new SelectListItem("Adjustment", "Adjustment", transaction.Type == "Adjustment")
        }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TransactionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Fetch existing transaction from DB
                var transaction = await _db.Transactions.FindAsync(vm.Id);
                if (transaction == null)
                {
                    return NotFound();
                }

                // Update only the editable fields
                transaction.Type = vm.Type;
                transaction.ProductId = vm.ProductId;
                transaction.Quantity = vm.Quantity;
                transaction.Date = vm.Date; // Use vm.Date so you don’t overwrite with Now
                transaction.UserId = vm.UserId;
                // Or transaction.UserId = _userManager.GetUserId(User); if using Identity

                // Adjust stock
                var product = await _db.Products.FindAsync(vm.ProductId);
                if (product != null)
                {
                    // Optional: recalc instead of adding blindly (depends on your logic)
                    if (transaction.Type == "Purchase" || transaction.Type == "Return")
                        product.Quantity += vm.Quantity;
                    else if (transaction.Type == "Sale" || transaction.Type == "Adjustment")
                        product.Quantity -= vm.Quantity;
                }

                await _db.SaveChangesAsync();

                TempData["success"] = "Transaction updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns for redisplay
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            // Optional: adjust stock back if needed
            var product = await _db.Products.FindAsync(transaction.ProductId);
            if (product != null)
            {
                if (transaction.Type == "Purchase" || transaction.Type == "Return")
                    product.Quantity -= transaction.Quantity;
                else if (transaction.Type == "Sale" || transaction.Type == "Adjustment")
                    product.Quantity += transaction.Quantity;
            }

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();

            TempData["success"] = "Transaction deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


    }
}
