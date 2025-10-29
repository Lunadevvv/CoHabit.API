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

        public async Task<Message?> GetByIdAsync(Guid messageId)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Conversation)
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.MessageId == messageId);
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

        public async Task<Message> CreateAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            
            // Update conversation's LastMessageAt
            var conversation = await _context.Conversations.FindAsync(message.ConversationId);
            if (conversation != null)
            {
                conversation.LastMessageAt = message.CreatedAt;
                await _context.SaveChangesAsync();
            }
            
            // Reload with navigation properties
            return (await GetByIdAsync(message.MessageId))!;
        }

        public async Task MarkAsReadAsync(Guid messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkConversationMessagesAsReadAsync(Guid conversationId, Guid userId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId &&
                        m.SenderId != userId &&
                        !m.IsRead)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCountAsync(Guid conversationId, Guid userId)
        {
            return await _context.Messages
                .CountAsync(m => m.ConversationId == conversationId &&
                            m.SenderId != userId &&
                            !m.IsRead);
        }
    }
}
