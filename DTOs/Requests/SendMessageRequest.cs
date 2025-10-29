using System;
using System.ComponentModel.DataAnnotations;

namespace CoHabit.API.DTOs.Requests
{
    public class SendMessageRequest
    {
        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;
    }
}
