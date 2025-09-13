using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    }
}