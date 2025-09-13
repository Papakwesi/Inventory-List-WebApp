using Inventory_List.Data;
using Inventory_List.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory_List.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SupplierController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var suppliers = _db.Suppliers.ToList();
            return View(suppliers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _db.Suppliers.Add(supplier);
                _db.SaveChanges();
                TempData["success"] = "Supplier added successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error while adding supplier.";
            return View(supplier);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _db.Suppliers.Update(supplier);
                _db.SaveChanges();
                TempData["success"] = "Supplier updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error while updating supplier.";
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var supplier = _db.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData["error"] = "Supplier not found!";
                return RedirectToAction(nameof(Index));
            }

            _db.Suppliers.Remove(supplier);
            _db.SaveChanges();
            TempData["success"] = "Supplier deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
