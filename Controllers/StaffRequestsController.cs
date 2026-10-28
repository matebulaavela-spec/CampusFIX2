
using CampusFix.Interfaces;
using CampusFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusFix.Controllers
{
    [Authorize(Roles = "Maintenance,Administrator")]
    public class StaffRequestsController : Controller
    {
        private readonly IMaintenanceRequestService _service;

        public StaffRequestsController(
            IMaintenanceRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var requests = await _service.GetAllRequestsAsync();

            return View(requests);
        }
        [HttpGet]
        public IActionResult ApiDashboard()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            RequestStatus status)
        {
            if (!Enum.IsDefined(typeof(RequestStatus), status))
            {
                return BadRequest("Invalid request status.");
            }

            try
            {
                await _service.UpdateStatusAsync(id, status);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid request ID.");
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid request status.");
            }

            TempData["SuccessMessage"] =
                $"Request #{id} status updated to {status}.";

            return RedirectToAction(nameof(Index));
           
        }
    }
}
