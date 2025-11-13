using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class UserSubcriptionService : IUserSubcriptionService
    {
        private readonly IUserSubcriptionRepository _userSubcriptionRepository;
        public UserSubcriptionService(IUserSubcriptionRepository userSubcriptionRepository)
        {
            _userSubcriptionRepository = userSubcriptionRepository;
        }

        public async Task<UserSubcription> GetActiveSubcriptionByUserId(Guid userId)
        {
            return await _userSubcriptionRepository.GetActiveSubcriptionByUserId(userId);
        }

        public async Task<int> AddUserSubcription(UserSubcription userSubcription)
        {
            return await _userSubcriptionRepository.AddUserSubcription(userSubcription);
        }

        public async Task<int> UpdateUserSubcriptionStatus(UserSubcription userSubcription, bool isActive)
        {
            return await _userSubcriptionRepository.UpdateUserSubcriptionStatus(userSubcription, isActive);
        }

        public async Task<UserSubcription?> GetActiveSubcriptionByUserIdAndSubId(Guid userId, int subId)
        {
            return await _userSubcriptionRepository.GetActiveSubcriptionByUserIdAndSubId(userId, subId);
        }
    }
}