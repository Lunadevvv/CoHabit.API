using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoHabit.API.Enums;

namespace CoHabit.API.Enitites
{
    public class Post
    {
        public Guid PostId { get; set; }
        public required string Title { get; set; }
        public required string Address { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public string? DepositPolicy { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Furniture>? Furnitures { get; set; }
        [JsonIgnore]
        public virtual ICollection<User>? LikedByUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}