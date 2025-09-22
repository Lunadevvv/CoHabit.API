using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Payment>> GetAllUserPayment(Guid userId)
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
        public async Task CreatePayment(Payment payment, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            payment.CreatedDate = DateTime.Now;
            payment.UpdatedDate = DateTime.Now;
            payment.User = user;
            _paymentRepository.CreatePayment(payment);
            await _paymentRepository.SaveChangesAsync();
        }

        public async Task<string> GeneratePaymentId()
        {
            var lastPayment = await _paymentRepository.GetLastPayment();
            if (lastPayment == null) return "Pm0001";
            int id = int.Parse(lastPayment.PaymentId.Substring(lastPayment.PaymentId.Length-4)) + 1; // lấy id cuối cùng và cộng thêm 1
            string generatedId = "Pm" + id.ToString("D4");
            return generatedId;
        }

        public async Task UpdatePaymentStatus(Payment payment)
        {
            payment.UpdatedDate = DateTime.Now;
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
    }
}