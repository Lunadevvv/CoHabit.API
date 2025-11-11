using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPostFeedbackService
    {
        Task<int> AddPostFeedbackAsync(PostFeedback postFeedback);
        Task<PaginationResponse<IEnumerable<PostFeedbackResponse>>> GetPostFeedbacksByPostIdAsync(Guid postId, int currentPage, int pageSize, double? averageRating);
        Task<int> DeletePostFeedbackAsync(Guid postFeedbackId);
        Task<bool> IsUserAlreadyFeedbackByPostId(Guid userId, Guid postId);
    }
}