using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly UserManager<User> _userManager;
        public PaymentService(IPaymentRepository paymentRepository, UserManager<User> userManager)
        {
            _userManager = userManager;
            _paymentRepository = paymentRepository;
        }
        public async Task<List<Payment>> GetAllPayment()
        {
            return await _paymentRepository.GetAllPayment();
        }

        public async Task<List<Payment>> GetAllUserPayment(string userId)
        {
            return await _paymentRepository.GetAllUserPayment(userId);
        }

        public async Task<Payment> GetPayment(string paymentId)
        {
            var payment = await _paymentRepository.GetPayment(paymentId);
            if (payment == null)
            {
                throw new Exception("Payment");
            }
            return payment;
        }
        public async Task CreatePayment(Payment payment, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            payment.CreatedDate = DateTime.UtcNow;
            payment.UpdatedDate = DateTime.UtcNow;
            payment.User = user;
            _paymentRepository.CreatePayment(payment);
            await _paymentRepository.SaveChangesAsync();
        }

        public string GeneratePaymentId(bool useUtc = true, int randomDigits = 3)
        {
            var now = useUtc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            // yyyyMMddHHmmssfff -> đến milliseconds
            var id = now.ToString("yyyyMMddHHmmssfff");

            // Lấy microseconds trong giây (0..999999), sau đó lấy phần vượt quá milliseconds (3 chữ số)
            long microseconds = (now.Ticks % TimeSpan.TicksPerSecond) / 10; // 1 tick = 100ns -> /10 => µs
            int microRemainder = (int)(microseconds % 1000); // phần thập phân nhỏ hơn 1 ms
            id += microRemainder.ToString("D3");

            // Thêm chữ số ngẫu nhiên an toàn (nếu cần)
            if (randomDigits > 0)
            {
                var sb = new StringBuilder(randomDigits);
                using var rng = RandomNumberGenerator.Create();
                var buf = new byte[randomDigits];
                rng.GetBytes(buf);
                foreach (var b in buf)
                    sb.Append((b % 10).ToString());
                id += sb.ToString();
            }

            return id;
        }

        public async Task UpdatePaymentStatus(Payment payment)
        {
            payment.UpdatedDate = DateTime.UtcNow;
            await _paymentRepository.UpdatePayment(payment);
            await _paymentRepository.SaveChangesAsync();
        }

        public async Task<string> GetUserByPaymentId(string paymentId)
        {
            var userId = await _paymentRepository.GetUserByPaymentId(paymentId);
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User not found for the given payment ID.");
            }
            return userId;
        }

        public async Task<Payment> GetPaymentByPaymentLinkId(string paymentLinkId)
        {
            return await _paymentRepository.GetPaymentByPaymentLinkId(paymentLinkId);
        }
    }
}