using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CoHabitDbContext _context;
        
        public AuthRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public async Task CreateUserAccountAsync(string phone, string passwordHash)
        {
            var user = new User
            {
                Phone = phone,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users
                .Where(u => u.Phone == phone && !u.IsRevoked)
                .FirstOrDefaultAsync();
        }
    }
}