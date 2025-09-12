using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IAuthService _authService;
        public AuthController(IOtpService otpService, IAuthService authService)
        {
            _authService = authService;
            _otpService = otpService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromQuery] string phoneNumber)
        {
            var otp = await _otpService.GenerateAndSendOtpAsync(phoneNumber);
            return Ok(ApiResponse<object>.SuccessResponse(
                new { Otp = otp },
                "OTP sent successfully."
            ));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var isValid = await _otpService.VerifyOtpAsync(request.Phone, request.Code);
            if (!isValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid or expired OTP."));
            }

            return Ok(ApiResponse<object>.SuccessResponse(new{}, "OTP verified successfully."));
        }
    }
}