using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoHabit.API.Services.Interfaces;
using CoHabit.API.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersByUserId()
        {
            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(ApiResponse<List<OrderResponse>>.SuccessResponse(result, "Orders retrieved successfully."));
        }

        [HttpGet("owner")]
        [Authorize]
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersByOwnerId()
        {
            var ownerId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
            var result = await _orderService.GetOrdersByOwnerIdAsync(ownerId);
            return Ok(ApiResponse<List<OrderResponse>>.SuccessResponse(result, "Orders retrieved successfully."));
        }

        // [HttpGet("{postId}")]
        // [Authorize(Roles = "Admin,Moderator,ProMember")]
        // public async Task<IActionResult> GetOrdersByPostId(Guid postId)
        // {
        //     var result = await _orderService.GetOrdersByPostIdAsync(postId);
        //     return Ok(result);
        // }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateOrder([FromQuery] Guid postId)
        {
            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");

            var result = await _orderService.CreateOrderAsync(userId, postId);
            
            if (result <= 0)
            {
                return BadRequest("Failed to create order.");
            }

            return Ok(ApiResponse<string>.SuccessResponse(string.Empty, "Order created successfully."));
        }

    }
}