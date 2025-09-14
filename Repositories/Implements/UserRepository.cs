using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly CoHabitDbContext _context;
        public UserRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsRevoked)
                .Include(u => u.Characteristics)
                .FirstOrDefaultAsync();
        }
    }
}