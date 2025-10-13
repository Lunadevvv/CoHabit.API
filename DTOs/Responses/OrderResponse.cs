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
        public Guid PostId { get; set; }
        public User? User { get; set; }
    }
}