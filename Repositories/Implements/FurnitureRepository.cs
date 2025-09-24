using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly CoHabitDbContext _context;
        public FurnitureRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public void CreateFurnitureAsync(Furniture furniture)
        {
            _context.Furnitures.Add(furniture);
        }

        public void UpdateOrDeleteFurnitureAsync(Furniture furniture)
        {
            _context.Furnitures.Update(furniture);
        }

        public async Task<List<Furniture>> GetFurnituresAsync()
        {
            return await _context.Furnitures.Where(f => f.IsActive).ToListAsync();
        }

        public async Task<string> GetLatestFurnitureIdAsync()
        {
            return await _context.Furnitures
                    .OrderByDescending(f => f.FurId)
                    .Select(c => c.FurId)
                    .FirstOrDefaultAsync() ?? "Fur000";
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Furniture> GetFurnitureByIdAsync(string furId)
        {
            var result = await _context.Furnitures
                    .Where(f => f.IsActive && f.FurId == furId)
                    .FirstOrDefaultAsync();

            if (result == null)
                throw new Exception($"Furniture with ID {furId} not found.");

            return result;
        }
    }
}