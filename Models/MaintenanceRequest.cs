
using System.ComponentModel.DataAnnotations;

namespace CampusFix.Models
{
    public enum RequestStatus
    {
        Submitted,
        InProgress,
        Completed
    }

    public enum RequestPriority
    {
        Low,
        Medium,
        High
    }

    public class MaintenanceRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        public RequestPriority Priority { get; set; } = RequestPriority.Medium;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
