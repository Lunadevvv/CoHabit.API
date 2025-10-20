using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public class PostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public string? DepositPolicy { get; set; }
        public List<string>? FurnitureIds { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}