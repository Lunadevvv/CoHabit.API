using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoHabit.API.Enitites
{
    public class Conversation
    {
        public Guid ConversationId { get; set; }
        public Guid PostId { get; set; }
        public Guid User1Id { get; set; }  // Post owner
        public Guid User2Id { get; set; }  // Interested user
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public virtual Post? Post { get; set; }
        [JsonIgnore]
        public virtual User? User1 { get; set; }
        [JsonIgnore]
        public virtual User? User2 { get; set; }
        [JsonIgnore]
        public virtual ICollection<Message>? Messages { get; set; }
    }
}
