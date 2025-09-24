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
    public class FurnitureService : IFurnitureService
    {
        private readonly IFurnitureRepository _furnitureRepository;
        public FurnitureService(IFurnitureRepository furnitureRepository)
        {
            _furnitureRepository = furnitureRepository;
        }
        public async Task<List<Furniture>> GetFurnituresAsync()
        {
            return await _furnitureRepository.GetFurnituresAsync();
        }

        public async Task CreateFurnitureAsync(string name)
        {
            var furniture = new Furniture
            {
                FurId = await GenerateFurnitureId(),
                Name = name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _furnitureRepository.CreateFurnitureAsync(furniture);
            await _furnitureRepository.SaveChangesAsync();
        }

        public async Task DeleteFurnitureAsync(string furId)
        {
            var furniture = await _furnitureRepository.GetFurnitureByIdAsync(furId);
            furniture.IsActive = false; // Soft delete
            furniture.UpdatedAt = DateTime.UtcNow;
            _furnitureRepository.UpdateOrDeleteFurnitureAsync(furniture);
            await _furnitureRepository.SaveChangesAsync();
        }

        public async Task UpdateFurnitureAsync(FurnitureRequest request)
        {
            var furniture = await _furnitureRepository.GetFurnitureByIdAsync(request.FurId);
            furniture.Name = request.Name;
            furniture.UpdatedAt = DateTime.UtcNow;
            _furnitureRepository.UpdateOrDeleteFurnitureAsync(furniture);
            await _furnitureRepository.SaveChangesAsync();
        }

        private async Task<string> GenerateFurnitureId()
        {
            var lastId = await _furnitureRepository.GetLatestFurnitureIdAsync(); // e.g., "C005"
            int newIdNumber = int.Parse(lastId.Substring(3)) + 1; // Increment the numeric part
            return $"Fur{newIdNumber:D3}"; // Format as "C" followed by a 3-digit number
        }
    }
}