using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid InterestedUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }
        public bool IsActive { get; set; } = true;
        [JsonIgnore]
        public virtual Post? Post { get; set; }
        [JsonIgnore]
        public virtual User? Owner { get; set; }
        [JsonIgnore]
        public virtual User? InterestedUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message>? Messages { get; set; }
    }
}