using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Services.Implements
{
    public class ProfileService : IProfileService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly UserManager<User> _userManager;
        public ProfileService(IAuthRepository authRepository, UserManager<User> userManager, ICharacteristicRepository characteristicRepository, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _characteristicRepository = characteristicRepository;
            _userManager = userManager;
            _authRepository = authRepository;
        }

        public async Task<ProfileResponse?> GetUserProfileAsync(Guid userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new ProfileResponse(
                user.Id,
                $"{user.FirstName} {user.LastName}",
                user.PhoneNumber ?? string.Empty,
                user.Yob,
                user.Sex.ToString(),
                user.Image,
                (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "BasicMember"
            );
        }

        public async Task<List<CharacteristicResponse>> GetUserCharacteristicsByUserIdAsync(Guid userId)
        {
            var characteristic = await _authRepository.GetUserCharacteristicsByUserIdAsync(userId);
            return characteristic.Select(c => new CharacteristicResponse(c.CharId, c.Title)).ToList();
        }

        public async Task UpdateUserCharacteristics(Guid userId, List<string> characteristicIds)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User Not Found!");

            // Xóa hết đặc điểm hiện tại của người dùng
            user.Characteristics.Clear();

            var characteristics = await _characteristicRepository.GetAllCharacteristicsAsync();
            // So sánh và lấy ra danh sách các đặc điểm có CharId nằm trong characteristicIds
            var selectedCharacteristics = characteristics
                .Where(c => characteristicIds.Contains(c.CharId))
                .ToList();
                
            await _authRepository.UpdateUserCharacteristics(user, selectedCharacteristics);
        }

        public async Task UpdateUserProfileAsync(Guid userId, ProfileRequest profile)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Yob = profile.Yob;
            user.Sex = profile.Sex;
            user.Image = profile.Image;

            await _userManager.UpdateAsync(user);
        }

        public async Task<List<Post>> GetFavoritePostsByUserIdAsync(Guid userId)
        {
            return await _authRepository.GetFavoritePostsByUserIdAsync(userId) ?? new List<Post>();
        }

        public async Task AddPostToFavoritesAsync(Guid userId, Guid postId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User Not Found!");

            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post == null)
                throw new Exception("Post Not Found!");

            if (user.FavoritePosts == null)
            {
                user.FavoritePosts = new List<Post>();
            }
            else if (user.FavoritePosts.Any(p => p.PostId == postId))
            {
                throw new Exception("Post already in favorites");
            }
            else if (post.UserId == userId)
            {
                throw new Exception("Cannot add your own post to favorites");
            }

            await _authRepository.AddFavoritePostAsync(user, post);
        }

        public async Task RemovePostFromFavoritesAsync(Guid userId, Guid postId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User Not Found!");

            var post = await _postRepository.IsPostInFavoritesAsync(userId, postId);

            if (post == null)
                throw new Exception("Post Not Found!");
            
            await _authRepository.RemoveFavoritePostAsync(user, post);
        }
    }
}