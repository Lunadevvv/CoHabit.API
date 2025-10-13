using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoHabit.API.Enums;

namespace CoHabit.API.Enitites
{
    public class Payment
    {
        public required string PaymentId { get; set; }
        public required string PaymentLinkId { get; set; }
        public int Price { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}