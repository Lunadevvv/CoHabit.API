using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task CreateUserAccountAsync(string phone, string passwordHash);
        Task<User> GetUserByPhoneAsync(string phone);
    }
}