using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class PostFeedbackRepository : IPostFeedbackRepository
    {
        private readonly CoHabitDbContext _context;
        public PostFeedbackRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        
        public void AddPostFeedbackAsync(PostFeedback postFeedback)
        {
            _context.PostFeedbacks.AddAsync(postFeedback);
        }

        public void UpdatePostFeedbackAsync(PostFeedback postFeedback)
        {
            _context.PostFeedbacks.Update(postFeedback);
        }

        public async Task<IEnumerable<PostFeedback>> GetPostFeedbacksByPostIdAsync(Guid postId)
        {
            return await _context.PostFeedbacks
                .Where(pf => pf.PostId == postId && !pf.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<PostFeedback?> GetPostFeedbackByIdAsync(Guid postFeedbackId)
        {
            return await _context.PostFeedbacks
                .FirstOrDefaultAsync(pf => pf.Id == postFeedbackId);
        }

        public async Task<bool> IsUserAlreadyFeedbackByPostId(Guid userId, Guid postId)
        {
            return await _context.PostFeedbacks
                .FirstOrDefaultAsync(pf => pf.UserId == userId && pf.PostId == postId) != null;
        }
    }
}