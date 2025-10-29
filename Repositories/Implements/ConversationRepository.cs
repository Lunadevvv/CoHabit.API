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

        public async Task<Conversation?> GetByIdAsync(Guid conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Post)
                    .ThenInclude(p => p!.PostImages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages!.OrderByDescending(m => m.CreatedAt).Take(1))
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

        public async Task<Conversation?> GetByPostAndUsersAsync(Guid postId, Guid user1Id, Guid user2Id)
        {
            return await _context.Conversations
                .Include(c => c.Post)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.PostId == postId &&
                    ((c.User1Id == user1Id && c.User2Id == user2Id) ||
                     (c.User1Id == user2Id && c.User2Id == user1Id)));
        }

        public async Task<List<Conversation>> GetUserConversationsAsync(Guid userId)
        {
            return await _context.Conversations
                .Include(c => c.Post)
                    .ThenInclude(p => p!.PostImages)
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages!.OrderByDescending(m => m.CreatedAt).Take(1))
                .AsSplitQuery()
                .Where(c => (c.User1Id == userId || c.User2Id == userId) && c.IsActive)
                .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
            
            // Reload with navigation properties
            return (await GetByIdAsync(conversation.ConversationId))!;
        }

        public async Task UpdateAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId)
        {
            return await _context.Conversations
                .AnyAsync(c => c.ConversationId == conversationId &&
                    (c.User1Id == userId || c.User2Id == userId));
        }
    }
}
