using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class CharacteristicRepository : ICharacteristicRepository
    {
        private readonly CoHabitDbContext _context;
        public CharacteristicRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public async Task AddCharacteristicAsync(Characteristic characteristic)
        {
            _context.Characteristics.Add(characteristic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCharacteristicAsync(Characteristic characteristic)
        {
            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Characteristic>> GetAllCharacteristicsAsync()
        {
            return await _context.Characteristics.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<Characteristic?> GetCharacteristicByIdAsync(string id)
        {
            return await _context.Characteristics
                    .Where(c => c.IsActive && c.CharId == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<string> GetLastCharIdAsync()
        {
            return await _context.Characteristics
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => c.CharId)
                    .FirstOrDefaultAsync() ?? "C000";
        }

        public async Task UpdateCharacteristicAsync(Characteristic characteristic)
        {
            _context.Characteristics.Update(characteristic);
            await _context.SaveChangesAsync();
        }
    }
}