using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
namespace CoHabit.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId);
        Task<PaginationResponse<IEnumerable<OrderResponse>>> GetOrdersByOwnerIdAsync(Guid ownerId, int currentPage, int pageSize);
        Task<int> CreateOrderAsync(Guid userId, Guid postId);
    }
}