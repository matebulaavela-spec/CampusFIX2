
using CampusFix.Data;
using CampusFix.Interfaces;
using CampusFix.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusFix.Repositories
{
    public class MaintenanceRequestRepository
        : IMaintenanceRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public MaintenanceRequestRepository(
            ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<MaintenanceRequest>> GetAllAsync()
        {
            return await _db.MaintenanceRequests
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRequest>> GetByUserIdAsync(
            string userId)
        {
            return await _db.MaintenanceRequests
                .Include(r => r.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetByIdAsync(int id)
        {
            return await _db.MaintenanceRequests
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(MaintenanceRequest request)
        {
            await _db.MaintenanceRequests.AddAsync(request);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
