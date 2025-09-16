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
        private readonly ICharacteristicRepository _characteristicRepository;
        private readonly UserManager<User> _userManager;
        public ProfileService(IAuthRepository authRepository, UserManager<User> userManager, ICharacteristicRepository characteristicRepository)
        {
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
                user.Phone,
                user.Yob,
                user.Sex.ToString(),
                user.Image
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
    }
}