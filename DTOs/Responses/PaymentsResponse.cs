using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class PaymentsResponse
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public int Price { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SubcriptionId { get; set; }
    }
}