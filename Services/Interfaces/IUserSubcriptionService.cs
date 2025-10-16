using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IUserSubcriptionService
    {
        public Task<UserSubcription> GetActiveSubcriptionByUserId(Guid userId);
        public Task<UserSubcription?> GetActiveSubcriptionByUserIdAndSubId(Guid userId, int subId);
        public Task<int> AddUserSubcription(UserSubcription userSubcription);
        public Task<int> UpdateUserSubcriptionStatus(UserSubcription userSubcription, bool isActive);
    }
}