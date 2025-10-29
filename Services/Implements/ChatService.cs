using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Helpers;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Services.Implements
{
    public class ChatService : IChatService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPostRepository _postRepository;
        private readonly CoHabitDbContext _context;

        public ChatService(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IPostRepository postRepository,
            CoHabitDbContext context)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _postRepository = postRepository;
            _context = context;
        }

        public async Task<ApiResponse<ConversationResponse>> CreateOrGetConversationAsync(Guid userId, CreateConversationRequest request)
        {
            try
            {
                // Get the post
                var post = await _context.Posts
                    .Include(p => p.PostImages)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post == null)
                {
                    return new ApiResponse<ConversationResponse>
                    {
                        Success = false,
                        Message = "Post not found",
                        Data = null
                    };
                }

                // Cannot create conversation with yourself
                if (post.UserId == userId)
                {
                    return new ApiResponse<ConversationResponse>
                    {
                        Success = false,
                        Message = "Cannot create conversation with yourself",
                        Data = null
                    };
                }

                // Check if conversation already exists
                var existingConversation = await _conversationRepository.GetByPostAndUsersAsync(
                    request.PostId, post.UserId, userId);

                if (existingConversation != null)
                {
                    var unreadCount = await _messageRepository.GetUnreadCountAsync(existingConversation.ConversationId, userId);
                    return new ApiResponse<ConversationResponse>
                    {
                        Success = true,
                        Message = "Conversation retrieved successfully",
                        Data = MapToConversationResponse(existingConversation, userId, unreadCount)
                    };
                }

                // Create new conversation
                var conversation = new Conversation
                {
                    ConversationId = Guid.NewGuid(),
                    PostId = request.PostId,
                    User1Id = post.UserId,  // Post owner
                    User2Id = userId,        // Current user
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdConversation = await _conversationRepository.CreateAsync(conversation);

                return new ApiResponse<ConversationResponse>
                {
                    Success = true,
                    Message = "Conversation created successfully",
                    Data = MapToConversationResponse(createdConversation, userId, 0)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConversationResponse>
                {
                    Success = false,
                    Message = $"Error creating conversation: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ConversationResponse>>> GetUserConversationsAsync(Guid userId)
        {
            try
            {
                var conversations = await _conversationRepository.GetUserConversationsAsync(userId);

                var conversationResponses = new List<ConversationResponse>();
                foreach (var conversation in conversations)
                {
                    var unreadCount = await _messageRepository.GetUnreadCountAsync(conversation.ConversationId, userId);
                    conversationResponses.Add(MapToConversationResponse(conversation, userId, unreadCount));
                }

                return new ApiResponse<List<ConversationResponse>>
                {
                    Success = true,
                    Message = "Conversations retrieved successfully",
                    Data = conversationResponses
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ConversationResponse>>
                {
                    Success = false,
                    Message = $"Error retrieving conversations: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<MessageResponse>>> GetConversationMessagesAsync(Guid userId, Guid conversationId, int page = 1)
        {
            try
            {
                // Verify user is part of conversation
                var isUserInConversation = await _conversationRepository.IsUserInConversationAsync(conversationId, userId);
                if (!isUserInConversation)
                {
                    return new ApiResponse<List<MessageResponse>>
                    {
                        Success = false,
                        Message = "Access denied",
                        Data = null
                    };
                }

                var messages = await _messageRepository.GetConversationMessagesAsync(conversationId, page);

                var messageResponses = messages.Select(m => new MessageResponse
                {
                    MessageId = m.MessageId,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    SenderName = $"{m.Sender?.FirstName} {m.Sender?.LastName}".Trim(),
                    SenderImage = m.Sender?.Image ?? string.Empty,
                    Content = m.Content,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt
                }).ToList();

                // Reverse to show oldest first
                messageResponses.Reverse();

                return new ApiResponse<List<MessageResponse>>
                {
                    Success = true,
                    Message = "Messages retrieved successfully",
                    Data = messageResponses
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MessageResponse>>
                {
                    Success = false,
                    Message = $"Error retrieving messages: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<MessageResponse>> SendMessageAsync(Guid userId, SendMessageRequest request)
        {
            try
            {
                // Verify user is part of conversation
                var isUserInConversation = await _conversationRepository.IsUserInConversationAsync(request.ConversationId, userId);
                if (!isUserInConversation)
                {
                    return new ApiResponse<MessageResponse>
                    {
                        Success = false,
                        Message = "Access denied",
                        Data = null
                    };
                }

                var message = new Message
                {
                    MessageId = Guid.NewGuid(),
                    ConversationId = request.ConversationId,
                    SenderId = userId,
                    Content = request.Content.Trim(),
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var createdMessage = await _messageRepository.CreateAsync(message);

                var messageResponse = new MessageResponse
                {
                    MessageId = createdMessage.MessageId,
                    ConversationId = createdMessage.ConversationId,
                    SenderId = createdMessage.SenderId,
                    SenderName = $"{createdMessage.Sender?.FirstName} {createdMessage.Sender?.LastName}".Trim(),
                    SenderImage = createdMessage.Sender?.Image ?? string.Empty,
                    Content = createdMessage.Content,
                    IsRead = createdMessage.IsRead,
                    CreatedAt = createdMessage.CreatedAt
                };

                return new ApiResponse<MessageResponse>
                {
                    Success = true,
                    Message = "Message sent successfully",
                    Data = messageResponse
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MessageResponse>
                {
                    Success = false,
                    Message = $"Error sending message: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> MarkMessagesAsReadAsync(Guid userId, Guid conversationId)
        {
            try
            {
                // Verify user is part of conversation
                var isUserInConversation = await _conversationRepository.IsUserInConversationAsync(conversationId, userId);
                if (!isUserInConversation)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Access denied",
                        Data = false
                    };
                }

                await _messageRepository.MarkConversationMessagesAsReadAsync(conversationId, userId);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Messages marked as read",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error marking messages as read: {ex.Message}",
                    Data = false
                };
            }
        }

        private ConversationResponse MapToConversationResponse(Conversation conversation, Guid currentUserId, int unreadCount)
        {
            var otherUser = conversation.User1Id == currentUserId ? conversation.User2 : conversation.User1;
            var lastMessage = conversation.Messages?.FirstOrDefault();

            return new ConversationResponse
            {
                ConversationId = conversation.ConversationId,
                PostId = conversation.PostId,
                PostTitle = conversation.Post?.Title ?? string.Empty,
                PostImage = conversation.Post?.PostImages?.FirstOrDefault()?.ImageUrl ?? string.Empty,
                User1Id = conversation.User1Id,
                User1Name = $"{conversation.User1?.FirstName} {conversation.User1?.LastName}".Trim(),
                User1Image = conversation.User1?.Image ?? string.Empty,
                User2Id = conversation.User2Id,
                User2Name = $"{conversation.User2?.FirstName} {conversation.User2?.LastName}".Trim(),
                User2Image = conversation.User2?.Image ?? string.Empty,
                CreatedAt = conversation.CreatedAt,
                LastMessageAt = conversation.LastMessageAt,
                LastMessage = lastMessage?.Content,
                UnreadCount = unreadCount,
                IsActive = conversation.IsActive
            };
        }
    }
}
