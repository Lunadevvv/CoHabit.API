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
        private readonly IPostRepository _postRepository;
        public PostFeedbackService(IPostFeedbackRepository postFeedbackRepository, IPostRepository postRepository)
        {
            _postFeedbackRepository = postFeedbackRepository;
            _postRepository = postRepository;
        }
        public async Task<int> AddPostFeedbackAsync(PostFeedback postFeedback)
        {
            //get post to update average rating
            var post = await _postRepository.GetPostByIdAsync(postFeedback.PostId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            //add feedback
            _postFeedbackRepository.AddPostFeedbackAsync(postFeedback);
            // var result = await _postFeedbackRepository.SaveChangesAsync();

            //get amount of feedbacks to calculate new average rating
            var feedbacks = await _postFeedbackRepository.GetPostFeedbacksByPostIdAsync(postFeedback.PostId);
            double totalRating = feedbacks.Sum(f => f.Rating) + postFeedback.Rating;
            double newAverageRating = totalRating / (feedbacks.Count() + 1);
            post.AverageRating = newAverageRating;
            _postRepository.UpdatePostAsync(post);
            return await _postRepository.SaveChangesAsync();
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