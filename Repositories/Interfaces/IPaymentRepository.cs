using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPayment();
        Task<List<Payment>> GetAllUserPayment(string userId);
        Task<Payment?> GetLastPayment();
        Task<Payment?> GetPayment(string paymentId);
        Task<Payment?> GetPaymentByPaymentLinkId(string paymentLinkId);
        void CreatePayment(Payment payment);
        Task<string> GetUserByPaymentId(string paymentId);
        Task UpdatePayment(Payment payment);
        Task SaveChangesAsync();
    }
}