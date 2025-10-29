using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(Guid messageId);
        Task<List<Message>> GetConversationMessagesAsync(Guid conversationId, int page = 1, int pageSize = 50);
        Task<Message> CreateAsync(Message message);
        Task MarkAsReadAsync(Guid messageId);
        Task MarkConversationMessagesAsReadAsync(Guid conversationId, Guid userId);
        Task<int> GetUnreadCountAsync(Guid conversationId, Guid userId);
    }
}
