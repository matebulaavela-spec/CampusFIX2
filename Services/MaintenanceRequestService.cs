
using CampusFix.Interfaces;
using CampusFix.Models;

namespace CampusFix.Services
{
    public class MaintenanceRequestService
        : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _repository;

        public MaintenanceRequestService(
            IMaintenanceRequestRepository repository)
        {
            _repository = repository;
        }

        public Task<List<MaintenanceRequest>> GetAllRequestsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<List<MaintenanceRequest>> GetMyRequestsAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "A user ID is required.", nameof(userId));
            }

            return _repository.GetByUserIdAsync(userId);
        }

        public Task<MaintenanceRequest?> GetRequestByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return _repository.GetByIdAsync(id);
        }

        public async Task UpdateRequestAsync(MaintenanceRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await _repository.SaveChangesAsync();
        }

        public async Task CreateRequestAsync(
            MaintenanceRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await _repository.AddAsync(request);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(
            int id,
            RequestStatus status)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (!Enum.IsDefined(typeof(RequestStatus), status))
            {
                throw new ArgumentException(
                    "Invalid request status.", nameof(status));
            }

            var request = await _repository.GetByIdAsync(id);

            if (request == null)
            {
                throw new KeyNotFoundException(
                    $"Maintenance request {id} was not found.");
            }

            request.Status = status;

            await _repository.SaveChangesAsync();
        }
    }
}
