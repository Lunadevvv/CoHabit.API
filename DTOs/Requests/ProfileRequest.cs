using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enums;

namespace CoHabit.API.DTOs.Requests
{
    public record ProfileRequest
    (
        string FirstName,
        string LastName,
        string Yob,
        Sex Sex,
        string Image
    );
}