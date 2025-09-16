using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record ChangePasswordRequest(string oldPassword,string newPassword);
}