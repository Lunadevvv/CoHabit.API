using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppFeedbackController : ControllerBase
    {
        private readonly IAppFeedbackService _appFeedbackService;
        public AppFeedbackController(IAppFeedbackService appFeedbackService)
        {
            _appFeedbackService = appFeedbackService;
        }

        //Get app feedbacks
        [HttpGet]
        public async Task<IActionResult> GetAppFeedbacksAsync()
        {
            var feedbacks =  await _appFeedbackService.GetAppFeedbacksAsync();
            return Ok(ApiResponse<List<AppFeedbacksResponse>>.SuccessResponse(feedbacks, "App feedbacks retrieved successfully"));
        }

        //Create app feedback
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAppFeedbackAsync([FromBody] AppFeedbackRequest request)
        {
            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
            
            var result = await _appFeedbackService.CreateAppFeedbackAsync(request, userId);
            if (result > 0)
            {
                return Ok(ApiResponse<string>.SuccessResponse("App feedback created successfully"));
            }
            return BadRequest(ApiResponse<string>.ErrorResponse("Failed to create app feedback"));
        }
    }
}