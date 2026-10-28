
using CampusFix.Data;
using CampusFix.Interfaces;
using CampusFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.Controllers
{
    [Authorize(Roles = "Student")]
    public class MaintenanceRequestsController : Controller
    {
        private readonly IMaintenanceRequestService _service;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MaintenanceRequestsController(
            IMaintenanceRequestService service,
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new RequestCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            RequestCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            if (!await _db.Categories.AnyAsync(
                c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(
                    nameof(model.CategoryId),
                    "Please select a valid category.");

                await LoadCategoriesAsync();
                return View(model);
            }

            if (!Enum.IsDefined(typeof(RequestPriority), model.Priority))
            {
                ModelState.AddModelError(
                    nameof(model.Priority),
                    "Please select a valid priority.");

                await LoadCategoriesAsync();
                return View(model);
            }

            var request = new MaintenanceRequest
            {
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                Location = model.Location.Trim(),
                CategoryId = model.CategoryId,
                Priority = model.Priority,
                Status = RequestStatus.Submitted,
                DateCreated = DateTime.UtcNow,
                UserId = userId
            };

            await _service.CreateRequestAsync(request);

            TempData["SuccessMessage"] =
                "Your maintenance request was submitted successfully.";

            return RedirectToAction(nameof(MyRequests));
        }

        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var requests =
                await _service.GetMyRequestsAsync(userId);

            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var request =
                await _service.GetRequestByIdAsync(id);

            // Students can only edit their own requests.
            if (request == null || request.UserId != userId)
                return NotFound();

            if (request.Status != RequestStatus.Submitted)
                return BadRequest(
                    "Only submitted requests can be edited.");

            await LoadCategoriesAsync();
            ViewBag.RequestId = request.Id;

            var model = new RequestCreateViewModel
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                CategoryId = request.CategoryId,
                Priority = request.Priority
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            RequestCreateViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var request =
                await _service.GetRequestByIdAsync(id);

            // Repeat the ownership check on POST.
            if (request == null || request.UserId != userId)
                return NotFound();

            if (request.Status != RequestStatus.Submitted)
                return BadRequest(
                    "Only submitted requests can be edited.");

            if (!Enum.IsDefined(typeof(RequestPriority), model.Priority))
            {
                ModelState.AddModelError(
                    nameof(model.Priority),
                    "Please select a valid priority.");
            }

            if (!await _db.Categories.AnyAsync(
                c => c.Id == model.CategoryId))
            {
                ModelState.AddModelError(
                    nameof(model.CategoryId),
                    "Please select a valid category.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                ViewBag.RequestId = id;
                return View(model);
            }

            request.Title = model.Title.Trim();
            request.Description = model.Description.Trim();
            request.Location = model.Location.Trim();
            request.CategoryId = model.CategoryId;
            request.Priority = model.Priority;

            await _service.UpdateRequestAsync(request);

            TempData["SuccessMessage"] =
                "Your maintenance request was updated successfully.";

            return RedirectToAction(nameof(MyRequests));
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Categories =
                new SelectList(categories, "Id", "Name");
        }
    }
}
