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
using Microsoft.Extensions.Logging;

namespace CoHabit.API.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IJwtService jwtService, UserManager<User> userManager, IAuthRepository authRepository, RoleManager<IdentityRole<Guid>> roleManager, ILogger<AuthService> logger)
        {
            _roleManager = roleManager;
            _authRepository = authRepository;
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task AssignRoleAsync(Guid userId, string role)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new Exception("Role does not exist");
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to remove user roles: {errors}");
            }
            result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role: {errors}");
            }
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
                _logger.LogWarning("Failed login attempt for phone: {Phone}", loginRequest.Phone);
                throw new UnauthorizedAccessException("Invalid phone number or password.");
            }
            if (user.IsRevoked)
            {
                _logger.LogWarning("Login attempt for banned user: {UserId}", user.Id);
                throw new UnauthorizedAccessException("You have been banned!");
            }
            var roles = await _userManager.GetRolesAsync(user);

            //Generate tokens
            var (jwtToken, jwtExpiresAtUtc) = _jwtService.GenerateJwtToken(user, roles);
            var (refreshToken, refreshTokenExpiresAtUtc) = _jwtService.GenerateRefreshToken();

            //Save tokens to cookie
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("AccessToken", jwtToken, jwtExpiresAtUtc);
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("RefreshToken", refreshToken, refreshTokenExpiresAtUtc);

            //Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiresAtUtc;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} logged in successfully", user.Id);
            
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
                _logger.LogWarning("Refresh token attempt for banned user: {UserId}", user.Id);
                throw new UnauthorizedAccessException("You have been banned!");
            }

            //Generate tokens
            var roles = await _userManager.GetRolesAsync(user);
            var (jwtToken, jwtExpiresAtUtc) = _jwtService.GenerateJwtToken(user, roles);
            var (refreshToken, refreshTokenExpiresAtUtc) = _jwtService.GenerateRefreshToken();

            //Save tokens to cookie
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("AccessToken", jwtToken, jwtExpiresAtUtc);
            _jwtService.WriteAuthTokenAsHttpOnlyCookie("RefreshToken", refreshToken, refreshTokenExpiresAtUtc);

            //Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiresAtUtc;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Token refreshed successfully for user: {UserId}", user.Id);

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
                PhoneNumber = request.Phone,
                Email = request.Email,
                EmailConfirmed = true,
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

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Logout attempt for non-existent user: {UserId}", userId);
                throw new UnauthorizedAccessException("User not found.");
            }

            // 1. Xóa refresh token khỏi database
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            
            // 2. Tăng SecurityStamp để invalidate tất cả token cũ
            await _userManager.UpdateSecurityStampAsync(user);
            
            // 3. Cập nhật user
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to logout user {UserId}: {Errors}", userId, errors);
                throw new Exception("Failed to logout user.");
            }

            // 4. Xóa cookies
            _jwtService.DeleteAuthCookie("AccessToken");
            _jwtService.DeleteAuthCookie("RefreshToken");

            _logger.LogInformation("User {UserId} logged out successfully", userId);
        }
    }
}