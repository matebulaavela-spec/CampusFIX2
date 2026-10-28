
using CampusFix.Data;
using CampusFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories
                .Include(c => c.MaintenanceRequests)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var name = model.Name.Trim();

            if (await _db.Categories.AnyAsync(
                c => c.Name.ToLower() == name.ToLower()))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "This category already exists.");

                return View(model);
            }

            var category = new Category
            {
                Name = name,
                Description = model.Description?.Trim()
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, Category model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            var name = model.Name.Trim();

            if (await _db.Categories.AnyAsync(c =>
                c.Id != id &&
                c.Name.ToLower() == name.ToLower()))
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "Another category already uses this name.");

                return View(model);
            }

            category.Name = name;
            category.Description = model.Description?.Trim();

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories
                .Include(c => c.MaintenanceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories
                .Include(c => c.MaintenanceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            if (category.MaintenanceRequests.Any())
            {
                TempData["ErrorMessage"] =
                    "This category cannot be deleted because it is used by existing requests.";

                return RedirectToAction(nameof(Index));
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
