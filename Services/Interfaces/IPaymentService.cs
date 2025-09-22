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
        Task<List<Payment>> GetAllUserPayment(Guid userId);
        Task<string> GetUserByPaymentId(string paymentId);
        Task<Payment> GetPayment(string paymentId);
        Task CreatePayment(Payment payment, Guid userId);
        Task UpdatePaymentStatus (Payment payment);
        Task<string> GeneratePaymentId();
    }
}