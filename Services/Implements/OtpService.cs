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
                    subject = "Verification Code - CoHabit",
                    htmlContent = $@"
                    <html>
                    <body style=""margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif; background-color: #f5f7fa;"">
                        <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""background-color: #f5f7fa;"">
                            <tr>
                                <td style=""padding: 40px 20px;"">
                                    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.07);"">
                                        <tr>
                                            <td style=""padding: 40px 40px 30px; text-align: center; background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); border-radius: 12px 12px 0 0;"">
                                                <h1 style=""margin: 0; color: #ffffff; font-size: 28px; font-weight: 700; letter-spacing: -0.5px;"">Mã xác thực của bạn</h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding: 40px;"">
                                                <p style=""margin: 0 0 24px; color: #1e293b; font-size: 16px; line-height: 1.6;"">
                                                    Xin chào,
                                                </p>
                                                <p style=""margin: 0 0 32px; color: #475569; font-size: 15px; line-height: 1.6;"">
                                                    Bạn đã yêu cầu mã xác thực OTP. Vui lòng sử dụng mã dưới đây để hoàn tất xác thực:
                                                </p>
                                                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                                                    <tr>
                                                        <td style=""padding: 30px; background: linear-gradient(135deg, #eff6ff 0%, #dbeafe 100%); border-radius: 12px; text-align: center; border: 2px solid #3b82f6;"">
                                                            <div style=""font-size: 42px; font-weight: 700; letter-spacing: 8px; color: #1e40af; font-family: 'Courier New', monospace;"">
                                                                {otpCode}
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <p style=""margin: 32px 0 24px; color: #475569; font-size: 15px; line-height: 1.6;"">
                                                    Mã này sẽ <strong style=""color: #1e293b;"">hết hạn sau 5 phút</strong> kể từ khi được gửi.
                                                </p>
                                                <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin-top: 32px;"">
                                                    <tr>
                                                        <td style=""padding: 20px; background-color: #fef3c7; border-left: 4px solid #f59e0b; border-radius: 8px;"">
                                                            <p style=""margin: 0; color: #92400e; font-size: 14px; line-height: 1.5;"">
                                                                <strong>⚠️ Lưu ý bảo mật:</strong> Không chia sẻ mã này với bất kỳ ai. Chúng tôi sẽ không bao giờ yêu cầu mã OTP qua điện thoại hoặc email.
                                                            </p>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <p style=""margin: 32px 0 0; color: #64748b; font-size: 14px; line-height: 1.6;"">
                                                    Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi nếu bạn có bất kỳ thắc mắc nào.
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding: 30px 40px; background-color: #f8fafc; border-radius: 0 0 12px 12px; border-top: 1px solid #e2e8f0;"">
                                                <p style=""margin: 0 0 8px; color: #64748b; font-size: 13px; line-height: 1.5; text-align: center;"">
                                                    Email này được gửi từ <strong style=""color: #1e293b;"">CoHabit</strong>
                                                </p>
                                                <p style=""margin: 0; color: #94a3b8; font-size: 12px; line-height: 1.5; text-align: center;"">
                                                    © 2025 CoHabit. All rights reserved.
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>"
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
                return false;
            }

        }
    }
}