using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.Services.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateAndSendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string phoneNumber, string code);
        Task CleanupExpiredOtpsAsync();
    }
}