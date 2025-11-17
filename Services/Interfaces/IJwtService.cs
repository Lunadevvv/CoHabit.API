using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IJwtService
    {
        (string refreshToken, DateTime expiresAtUtc) GenerateRefreshToken();
        (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles);
        void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
        void DeleteAuthCookie(string cookieName);
    }
}