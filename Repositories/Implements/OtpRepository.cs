using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class OtpRepository : IOtpRepository
    {
        private readonly CoHabitDbContext _context;
        
        public OtpRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task CleanupExpiredOtpsAsync()
        {
            var expiredOtps = await _context.Otps
                .Where(o => o.ExpiredAt <= DateTime.UtcNow)
                .ToListAsync();

            _context.Otps.RemoveRange(expiredOtps);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GenerateOtpAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Otp> GetOtpByPhoneAsync(string phoneNumber)
        {
            var otp = await _context.Otps
                .Where(o => o.Phone == phoneNumber && !o.IsUsed && o.ExpiredAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if(otp == null)
            {
                throw new KeyNotFoundException("Mã OTP này đã hết hạn.");
            }
            return otp;
        }

        public async Task VerifiedOtpAsync(Otp otp)
        {
            otp.IsUsed = true;
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}