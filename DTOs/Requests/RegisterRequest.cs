using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record RegisterRequest
    {
        [Phone(ErrorMessage = "Invalid phone number format")]
        public required string Phone { get; set; }
        [Range(6, 20, ErrorMessage = "Password must be between 6 and 20 characters")]
        public required string Password { get; set; }
    }
}