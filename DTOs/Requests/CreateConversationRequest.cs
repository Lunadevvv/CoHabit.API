using System;
using System.ComponentModel.DataAnnotations;

namespace CoHabit.API.DTOs.Requests
{
    public class CreateConversationRequest
    {
        [Required]
        public Guid PostId { get; set; }
    }
}
