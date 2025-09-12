using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;

namespace CoHabit.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterRequest request);
    }
}