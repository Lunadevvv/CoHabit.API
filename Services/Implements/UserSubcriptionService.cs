using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Hangfire;
using Hangfire.Logging;

namespace CoHabit.API.Services.Implements
{
    public class UserSubcriptionService : IUserSubcriptionService
    {
        private readonly IUserSubcriptionRepository _userSubcriptionRepository;
        private readonly ILogger<UserSubcriptionService> _logger;
        private readonly IAuthService _authService;
        public UserSubcriptionService(IAuthService authService, IUserSubcriptionRepository userSubcriptionRepository, ILogger<UserSubcriptionService> logger)
        {
            _userSubcriptionRepository = userSubcriptionRepository;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserSubcription> GetActiveSubcriptionByUserId(Guid userId)
        {
            return await _userSubcriptionRepository.GetActiveSubcriptionByUserId(userId);
        }

        public async Task<UserSubcription> AddUserSubcription(UserSubcription userSubcription)
        {
            return await _userSubcriptionRepository.AddUserSubcription(userSubcription);
        }

        public async Task<int> UpdateUserSubcriptionStatus(UserSubcription userSubcription, bool isActive)
        {
            return await _userSubcriptionRepository.UpdateUserSubcriptionStatus(userSubcription, isActive);
        }

        public async Task<UserSubcription?> GetActiveSubcriptionByUserIdAndSubId(Guid userId, int subId)
        {
            return await _userSubcriptionRepository.GetActiveSubcriptionByUserIdAndSubId(userId, subId);
        }

        public async Task<int> ProcessExpiredSubscriptionAsync(int userSubscriptionId)
        {
            try
            {
                //Get the user subscription
                var userSubscription = await _userSubcriptionRepository.GetUserSubcriptionById(userSubscriptionId);
                if (userSubscription == null || !userSubscription.IsActive)
                {
                    _logger.LogWarning($"No active subscription found with id {userSubscriptionId}");
                    return 0;
                }

                _logger.LogInformation(
                        $"Processing expired subscription {userSubscriptionId} for User {userSubscription.UserId}. " +
                        $"Current subscription: {userSubscription.Subcription?.Name}, EndDate: {userSubscription.EndDate}");

                // Deactivate the current subscription
                userSubscription.IsActive = false;

                //Get other active subscription if any
                var otherSubscription = await _userSubcriptionRepository.GetOtherActiveUserSubscriptionAsync(userSubscription.UserId, userSubscriptionId);
                if (otherSubscription == null)
                {
                    _logger.LogInformation($"No other active subscription found for user {userSubscription.UserId}. User will have no active subscription now.");
                    await _authService.AssignRoleAsync(userSubscription.UserId, "BasicMember");
                }
                else
                {
                    _logger.LogInformation($"Found other subscription {otherSubscription.Subcription?.Name}, update role for user {userSubscription.UserId}.");
                    await _authService.AssignRoleAsync(userSubscription.UserId, otherSubscription.Subcription!.Name);
                }

                _logger.LogInformation($"Successfully processed expired subscription {userSubscriptionId} for User {userSubscription.UserId}");
                return await _userSubcriptionRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing expired subscription {userSubscriptionId}");
                return 0;
            }
            
        }

        public Task ScheduleExpirationJobAsync(int userSubscriptionId, DateTime endDate)
        {
            var jobId = BackgroundJob.Schedule<IUserSubcriptionService>(
                service => service.ProcessExpiredSubscriptionAsync(userSubscriptionId),
                endDate);

            _logger.LogInformation(
                $"Scheduled expiration job {jobId} for UserSubscription {userSubscriptionId}, will run at {endDate:yyyy-MM-dd HH:mm:ss} UTC");

            return Task.CompletedTask;
        }
    }
}