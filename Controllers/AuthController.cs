using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IAuthService _authService;
        public AuthController(IOtpService otpService, IAuthService authService)
        {
            _authService = authService;
            _otpService = otpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterUserAsync(request);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "User registered successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var response = await _authService.LoginUserAync(loginRequest);
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken"); // Xóa cookie đăng nhập
            Response.Cookies.Delete("RefreshToken"); // Xóa cookie đăng nhập

            return Ok("Log out successful");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshJwtTokenAsync(request);
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(response, "Token refreshed successfully."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
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

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "OTP verified successfully."));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");

                await _authService.ChangePasswordAsync(userId, request);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Password changed successfully."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await _authService.ForgotPasswordAsync(request);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Reset password successfully."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }
    }
}