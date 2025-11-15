using System;
using CoHabit.API.Enitites;

namespace CoHabit.API.DTOs.Responses
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string PostAddress { get; set; } = string.Empty;
        public Guid ConversationId { get; set; }
    }
}