using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
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
                .Include(pf => pf.User)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(pf => pf.PostId == postId && !pf.IsDeleted)
                .ToListAsync();
        }

        public async Task<PaginationResponse<IEnumerable<PostFeedbackResponse>>> GetPostFeedbacksPagingByPostIdAsync(Guid postId, int currentPage, int pageSize, double? averageRating)
        {
            var query = _context.PostFeedbacks
                .Include(pf => pf.User)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(pf => pf.PostId == postId && !pf.IsDeleted)
                .AsQueryable();

            if (averageRating.HasValue)
            {
                query = query.Where(pf => pf.Rating == averageRating.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var feedbacks = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var feedbackResponses = feedbacks.Select(pf => new PostFeedbackResponse
            {
                Id = pf.Id,
                PostId = pf.PostId,
                UserId = pf.UserId,
                UserName = pf.User.FirstName + " " + pf.User.LastName,
                UserAvatar = string.Empty,
                Rating = pf.Rating,
                Comment = pf.Comment,
                CreatedAt = pf.CreatedAt
            });

            return new PaginationResponse<IEnumerable<PostFeedbackResponse>>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = feedbackResponses
            } ?? new PaginationResponse<IEnumerable<PostFeedbackResponse>> { Items = new List<PostFeedbackResponse>() };
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