using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId);
        Task<PaginationResponse<IEnumerable<OrderResponse>>> GetOrdersByOwnerIdAsync(Guid ownerId, int currentPage, int pageSize);
        void CreateOrderAsync(Order order);
        Task<int> SaveChangesAsync();
    }
}