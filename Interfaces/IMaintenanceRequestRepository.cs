
using CampusFix.Models;

namespace CampusFix.Interfaces
{
    public interface IMaintenanceRequestRepository
    {
        Task<List<MaintenanceRequest>> GetAllAsync();

        Task<List<MaintenanceRequest>> GetByUserIdAsync(
            string userId);

        Task<MaintenanceRequest?> GetByIdAsync(int id);

        Task AddAsync(MaintenanceRequest request);

        Task SaveChangesAsync();
    }
}
