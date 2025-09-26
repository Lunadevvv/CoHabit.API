using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileResponse?> GetUserProfileAsync(Guid userId);
        Task UpdateUserProfileAsync(Guid userId, ProfileRequest profile);
        Task<List<CharacteristicResponse>> GetUserCharacteristicsByUserIdAsync(Guid userId);
        Task UpdateUserCharacteristics(Guid userId, List<string> characteristicIds);
        Task<List<Post>> GetFavoritePostsByUserIdAsync(Guid userId);
        Task AddPostToFavoritesAsync(Guid userId, Guid postId);
        Task RemovePostFromFavoritesAsync(Guid userId, Guid postId);
    }
}