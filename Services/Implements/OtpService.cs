using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Helpers;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CoHabit.API.Services.Implements
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BrevoConfig _config;
        public OtpService(IOtpRepository otpRepository, IAuthRepository authRepository, UserManager<User> userManager, IHttpClientFactory httpClientFactory, IOptions<BrevoConfig> config)
        {
            _config = config.Value;
            _httpClientFactory = httpClientFactory;
            _otpRepository = otpRepository;
            _authRepository = authRepository;
            _userManager = userManager;
        }
        public async Task CleanupExpiredOtpsAsync()
        {
            await _otpRepository.CleanupExpiredOtpsAsync();
        }

        public async Task GenerateAndSendOtpAsync(string phoneNumber, string email)
        {
            await CleanupExpiredOtpsAsync();

            //Check if phone number is registered
            var user = await _authRepository.GetUserByPhoneAsync(phoneNumber);
            if (user != null)
            {
                throw new Exception("Phone number is registered");
            }

            //Generate OTP code
            var otpCode = new Random().Next(100000, 999999).ToString();
            var otpHash = string.Empty;
            var salt = Array.Empty<byte>();
            
            try
            {
                //Hash Otp Code
                using (var hmac = new HMACSHA512())
                {
                    salt = hmac.Key;
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otpCode));
                    otpHash = Convert.ToBase64String(hash);
                }
            }
            catch (Exception)
            {
                throw new Exception("Hash OTP Service Error");
            }
            
            //Create Otp entity
            var otp = new Otp
            {
                OtpId = Guid.NewGuid(),
                Phone = phoneNumber,
                Email = email,
                CodeHashed = otpHash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            //Save to database
            var result = await _otpRepository.GenerateOtpAsync(otp);
            if (!result)
            {
                throw new Exception("Failed to generate OTP. Your have requested OTP too many times.");
            }
            //Send OTP code via Email (Placeholder)
            try
            {
                var client = _httpClientFactory.CreateClient("brevo");
                client.DefaultRequestHeaders.Add("api-key", _config.ApiKey ?? throw new Exception("Brevo API key is not configured."));
                var emailContent = new
                {
                    sender = new { email = "cohabit.vn@gmail.com" },
                    to = new[] { new { email = email } },
                    subject = "Your OTP Code",
                    htmlContent = $"<html><body><h1>Your OTP Code</h1><p>Your OTP code is: <strong>{otpCode}</strong></p><p>This code will expire in 5 minutes.</p></body></html>",
                };
                var json = System.Text.Json.JsonSerializer.Serialize(emailContent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("smtp/email", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new Exception("Failed to send OTP email.");
            }         
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string email, string code)
        {
            try
            {
                var user = await _authRepository.GetUserByPhoneAsync(phoneNumber);
                if (user != null)
                {
                    throw new Exception("User with this phone number existed");
                }
                var otp = await _otpRepository.GetOtpByPhoneAsync(phoneNumber);

                if (otp == null)
                {
                    return false;
                }
                var otpHash = string.Empty;
                //Hash the provided code
                using (var hmac = new HMACSHA512(otp.Salt))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));
                    otpHash = Convert.ToBase64String(hash);
                }
                if (otpHash != otp.CodeHashed)
                {
                    return false;
                }

                await _otpRepository.VerifiedOtpAsync(otp);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Verify OTP Service Error");
            }

        }
    }
}