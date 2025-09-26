using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
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

        public async Task AddFavoritePostAsync(User user, Post post)
        {
            user.FavoritePosts.Add(post);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoritePostAsync(User user, Post post)
        {
            user.FavoritePosts.Remove(post);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetFavoritePostsByUserIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.FavoritePosts)
                .FirstOrDefaultAsync();

            return user?.FavoritePosts?.ToList() ?? new List<Post>();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId && !u.IsRevoked)
                .Include(u => u.Characteristics)
                .Include(u => u.FavoritePosts)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users
                .Where(u => u.PhoneNumber == phone)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Characteristic>> GetUserCharacteristicsByUserIdAsync(Guid userId)
        {
            var user = await _context.Users
                        .Where(u => u.Id == userId)
                        .Include(u => u.Characteristics)
                        .FirstOrDefaultAsync();
            return user?.Characteristics?.ToList() ?? new List<Characteristic>();
        }

        public async Task UpdateUserCharacteristics(User user, List<Characteristic> characteristics)
        {
            user.Characteristics = characteristics;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            return await _context.Users
                .Where(u => u.Id == userId && u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }
    }
}