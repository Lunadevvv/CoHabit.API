using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Services.Interfaces
{
    public interface IChatService
    {
        Task <ConversationResponse> CreateOrGetConversationAsync(Guid userId, Guid PostId);
        Task <List<ConversationResponse>> GetUserConversationsAsync(Guid userId);
        Task <List<MessageResponse>> GetConversationMessagesAsync(Guid userId, Guid conversationId, int page = 1);
        Task <MessageResponse> SendMessageAsync(Guid userId, SendMessageRequest request);
        Task <bool> MarkMessagesAsReadAsync(Guid userId, Guid conversationId);
    }
}