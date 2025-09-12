using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;
        public AuthService(IJwtService jwtService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
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