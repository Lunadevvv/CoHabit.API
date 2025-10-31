using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CoHabitDbContext _context;
        public MessageRepository(CoHabitDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Message message)
        {
            _context.Messages.Add(message);

            // Update conversation's LastMessageAt
            var conversation = await _context.Conversations.FindAsync(message.ConversationId);
            if (conversation != null)
            {
                conversation.LastMessageAt = message.CreatedAt;
            }
        }

        public async Task<List<Message>> GetConversationMessagesAsync(Guid conversationId, int page = 1, int pageSize = 50)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .AsSplitQuery()
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid conversationId, Guid userId)
        {
            return await _context.Messages
                .CountAsync(m => m.ConversationId == conversationId &&
                            m.SenderId != userId &&
                            !m.IsRead);
        }

        public async Task MarkAsReadAsync(Guid messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}