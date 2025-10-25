using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPostFeedbackService
    {
        Task<int> AddPostFeedbackAsync(PostFeedback postFeedback);
        Task<IEnumerable<PostFeedback>> GetPostFeedbacksByPostIdAsync(Guid postId);
        Task<int> DeletePostFeedbackAsync(Guid postFeedbackId);
        Task<bool> IsUserAlreadyFeedbackByPostId(Guid userId, Guid postId);
    }
}