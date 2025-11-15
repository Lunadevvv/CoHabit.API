using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        [JsonIgnore]
        public virtual Post Post { get; set; }
        public Guid ConversationId { get; set; }
        [JsonIgnore]
        public virtual Conversation Conversation { get; set; }
    }
}