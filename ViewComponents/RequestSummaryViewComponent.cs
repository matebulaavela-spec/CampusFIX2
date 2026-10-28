
using CampusFix.Data;
using CampusFix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.ViewComponents
{
    public class RequestSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public RequestSummaryViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var summary = new RequestSummaryViewModel
            {
                TotalRequests = await _db.MaintenanceRequests
                    .CountAsync(),

                SubmittedRequests = await _db.MaintenanceRequests
                    .CountAsync(r => r.Status == RequestStatus.Submitted),

                InProgressRequests = await _db.MaintenanceRequests
                    .CountAsync(r => r.Status == RequestStatus.InProgress),

                CompletedRequests = await _db.MaintenanceRequests
                    .CountAsync(r => r.Status == RequestStatus.Completed)
            };

            return View(summary);
        }
    }

    public class RequestSummaryViewModel
    {
        public int TotalRequests { get; set; }
        public int SubmittedRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
    }
}
