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

namespace CoHabit.API.Services.Implements
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        public OtpService(IOtpRepository otpRepository, IAuthRepository authRepository, UserManager<User> userManager)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _otpRepository = otpRepository;
        }
        public async Task CleanupExpiredOtpsAsync()
        {
            await _otpRepository.CleanupExpiredOtpsAsync();
        }

        public async Task<string> GenerateAndSendOtpAsync(string phoneNumber)
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
            //Send OTP code via SMS (Placeholder)
            // SpeedSMSAPI smsApi = new SpeedSMSAPI("w0K2oXFmgQfK05bvubtWif7ddzYsG0_T");
            // var response = smsApi.sendSMS(new String[] { phoneNumber }, $"Ma OTP cua ban la: {otpCode}. Ma co hieu luc trong 5 phut.", SpeedSMSAPI.TYPE_CSKH, "84357968555");
            //Return the OTP code for testing purposes (In production, do not return the OTP code
            return otpCode;
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string code)
        {
            try
            {
                var user = await _authRepository.GetUserByPhoneAsync(phoneNumber);
                if (user == null)
                {
                    throw new Exception("User with this phone number does not exist");
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
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Verify OTP Service Error");
            }

        }
    }
}