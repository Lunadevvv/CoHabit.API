using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class AppFeedbacksResponse
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}