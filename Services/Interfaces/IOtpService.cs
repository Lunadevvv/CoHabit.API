using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.Services.Interfaces
{
    public interface IOtpService
    {
        Task GenerateAndSendOtpAsync(string phoneNumber, string email);
        Task<bool> VerifyOtpAsync(string phoneNumber, string email, string code);
        Task CleanupExpiredOtpsAsync();
    }
}