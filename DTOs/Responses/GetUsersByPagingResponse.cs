using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class GetUsersByPagingResponse
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? avatarUrl { get; set; }
        public string? Role { get; set; }
        public string? Sex { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}