using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoHabit.API.Services.Implements
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IPostRepository _postRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<User> _userManager;
        public ChatService(IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        IPostRepository postRepository,
        IOrderRepository orderRepository,
        UserManager<User> userManager)
        {
            _orderRepository = orderRepository;
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }
        public async Task<ConversationResponse> CreateOrGetConversationAsync(Guid userId, Guid PostId)
        {
            try
            {
                //get post
                var post = await _postRepository.GetPostByIdAsync(PostId);
                if (post == null)
                {
                    throw new Exception("Post not found");
                }

                //Cannot create conversation with yourself
                if (post.UserId == userId)
                {
                    throw new Exception("Cannot create conversation with yourself");
                }

                //check existing conversation
                var existingConversation = await _conversationRepository.GetByPostAndUsersAsync(PostId, post.UserId, userId);
                if (existingConversation != null)
                {
                    var unreadCount = await _messageRepository.GetUnreadCountAsync(existingConversation.Id, userId);
                    return MapToConversationResponse(existingConversation, unreadCount);
                }

                //create new conversation
                var newConversation = new Conversation()
                {
                    Id = Guid.NewGuid(),
                    PostId = PostId,
                    OwnerId = post.UserId,
                    InterestedUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                //Create Order
                var newOrder = new Order()
                {
                    OrderId = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    OwnerId = post.UserId,
                    UserId = userId,
                    PostId = PostId,
                    ConversationId = newConversation.Id
                };
                newConversation.OrderId = newOrder.OrderId;
                _conversationRepository.CreateAsync(newConversation);
                _orderRepository.CreateOrderAsync(newOrder);

                await _conversationRepository.SaveChangesAsync();

                return MapToConversationResponse(newConversation, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<MessageResponse>> GetConversationMessagesAsync(Guid userId, Guid conversationId, int page = 1)
        {
            try
            {
                //check if user is in conversation
                var isUserInConversation = _conversationRepository.IsUserInConversationAsync(conversationId, userId);
                if (!isUserInConversation.Result)
                {
                    throw new Exception("You are not in this conversation");
                }

                //get messages
                var messages = await _messageRepository.GetConversationMessagesAsync(conversationId, page);
                var messageResponses = messages.Select(m => new MessageResponse()
                {
                    MessageId = m.Id,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    SenderImage = string.Empty,
                    SenderName = $"{m.Sender?.FirstName} {m.Sender?.LastName}".Trim(),
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    IsRead = m.IsRead
                }).ToList();

                // Reverse to show oldest first
                messageResponses.Reverse();

                return messageResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ConversationResponse>> GetUserConversationsAsync(Guid userId)
        {
            try
            {
                var conversations = await _conversationRepository.GetAllConversationsByUserIdAsync(userId);
                var conversationResponses = new List<ConversationResponse>();

                foreach (var conversation in conversations)
                {
                    var unreadCount = await _messageRepository.GetUnreadCountAsync(conversation.Id, userId);
                    var conversationResponse = MapToConversationResponse(conversation, unreadCount);
                    conversationResponses.Add(conversationResponse);
                }

                return conversationResponses;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> MarkMessagesAsReadAsync(Guid userId, Guid conversationId)
        {
            try
            {
                //check if user is in conversation
                var isUserInConversation = await _conversationRepository.IsUserInConversationAsync(conversationId, userId);
                if (!isUserInConversation)
                {
                    throw new Exception("You are not in this conversation");
                }

                //get messages
                var messages =  await _messageRepository.GetConversationMessagesAsync(conversationId);

                //mark as read
                foreach (var message in messages)
                {
                    if (message.SenderId != userId && !message.IsRead)
                    {
                        await _messageRepository.MarkAsReadAsync(message.Id);
                    }
                }
                await _messageRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<MessageResponse> SendMessageAsync(Guid userId, SendMessageRequest request)
        {
            try
            {
                //check if user is in conversation
                var isUserInConversation =  await _conversationRepository.IsUserInConversationAsync(request.ConversationId, userId);
                if (!isUserInConversation)
                {
                    throw new Exception("You are not in this conversation");
                }

                //get user
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                //create message
                var newMessage = new Message()
                {
                    ConversationId = request.ConversationId,
                    SenderId = userId,
                    Content = request.Content,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                await _messageRepository.CreateAsync(newMessage);
                await _messageRepository.SaveChangesAsync();

                var messageResponse = new MessageResponse()
                {
                    MessageId = newMessage.Id,
                    ConversationId = newMessage.ConversationId,
                    SenderId = newMessage.SenderId,
                    SenderImage = string.Empty,
                    SenderName = $"{user.FirstName} {user.LastName}".Trim(),
                    Content = newMessage.Content,
                    CreatedAt = newMessage.CreatedAt,
                    IsRead = newMessage.IsRead
                };

                return messageResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private ConversationResponse MapToConversationResponse(Conversation conversation, int unreadCount)
        {
            return new ConversationResponse()
            {
                ConversationId = conversation.Id,
                PostId = conversation.PostId,
                PostAddress = conversation.Post!.Address,
                PostPrice = conversation.Post!.Price,
                PostRating = conversation.Post!.AverageRating,
                PostTitle = conversation.Post!.Title,
                OwnerId = conversation.OwnerId,
                OwnerName = $"{conversation.Owner?.FirstName} {conversation.Owner?.LastName}".Trim(),
                OwnerImage = string.Empty,
                InterestedUserId = conversation.InterestedUserId,
                InterestedUserName = $"{conversation.InterestedUser?.FirstName} {conversation.InterestedUser?.LastName}".Trim(),
                InterestedUserImage = string.Empty,
                LastMessage = conversation.Messages != null && conversation.Messages.Count > 0 ? conversation.Messages.OrderByDescending(m => m.CreatedAt).First().Content : null,
                LastMessageAt = conversation.LastMessageAt,
                UnreadCount = unreadCount
            };
        }
    }
}