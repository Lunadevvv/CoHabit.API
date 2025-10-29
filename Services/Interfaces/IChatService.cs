using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;

namespace CoHabit.API.Services.Interfaces
{
    public interface IChatService
    {
        Task<ApiResponse<ConversationResponse>> CreateOrGetConversationAsync(Guid userId, CreateConversationRequest request);
        Task<ApiResponse<List<ConversationResponse>>> GetUserConversationsAsync(Guid userId);
        Task<ApiResponse<List<MessageResponse>>> GetConversationMessagesAsync(Guid userId, Guid conversationId, int page = 1);
        Task<ApiResponse<MessageResponse>> SendMessageAsync(Guid userId, SendMessageRequest request);
        Task<ApiResponse<bool>> MarkMessagesAsReadAsync(Guid userId, Guid conversationId);
    }
}
