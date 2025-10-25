using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IPostFeedbackRepository
    {
        void AddPostFeedbackAsync(PostFeedback postFeedback);
        Task<IEnumerable<PostFeedback>> GetPostFeedbacksByPostIdAsync(Guid postId);
        Task<double> GetAverageRatingByPostIdAsync(Guid postId);
        void UpdatePostFeedbackAsync(PostFeedback postFeedback);
        Task<PostFeedback?> GetPostFeedbackByIdAsync(Guid postFeedbackId);
        Task<bool> IsUserAlreadyFeedbackByPostId(Guid userId, Guid postId);
        Task<int> SaveChangesAsync();
    }
}