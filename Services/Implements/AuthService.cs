using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly IAuthRepository _authRepository;
        public AuthService(IJwtService jwtService, UserManager<User> userManager, IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, request.oldPassword);
            if (!isOldPasswordValid)
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }
            var result = await _userManager.ChangePasswordAsync(user, request.oldPassword, request.newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to change password: {errors}");
            }
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _authRepository.GetUserByPhoneAsync(request.phone);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, request.oldPassword);
            if (!isOldPasswordValid)
            {
                throw new UnauthorizedAccessException("Old password is incorrect.");
            }
            var result = await _userManager.ChangePasswordAsync(user, request.oldPassword, request.newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to change password: {errors}");
            }
        }

        public async Task<LoginResponse> LoginUserAync(LoginRequest loginRequest)
        {
            var user = await _authRepository.GetUserByPhoneAsync(loginRequest.Phone);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new UnauthorizedAccessException("Invalid phone number or password.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            //Generate tokens
            var (jwtToken, jwtExpiresAtUtc) = _jwtService.GenerateJwtToken(user, roles);
            var (refreshToken, refreshTokenExpiresAtUtc) = _jwtService.GenerateRefreshToken();

            //Save accesstoken to cookie
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("AccessToken", jwtToken, jwtExpiresAtUtc);
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("RefreshToken", refreshToken, refreshTokenExpiresAtUtc);

            //Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiresAtUtc;
            await _userManager.UpdateAsync(user);
            return new LoginResponse
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> RefreshJwtTokenAsync(RefreshTokenRequest request)
        {
            //Validate the refresh token
            var user = await _authRepository.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken) ?? throw new UnauthorizedAccessException("Invalid refresh token.");

            //Check if user is revoked
            if (user.IsRevoked)
            {
                throw new UnauthorizedAccessException("You have been banned!");
            }

            //Generate tokens
            var roles = await _userManager.GetRolesAsync(user);
            var (jwtToken, jwtExpiresAtUtc) = _jwtService.GenerateJwtToken(user, roles);
            var (refreshToken, refreshTokenExpiresAtUtc) = _jwtService.GenerateRefreshToken();

            //Save accesstoken and refreshtoken to cookie
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("AccessToken", jwtToken, jwtExpiresAtUtc);
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("RefreshToken", refreshToken, refreshTokenExpiresAtUtc);

            //Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiresAtUtc;
            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task RegisterUserAsync(RegisterRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.Phone,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password); //hash password trước khi lưu vào bảng AspNetUsers
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "BasicMember");
        }

        public async Task RevokeTokenAsync(Guid userId)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            user.IsRevoked = true;
            await _userManager.UpdateAsync(user);
        }
    }
}