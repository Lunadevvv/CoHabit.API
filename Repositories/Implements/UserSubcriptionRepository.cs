using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class UserSubcriptionRepository : IUserSubcriptionRepository
    {
        private readonly CoHabitDbContext _context;
        public UserSubcriptionRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<UserSubcription> GetActiveSubcriptionByUserId(Guid userId)
        {
            return await _context.UserSubcriptions
                        .Where(us => us.UserId == userId && us.IsActive)
                        .Include(us => us.Subcription)
                        .FirstOrDefaultAsync() ?? throw new Exception("No active subcription found for this user");
        }

        public async Task<UserSubcription?> GetUserSubcriptionById(int userSubscriptionId)
        {
            return await _context.UserSubcriptions
                        .Include(us => us.Subcription)
                        .FirstOrDefaultAsync(us => us.UserSubcriptionId == userSubscriptionId);
        }

        public async Task<UserSubcription> AddUserSubcription(UserSubcription userSubcription)
        {
            _context.UserSubcriptions.Add(userSubcription);
            await _context.SaveChangesAsync();
            
            // Ensure ID is populated by EF Core after SaveChanges
            return userSubcription;
        }

        public async Task<int> UpdateUserSubcriptionStatus(UserSubcription userSubcription, bool isActive)
        {
            userSubcription.IsActive = isActive;
            _context.UserSubcriptions.Update(userSubcription);
            return await _context.SaveChangesAsync();
        }

        public async Task<UserSubcription?> GetActiveSubcriptionByUserIdAndSubId(Guid userId, int subId)
        {
            return await _context.UserSubcriptions
                        .Where(us => us.UserId == userId && us.IsActive && us.SubcriptionId == subId)
                        .Include(us => us.Subcription)
                        .FirstOrDefaultAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<UserSubcription?> GetOtherActiveUserSubscriptionAsync(Guid userId, int userSubscriptionId)
        {
            return await _context.UserSubcriptions
                        .Include(us => us.Subcription)
                        .OrderByDescending(us => us.EndDate)
                        .FirstOrDefaultAsync(us => us.UserId == userId && us.IsActive && us.UserSubcriptionId != userSubscriptionId);
        }
    }
}