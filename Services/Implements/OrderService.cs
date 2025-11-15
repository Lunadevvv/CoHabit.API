using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPostRepository _postRepository;
        public OrderService(IOrderRepository orderRepository, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders;
        }

        public async Task<PaginationResponse<IEnumerable<OrderResponse>>> GetOrdersByOwnerIdAsync(Guid ownerId, int currentPage, int pageSize)
        {
            var orders = await _orderRepository.GetOrdersByOwnerIdAsync(ownerId, currentPage, pageSize);
            return orders;
        }

        public async Task<int> CreateOrderAsync(Guid userId, Guid postId)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    throw new Exception("Post not found");
                }
                var order = new Order
                {
                    OrderId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    OwnerId = post.UserId,
                    UserId = userId,
                    PostId = postId
                };
                _orderRepository.CreateOrderAsync(order);
                return await _orderRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}