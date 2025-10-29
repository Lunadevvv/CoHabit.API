using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        /// <summary>
        /// Create or get existing conversation for a specific post
        /// </summary>
        [HttpPost("conversations")]
        public async Task<ActionResult<ApiResponse<ConversationResponse>>> CreateOrGetConversation([FromBody] CreateConversationRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.CreateOrGetConversationAsync(userId, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all conversations for the current user
        /// </summary>
        [HttpGet("conversations")]
        public async Task<ActionResult<ApiResponse<ConversationResponse>>> GetUserConversations()
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.GetUserConversationsAsync(userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get messages for a specific conversation
        /// </summary>
        [HttpGet("conversations/{conversationId}/messages")]
        public async Task<ActionResult<ApiResponse<MessageResponse>>> GetConversationMessages(
            Guid conversationId,
            [FromQuery] int page = 1)
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.GetConversationMessagesAsync(userId, conversationId, page);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Send a message (alternative to SignalR)
        /// </summary>
        [HttpPost("messages")]
        public async Task<ActionResult<ApiResponse<MessageResponse>>> SendMessage([FromBody] SendMessageRequest request)
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.SendMessageAsync(userId, request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Mark all messages in a conversation as read
        /// </summary>
        [HttpPost("conversations/{conversationId}/read")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkMessagesAsRead(Guid conversationId)
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.MarkMessagesAsReadAsync(userId, conversationId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
