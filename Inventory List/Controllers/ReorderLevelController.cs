using Inventory_List.Data;
using Inventory_List.Models;
using Inventory_List.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventory_List.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ReorderLevelController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReorderLevelController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var reorderLevels = _db.ReorderLevels
                .Include(r => r.Product)
                .ToList();
            return View(reorderLevels);
        }

        public IActionResult Create()
        {
            var vm = new ReorderLevelViewModel
            {
                Products = _db.Products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReorderLevelViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var reorderLevel = new ReorderLevel
                {
                    ProductId = vm.ProductId,
                    Level = vm.Level
                };

                _db.ReorderLevels.Add(reorderLevel);
                _db.SaveChanges();
                TempData["success"] = "Reorder level created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown if validation fails
            vm.Products = _db.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View(vm);
        }
        public IActionResult Edit(int id)
        {
            var reorderLevel = _db.ReorderLevels.Find(id);
            if (reorderLevel == null)
            {
                return NotFound();
            }

            var vm = new ReorderLevelViewModel
            {
                Id = reorderLevel.Id,
                ProductId = reorderLevel.ProductId,
                Level = reorderLevel.Level,
                Products = _db.Products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReorderLevelViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var reorderLevel = _db.ReorderLevels.Find(vm.Id);
                if (reorderLevel == null)
                {
                    return NotFound();
                }

                reorderLevel.ProductId = vm.ProductId;
                reorderLevel.Level = vm.Level;

                _db.ReorderLevels.Update(reorderLevel);
                _db.SaveChanges();

                TempData["success"] = "Reorder level updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown if validation fails
            vm.Products = _db.Products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var recordLevel = _db.ReorderLevels.Find(id);
            if (recordLevel == null)
            {
                TempData["error"] = "Record level not found!";
                return RedirectToAction(nameof(Index));
            }

            _db.ReorderLevels.Remove(recordLevel);
            _db.SaveChanges();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
