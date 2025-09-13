using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}