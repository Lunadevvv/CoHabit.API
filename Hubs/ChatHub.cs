using System;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CoHabit.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                // Join user to their own group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
        }

        public async Task SendMessage(string conversationId, string content)
        {
            try
            {
                var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    await Clients.Caller.SendAsync("ReceiveError", "User not authenticated");
                    return;
                }

                if (!Guid.TryParse(conversationId, out var convId))
                {
                    await Clients.Caller.SendAsync("ReceiveError", "Invalid conversation ID");
                    return;
                }

                var request = new SendMessageRequest
                {
                    ConversationId = convId,
                    Content = content
                };

                var result = await _chatService.SendMessageAsync(userId, request);

                if (result.Success && result.Data != null)
                {
                    // Send to all users in the conversation
                    await Clients.Group($"conversation_{conversationId}").SendAsync("ReceiveMessage", result.Data);
                    
                    // Also notify both users in case they're not actively in the conversation
                    await Clients.Group($"user_{result.Data.SenderId}").SendAsync("NewMessage", result.Data);
                    
                    // Get the other user ID from the conversation and notify them
                    await Clients.GroupExcept($"conversation_{conversationId}", Context.ConnectionId)
                        .SendAsync("NewMessageNotification", result.Data);
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveError", result.Message);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", $"Error sending message: {ex.Message}");
            }
        }

        public async Task MarkMessagesAsRead(string conversationId)
        {
            try
            {
                var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return;
                }

                if (!Guid.TryParse(conversationId, out var convId))
                {
                    return;
                }

                var result = await _chatService.MarkMessagesAsReadAsync(userId, convId);

                if (result.Success)
                {
                    // Notify other users in the conversation that messages were read
                    await Clients.OthersInGroup($"conversation_{conversationId}")
                        .SendAsync("MessagesRead", conversationId, userId);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", $"Error marking messages as read: {ex.Message}");
            }
        }

        public async Task UserTyping(string conversationId)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                await Clients.OthersInGroup($"conversation_{conversationId}")
                    .SendAsync("UserTyping", conversationId, userIdClaim);
            }
        }

        public async Task UserStoppedTyping(string conversationId)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                await Clients.OthersInGroup($"conversation_{conversationId}")
                    .SendAsync("UserStoppedTyping", conversationId, userIdClaim);
            }
        }
    }
}
