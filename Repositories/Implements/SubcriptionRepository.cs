using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class SubcriptionRepository : ISubcriptionRepository
    {
        private readonly CoHabitDbContext _context;
        public SubcriptionRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<Subcription> GetSubcriptionById(int subcriptionId)
        {
            return await _context.Subcriptions.Where(s => s.SubcriptionId == subcriptionId).FirstOrDefaultAsync() ?? throw new Exception("Subcription not found");
        }
    }
}