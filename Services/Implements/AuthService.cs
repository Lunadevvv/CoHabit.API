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

        public Task RegisterUserAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}