using CoHabit.API.Enitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Repositories.Interfaces; // Added missing namespace for IOrderRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CoHabitDbContext _context;
        public OrderRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId)
        {
            var query = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.Post)
                .AsSplitQuery()
                .ToListAsync();

            return query.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                CreatedAt = o.CreatedAt,
                OwnerId = o.OwnerId,
                UserId = o.UserId,
                UserName = o.User.FirstName + " " + o.User.LastName,
                PostId = o.PostId,
                PostTitle = o.Post != null ? o.Post.Title : string.Empty,
                PostAddress = o.Post != null ? o.Post.Address : string.Empty,
                ConversationId = o.ConversationId
            }).ToList();
        }

        public async Task<PaginationResponse<IEnumerable<OrderResponse>>> GetOrdersByOwnerIdAsync(Guid ownerId, int currentPage, int pageSize)
        {
            var query = _context.Orders
                .Where(o => o.OwnerId == ownerId)
                .Include(o => o.User)
                .Include(o => o.Post)
                .AsSplitQuery()
                .AsQueryable();
            
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var orders = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            var orderResponses = orders.Select(o => new OrderResponse
            {
                OrderId = o.OrderId,
                CreatedAt = o.CreatedAt,
                OwnerId = o.OwnerId,
                UserId = o.UserId,
                UserName = o.User.FirstName + " " + o.User.LastName,
                PostId = o.PostId,
                PostTitle = o.Post != null ? o.Post.Title : string.Empty,
                PostAddress = o.Post != null ? o.Post.Address : string.Empty,
                ConversationId = o.ConversationId
            }).ToList();

            return new PaginationResponse<IEnumerable<OrderResponse>>
            {
                Items = orderResponses,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalItems,
                TotalPages = totalPages
            };
        }

        public void CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
