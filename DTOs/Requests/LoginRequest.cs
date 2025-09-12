using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record LoginRequest
    {
        [Phone(ErrorMessage = "Invalid phone number format")]
        public required string Phone { get; set; }
        public required string Password { get; set; }
    }
}