using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}