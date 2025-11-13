using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<List<Conversation>> GetAllConversationsByUserIdAsync(Guid userId);
        // Task<Conversation?> GetByIdAsync(Guid conversationId);
        Task<Conversation?> GetByPostAndUsersAsync(Guid postId, Guid ownerId, Guid interestedUserId);
        void CreateAsync(Conversation conversation);
        Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId);
        Task<int> SaveChangesAsync();
    }
}