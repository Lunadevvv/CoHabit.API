using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.Enitites
{
    public class PostImage
    {
        public int Id { get; set; }
        public Guid PostId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public virtual Post Post { get; set; }
    }
}