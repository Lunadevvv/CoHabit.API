using System.Security.Claims;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        private Guid GetCurrentUserId()
        {
            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");

            return userId;
        }

        //Create or get conversation for a specific post
        [HttpPost("conversation")]
        public async Task<ActionResult<ApiResponse<ConversationResponse>>> GetOrCreateConversation([FromQuery] Guid postId)
        {
            var userId = GetCurrentUserId();
            var conversation = await _chatService.CreateOrGetConversationAsync(userId, postId);

            return Ok(ApiResponse<ConversationResponse>.SuccessResponse(conversation, "Conversation retrieved successfully."));
        }

        //Get all conversations for the current user
        [HttpGet("conversations")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ConversationResponse>>>> GetUserConversations()
        {
            var userId = GetCurrentUserId();
            var conversations = await _chatService.GetUserConversationsAsync(userId);

            return Ok(ApiResponse<IEnumerable<ConversationResponse>>.SuccessResponse(conversations, "Conversations retrieved successfully."));
        }

        //Get messages for a specific conversation
        [HttpGet("conversation/{conversationId}/messages")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MessageResponse>>>> GetConversationMessages(Guid conversationId, [FromQuery] int page = 1)
        {
            var userId = GetCurrentUserId();
            var messages = await _chatService.GetConversationMessagesAsync(userId, conversationId, page);

            return Ok(ApiResponse<IEnumerable<MessageResponse>>.SuccessResponse(messages, "Messages retrieved successfully."));
        }

        //Mark messages as read in a conversation
        [HttpPost("conversation/{conversationId}/read")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkMessagesAsRead(Guid conversationId)
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.MarkMessagesAsReadAsync(userId, conversationId);

            if (result)
            {
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Messages marked as read successfully."));
            }
            else
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to mark messages as read."));
            }
        }

        //Send a message in a conversation
        [HttpPost("conversation/message")]
        public async Task<ActionResult<ApiResponse<MessageResponse>>> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = GetCurrentUserId();
            var message = await _chatService.SendMessageAsync(userId, request);

            return Ok(ApiResponse<MessageResponse>.SuccessResponse(message, "Message sent successfully."));
        }
    }
}