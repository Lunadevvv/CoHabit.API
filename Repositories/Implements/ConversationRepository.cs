using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly CoHabitDbContext _context;
        public ConversationRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        
        public void CreateAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
        }

        public async Task<List<Conversation>> GetAllConversationsByUserIdAsync(Guid userId)
        {
            return await _context.Conversations
                .Include(c => c.Post)
                    .ThenInclude(p => p!.PostImages)
                .Include(c => c.Owner)
                .Include(c => c.InterestedUser)
                .Include(c => c.Messages!.OrderByDescending(m => m.CreatedAt).Take(1))
                .AsSplitQuery()
                .Where(c => (c.OwnerId == userId || c.InterestedUserId == userId) && c.IsActive)
                .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Conversation?> GetByPostAndUsersAsync(Guid postId, Guid ownerId, Guid interestedUserId)
        {
            return await _context.Conversations
                .Include(c => c.Post)
                .Include(c => c.Owner)
                .Include(c => c.InterestedUser)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.PostId == postId && c.IsActive &&
                    ((c.OwnerId == ownerId && c.InterestedUserId == interestedUserId) ||
                     (c.InterestedUserId == interestedUserId && c.OwnerId == ownerId)));
        }

        public async Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId)
        {
            return await _context.Conversations
                .AnyAsync(c => c.Id == conversationId &&
                    (c.OwnerId == userId || c.InterestedUserId == userId));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}