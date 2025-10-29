using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByIdAsync(Guid conversationId);
        Task<Conversation?> GetByPostAndUsersAsync(Guid postId, Guid user1Id, Guid user2Id);
        Task<List<Conversation>> GetUserConversationsAsync(Guid userId);
        Task<Conversation> CreateAsync(Conversation conversation);
        Task UpdateAsync(Conversation conversation);
        Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId);
    }
}
