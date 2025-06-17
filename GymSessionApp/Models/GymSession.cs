using System;
using System.ComponentModel.DataAnnotations;

namespace GymSessionApp.Models
{
    public class GymSession
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Session name is required")]
        [StringLength(100, ErrorMessage = "Session name cannot exceed 100 characters")]
        public string SessionName { get; set; }

        [Required(ErrorMessage = "Trainer name is required")]
        [StringLength(50, ErrorMessage = "Trainer name cannot exceed 50 characters")]
        public string TrainerName { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Range(1, 50, ErrorMessage = "Capacity must be between 1 and 50")]
        public int Capacity { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}