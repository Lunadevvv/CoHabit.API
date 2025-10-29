using System;

namespace CoHabit.API.DTOs.Responses
{
    public class ConversationResponse
    {
        public Guid ConversationId { get; set; }
        public Guid PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string PostImage { get; set; } = string.Empty;
        public Guid User1Id { get; set; }
        public string User1Name { get; set; } = string.Empty;
        public string User1Image { get; set; } = string.Empty;
        public Guid User2Id { get; set; }
        public string User2Name { get; set; } = string.Empty;
        public string User2Image { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string? LastMessage { get; set; }
        public int UnreadCount { get; set; }
        public bool IsActive { get; set; }
    }
}
