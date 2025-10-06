using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllPayment();
        Task<List<Payment>> GetAllUserPayment(string userId);
        Task<string> GetUserByPaymentId(string paymentId);
        Task<Payment> GetPayment(string paymentId);
        Task<Payment> GetPaymentByPaymentLinkId(string paymentLinkId);
        Task CreatePayment(Payment payment, string userId);
        Task UpdatePaymentStatus (Payment payment);
        string GeneratePaymentId(bool useUtc = true, int randomDigits = 3);
    }
}