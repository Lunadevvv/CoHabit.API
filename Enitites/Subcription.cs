using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class Subcription
    {
        public int SubcriptionId { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int DurationInDays { get; set; }
        public virtual ICollection<UserSubcription>? UserSubcriptions { get; set; }
    }
}