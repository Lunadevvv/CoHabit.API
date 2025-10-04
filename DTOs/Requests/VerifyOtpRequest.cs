using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record VerifyOtpRequest
    {
        public required string Phone { get; set; }
        public required string Email { get; set; }
        [MaxLength(6)]
        public required string Code { get; set; }
    }
}