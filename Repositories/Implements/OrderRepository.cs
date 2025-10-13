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

namespace CoHabit.API.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CoHabitDbContext _context;
        public OrderRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Orders
                .Where(o => o.OwnerId == ownerId)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync();
        }
    }
}
