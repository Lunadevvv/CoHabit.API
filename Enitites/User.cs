using System.Text.Json.Serialization;
using CoHabit.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Enitites
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Yob { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public string Image { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public virtual ICollection<Characteristic>? Characteristics { get; set; }
        [JsonIgnore]
        public virtual ICollection<Payment>? Payments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post>? Posts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post>? FavoritePosts { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserSubcription>? UserSubcriptions { get; set; }
        [JsonIgnore]
        public virtual ICollection<PostFeedback>? PostFeedbacks { get; set; }
    }
}
