using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class AppFeedbackService : IAppFeedbackService
    {
        private readonly IAppFeedbackRepository _appFeedbackRepository;
        public AppFeedbackService(IAppFeedbackRepository appFeedbackRepository)
        {
            _appFeedbackRepository = appFeedbackRepository;
        }
        public async Task<int> CreateAppFeedbackAsync(AppFeedbackRequest request, Guid userId)
        {
            var appFeedback = new AppFeedback
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FeedbackText = request.FeedbackText,
                Rating = request.Rating,
                ExperienceScore = request.ExperienceScore,
                MostFavoriteFeature = request.MostFavoriteFeature,
                CreatedAt = DateTime.UtcNow
            };

            _appFeedbackRepository.CreateAppFeedbackAsync(appFeedback);
            return await _appFeedbackRepository.SaveChangesAsync();
        }

        public async Task<List<AppFeedbacksResponse>> GetAppFeedbacksAsync()
        {
            return await _appFeedbackRepository.GetAppFeedbacksAsync()
                .ContinueWith(task => task.Result.Select(af => new AppFeedbacksResponse
                {
                    Id = af.Id,
                    FullName = af.User?.FirstName + " " + af.User?.LastName,
                    AvatarUrl = af.User?.Image,
                    FeedbackText = af.FeedbackText,
                    Rating = af.Rating,
                    CreatedAt = af.CreatedAt
                }).ToList());
        }
    }
}