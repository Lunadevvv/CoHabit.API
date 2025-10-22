using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.DTOs.Responses
{
    public record ProfileResponse(
        Guid Id,
        string FullName,
        string Phone,
        string Yob,
        string Sex,
        string Image,
        string Role
    );
    
}