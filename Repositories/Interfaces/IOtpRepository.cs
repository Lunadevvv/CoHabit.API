using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task<bool> GenerateOtpAsync(Otp otp);
        Task<Otp> GetOtpByPhoneAsync(string phoneNumber);
        Task CleanupExpiredOtpsAsync();
        Task VerifiedOtpAsync(Otp otp);
    }
}