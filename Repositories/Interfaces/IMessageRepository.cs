using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetConversationMessagesAsync(Guid conversationId, int page = 1, int pageSize = 50);
        Task CreateAsync(Message message);
        Task MarkAsReadAsync(Guid messageId);
        Task<int> GetUnreadCountAsync(Guid conversationId, Guid userId);
        Task<int> SaveChangesAsync();
    }
}