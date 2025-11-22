using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public class AppFeedbackRequest
    {
        [Required]
        [MaxLength(2000, ErrorMessage = "Feedback text cannot exceed 2000 characters.")]
        public string FeedbackText { get; set; } = string.Empty;
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int ExperienceScore { get; set; }
        public string? MostFavoriteFeature { get; set; }
    }
}