
using CampusFix.Models;

namespace CampusFix.Interfaces
{
    public interface IMaintenanceRequestService
    {
        Task<List<MaintenanceRequest>> GetAllRequestsAsync();

        Task<List<MaintenanceRequest>> GetMyRequestsAsync(
            string userId);
       
        Task<MaintenanceRequest?> GetRequestByIdAsync(int id);

        Task CreateRequestAsync(MaintenanceRequest request);
        Task UpdateRequestAsync(MaintenanceRequest request);
        Task UpdateStatusAsync(
            int id,
            RequestStatus status);
    }
}
