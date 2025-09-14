using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly ICharacteristicRepository _charRepo;
        public CharacteristicService(ICharacteristicRepository charRepo)
        {
            _charRepo = charRepo;
        }
        public async Task AddCharacteristicAsync(string title)
        {
            var characteristic = new Characteristic
            {
                CharId = await GenerateCharacteristicId(),
                Title = title,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _charRepo.AddCharacteristicAsync(characteristic);
        }

        public async Task DeleteCharacteristicAsync(string id)
        {
            var characteristic = await _charRepo.GetCharacteristicByIdAsync(id);
            if (characteristic == null)
                throw new Exception("Characteristic not found");

            characteristic.IsActive = false; // Soft delete
            await _charRepo.UpdateCharacteristicAsync(characteristic);
        }

        public async Task<List<Characteristic>> GetAllCharacteristicsAsync()
        {
            return await _charRepo.GetAllCharacteristicsAsync();
        }

        public async Task<Characteristic?> GetCharacteristicByIdAsync(string id)
        {
            return await _charRepo.GetCharacteristicByIdAsync(id);
        }

        public async Task UpdateCharacteristicAsync(CharacteristicRequest request)
        {
            var characteristic = await _charRepo.GetCharacteristicByIdAsync(request.charId);
            if (characteristic == null)
                throw new Exception("Characteristic not found");

            characteristic.Title = request.title;
            await _charRepo.UpdateCharacteristicAsync(characteristic);
        }

        private async Task<string> GenerateCharacteristicId()
        {
            var lastId = await _charRepo.GetLastCharIdAsync(); // e.g., "C005"
            int newIdNumber = int.Parse(lastId.Substring(1)) + 1; // Increment the numeric part
            return $"C{newIdNumber:D3}"; // Format as "C" followed by a 3-digit number
        }
    }
}