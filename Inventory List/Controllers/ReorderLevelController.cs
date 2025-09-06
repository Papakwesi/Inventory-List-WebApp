using Inventory_List.Data;
using Inventory_List.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventory_List.Controllers
{
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
            ViewBag.Products = _db.Products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ReorderLevel reorderLevel)
        {
            if (ModelState.IsValid)
            {
                _db.ReorderLevels.Add(reorderLevel);
                _db.SaveChanges();
                TempData["success"] = "Reorder level added successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = _db.Products.ToList();
            return View(reorderLevel);
        }

        public IActionResult Edit(int id)
        {
            var reorder = _db.ReorderLevels.Find(id);
            if (reorder == null) return NotFound();

            ViewBag.Products = _db.Products.ToList();
            return View(reorder);
        }

        [HttpPost]
        public IActionResult Edit(ReorderLevel reorderLevel)
        {
            if (ModelState.IsValid)
            {
                _db.ReorderLevels.Update(reorderLevel);
                _db.SaveChanges();
                TempData["success"] = "Reorder level updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Products = _db.Products.ToList();
            return View(reorderLevel);
        }

        public IActionResult Delete(int id)
        {
            var reorder = _db.ReorderLevels.Include(r => r.Product).FirstOrDefault(r => r.Id == id);
            if (reorder == null) return NotFound();
            return View(reorder);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var reorder = _db.ReorderLevels.Find(id);
            if (reorder == null) return NotFound();

            _db.ReorderLevels.Remove(reorder);
            _db.SaveChanges();
            TempData["success"] = "Reorder level deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
