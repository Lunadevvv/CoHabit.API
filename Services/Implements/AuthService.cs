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
    }
}