using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class AppFeedback
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int ExperienceScore { get; set; }
        public string? MostFavoriteFeature { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
    }
}