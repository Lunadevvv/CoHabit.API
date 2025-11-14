using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IUserSubcriptionRepository
    {
        public Task<UserSubcription> GetActiveSubcriptionByUserId(Guid userId);
        public Task<UserSubcription?> GetUserSubcriptionById(int userSubscriptionId);
        public Task<UserSubcription?> GetOtherActiveUserSubscriptionAsync(Guid userId, int userSubscriptionId);
        public Task<UserSubcription?> GetActiveSubcriptionByUserIdAndSubId(Guid userId, int subId);
        public Task<UserSubcription> AddUserSubcription(UserSubcription userSubcription);
        public Task<int> UpdateUserSubcriptionStatus(UserSubcription userSubcription, bool isActive);
        public Task<int> SaveChangesAsync();
    }
}