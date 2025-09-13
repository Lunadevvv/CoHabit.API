using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CoHabit.API.Services.Implements
{
    public class JwtService : IJwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtOptions:Secret")!));

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("JwtOptions:ExpirationTimeInMinutes"));

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtOptions:Issuer"),
                audience: _configuration.GetValue<string>("JwtOptions:Audience"),
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (jwtToken, expires);
        }

        //viết token vào cookie gửi lên client
        public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token,
            DateTime expiration)
        {
            //thêm mới cookie hoac cập nhật cookie nếu đã tồn tại
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName,
                token, new CookieOptions
                {
                    HttpOnly = true, // ngăn javascript truy cập cookie này
                    Expires = expiration,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
        }

        public (string refreshToken, DateTime expiresAtUtc) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var expiresAtUtc = DateTime.UtcNow.AddDays(_configuration.GetValue<double>("JwtOptions:RefreshTokenExpirationInDays"));
            return (refreshToken, expiresAtUtc);
        }
    }
}