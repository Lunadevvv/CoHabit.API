using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;

namespace CoHabit.API.DTOs.Responses
{
    public class PostResponse
    {
        public Guid PostId { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public string? DepositPolicy { get; set; }
        public PostStatus Status { get; set; }
        public UserResponse? User { get; set; }
        public ICollection<FurnitureResponse>? Furnitures { get; set; }
    }
}