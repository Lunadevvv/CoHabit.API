using CoHabit.API.Enums;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Enitites
{
    public class User : IdentityUser<Guid>
    {
        public required string Phone { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Yob { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public string Image { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Characteristic>? Characteristics { get; set; }
    }
}
