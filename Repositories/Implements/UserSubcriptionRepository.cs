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

        public async Task<int> AddUserSubcription(UserSubcription userSubcription)
        {
            _context.UserSubcriptions.Add(userSubcription);
            return await _context.SaveChangesAsync();
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
    }
}