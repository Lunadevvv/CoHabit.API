using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public record UserResponse
    (
        Guid UserId,
        string FirstName,
        string LastName,
        string? PhoneNumber,
        string? AvatarUrl
    );
}