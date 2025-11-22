using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class AppFeedbackRepository : IAppFeedbackRepository
    {
        private readonly CoHabitDbContext _context;
        public AppFeedbackRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public void CreateAppFeedbackAsync(AppFeedback appFeedback)
        {
            _context.AppFeedbacks.AddAsync(appFeedback);
        }

        public async Task<List<AppFeedback>> GetAppFeedbacksAsync()
        {
            return await _context.AppFeedbacks
                    .Include(af => af.User)
                    .OrderByDescending(af => af.Rating)
                    .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}