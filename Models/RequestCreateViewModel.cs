
using System.ComponentModel.DataAnnotations;

namespace CampusFix.Models
{
    public class RequestCreateViewModel
    {
        [Required(ErrorMessage = "Please enter a request title.")]
        [StringLength(120)]
        [Display(Name = "Request title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the problem.")]
        [StringLength(2000)]
        [Display(Name = "Problem description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the location.")]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Please select a category.")]
        [Display(Name = "Maintenance category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public RequestPriority Priority { get; set; }
            = RequestPriority.Medium;
    }
}
