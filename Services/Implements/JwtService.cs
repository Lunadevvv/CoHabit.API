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
        private readonly IAuthRepository _authRepository;
        public JwtService(IHttpContextAccessor httpContextAccessor, IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(User user, IList<string> roles)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtOptions__Secret")!));

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expirationMinutes = double.Parse(Environment.GetEnvironmentVariable("JwtOptions__ExpirationInMinutes") ?? "30");
            var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JwtOptions__Issuer"),
                audience: Environment.GetEnvironmentVariable("JwtOptions__Audience"),
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
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            // Secure = true chỉ khi production (HTTPS), false khi development (HTTP)
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            //thêm mới cookie hoặc cập nhật cookie nếu đã tồn tại
            httpContext.Response.Cookies.Append(cookieName, token, new CookieOptions
            {
                HttpOnly = true, // ngăn javascript truy cập cookie này
                Expires = expiration,
                IsEssential = true,
                Secure = isProduction, // true cho production, false cho development
                SameSite = SameSiteMode.None,
                Path = "/" // Đảm bảo cookie available cho toàn bộ application
            });
        }

        // Xóa cookie authentication
        public void DeleteAuthCookie(string cookieName)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            // ⭐ DEBUG: Log cookie trước khi xóa
            Console.WriteLine($"[DEBUG] Attempting to delete cookie: {cookieName}");
            Console.WriteLine($"[DEBUG] Current cookies: {string.Join(", ", httpContext.Request.Cookies.Keys)}");
            Console.WriteLine($"[DEBUG] Environment: {(isProduction ? "Production" : "Development")}");

            // Xóa cookie với cùng options như lúc tạo
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1),
                IsEssential = true
            };

            httpContext.Response.Cookies.Delete(cookieName, cookieOptions);

            // ⭐ DEBUG: Verify deletion
            Console.WriteLine($"[DEBUG] Cookie deletion executed for: {cookieName}");
        }

        public (string refreshToken, DateTime expiresAtUtc) GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var refreshTokenExpirationDays = double.Parse(Environment.GetEnvironmentVariable("JwtOptions__RefreshTokenExpirationInDays") ?? "7");
            var expiresAtUtc = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
            return (refreshToken, expiresAtUtc);
        }
    }
}