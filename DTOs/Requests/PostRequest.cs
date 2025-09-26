using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record PostRequest(
        string Title,
        string Address,
        int Price,
        string? Description,
        string? Condition,
        string? DepositPolicy
        );
}