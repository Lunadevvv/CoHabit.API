using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CoHabitDbContext _context;

        public PaymentRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllPayment()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<List<Payment>> GetAllUserPayment(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment?> GetLastPayment()
        {
            return await _context.Payments
                .OrderByDescending(p => p.PaymentId)
                .FirstOrDefaultAsync();
        }
        public async Task<Payment?> GetPayment(string paymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public void CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public async Task UpdatePayment(Payment updatedPayment)
        {
            var existingPayment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == updatedPayment.PaymentId);
            if (existingPayment == null)
            {
                throw new Exception("Payment not found");
            }
            // Because payment status will be the only value that change
            if (updatedPayment.Status != null)
                existingPayment.Status = updatedPayment.Status;
        }

        public async Task<string> GetUserByPaymentId(string paymentId)
        {
            var userId = await _context.Payments
                .Where(p => p.PaymentId == paymentId)
                .Select(p => p.UserId)
                .FirstOrDefaultAsync();

            return userId.ToString();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}