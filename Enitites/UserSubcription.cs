using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class UserSubcription
    {
        public int UserSubcriptionId { get; set; }
        public Guid UserId { get; set; }
        public int SubcriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Subcription Subcription { get; set; }
    }
}