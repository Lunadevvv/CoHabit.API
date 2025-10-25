using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class PostFeedbackService : IPostFeedbackService
    {
        private readonly IPostFeedbackRepository _postFeedbackRepository;
        public PostFeedbackService(IPostFeedbackRepository postFeedbackRepository)
        {
            _postFeedbackRepository = postFeedbackRepository;
        }
        public async Task<int> AddPostFeedbackAsync(PostFeedback postFeedback)
        {
            _postFeedbackRepository.AddPostFeedbackAsync(postFeedback);
            return await _postFeedbackRepository.SaveChangesAsync();
        }

        public async Task<int> DeletePostFeedbackAsync(Guid postFeedbackId)
        {
            var postFeedback = await _postFeedbackRepository.GetPostFeedbackByIdAsync(postFeedbackId);
            if (postFeedback == null)
            {
                throw new Exception("Post feedback not found");
            }
            postFeedback.IsDeleted = true;
            _postFeedbackRepository.UpdatePostFeedbackAsync(postFeedback);
            return await _postFeedbackRepository.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingByPostIdAsync(Guid postId)
        {
            return await _postFeedbackRepository.GetAverageRatingByPostIdAsync(postId);
        }

        public async Task<IEnumerable<PostFeedback>> GetPostFeedbacksByPostIdAsync(Guid postId)
        {
            return await _postFeedbackRepository.GetPostFeedbacksByPostIdAsync(postId);
        }

        public async Task<bool> IsUserAlreadyFeedbackByPostId(Guid userId, Guid postId)
        {
            return await _postFeedbackRepository.IsUserAlreadyFeedbackByPostId(userId, postId);
        }
    }
}