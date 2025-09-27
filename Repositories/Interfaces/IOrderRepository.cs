using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<Order>> GetOrdersByOwnerIdAsync(Guid ownerId);
        Task<int> CreateOrderAsync(Order order);
    }
}