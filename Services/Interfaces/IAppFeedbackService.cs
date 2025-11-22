using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IAppFeedbackService
    {
        Task<List<AppFeedbacksResponse>> GetAppFeedbacksAsync();
        Task<int> CreateAppFeedbackAsync(AppFeedbackRequest appFeedback, Guid userId);
    }
}