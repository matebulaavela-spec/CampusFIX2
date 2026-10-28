using CampusFix.Interfaces;
using CampusFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusFix.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    [Authorize(Roles = "Maintenance,Administrator")]
    public class MaintenanceApiController : ControllerBase
    {
        private readonly IMaintenanceRequestService _service;

        public MaintenanceApiController(
            IMaintenanceRequestService service)
        {
            _service = service;
        }

        // GET: /api/maintenance
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _service.GetAllRequestsAsync();

            var result = requests.Select(r => new
            {
                r.Id,
                r.Title,
                r.Description,
                r.Location,
                Category = r.Category?.Name,
                Student = r.User?.FullName,
                Priority = r.Priority.ToString(),
                Status = r.Status.ToString(),
                r.DateCreated
            });

            return Ok(result);
        }

        // GET: /api/maintenance/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid request ID." });

            var request = await _service.GetRequestByIdAsync(id);

            if (request == null)
                return NotFound(new { message = "Request not found." });

            return Ok(new
            {
                request.Id,
                request.Title,
                request.Description,
                request.Location,
                Category = request.Category?.Name,
                Student = request.User?.FullName,
                Priority = request.Priority.ToString(),
                Status = request.Status.ToString(),
                request.DateCreated
            });
        }

        // POST: /api/maintenance/1/status
        [HttpPost("{id:int}/status")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateMaintenanceStatusDto model)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid request ID." });

            if (!Enum.TryParse<RequestStatus>(
                    model.Status, true, out var status) ||
                !Enum.IsDefined(typeof(RequestStatus), status))
            {
                return BadRequest(new
                {
                    message = "Status must be Submitted, InProgress or Completed."
                });
            }

            try
            {
                await _service.UpdateStatusAsync(id, status);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Request not found." });
            }

            return Ok(new
            {
                message = "Request status updated successfully.",
                requestId = id,
                status = status.ToString()
            });
        }
    }

    public class UpdateMaintenanceStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}