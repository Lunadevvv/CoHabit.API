using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class ConversationResponse
    {
        public Guid ConversationId { get; set; }
        public Guid PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string PostAddress { get; set; } = string.Empty;
        public double PostRating { get; set; }
        public int PostPrice { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerImage { get; set; } = string.Empty;
        public Guid InterestedUserId { get; set; }
        public string InterestedUserName { get; set; } = string.Empty;
        public string InterestedUserImage { get; set; } = string.Empty;
        public DateTime? LastMessageAt { get; set; }
        public string? LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}