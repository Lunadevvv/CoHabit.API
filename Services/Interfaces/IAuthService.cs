using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterRequest request);
        Task<LoginResponse> LoginUserAync(LoginRequest loginRequest);
        Task<LoginResponse> RefreshJwtTokenAsync(RefreshTokenRequest request);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task RevokeTokenAsync(Guid userId);
        Task AssignRoleAsync(Guid userId, string role);
        Task LogoutAsync(Guid userId);
        Task<PaginationResponse<List<GetUsersByPagingResponse>>> GetUsersByPagingAsync(PaginationRequest paginationRequest);
    }
}