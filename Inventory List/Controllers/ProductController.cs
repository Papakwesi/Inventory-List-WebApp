using Inventory_List.Data;
using Inventory_List.Models;
using Inventory_List.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventory_List.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products
                .Include(p => p.Category)
                .ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            var viewModel = new ProductVM
            {
                Product = new Product(),
                CategoryList = _db.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(vm.Product);
                _db.SaveChanges();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }

            // reload dropdown if validation fails
            vm.CategoryList = _db.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            TempData["error"] = "Error while creating product.";
            return View(vm);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var vm = new ProductVM
            {
                Product = product,
                CategoryList = _db.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(vm.Product);
                _db.SaveChanges();
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            vm.CategoryList = _db.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            TempData["error"] = "Error while updating product.";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
            {
                TempData["error"] = "Product not found!";
                return RedirectToAction(nameof(Index));
            }

            _db.Products.Remove(product);
            _db.SaveChanges();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
