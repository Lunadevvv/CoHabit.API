using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public class PostFeedbackRequest
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Range(1, 5)]
        [Required]
        public double Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}