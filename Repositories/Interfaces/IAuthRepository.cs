using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        Task<List<Characteristic>> GetUserCharacteristicsByUserIdAsync(Guid userId);
        Task UpdateUserCharacteristics(User user, List<Characteristic> characteristics);
        Task<List<Post>>? GetFavoritePostsByUserIdAsync(Guid userId);
        Task AddFavoritePostAsync(User user, Post post);
        Task RemoveFavoritePostAsync(User user, Post post);
    }
}