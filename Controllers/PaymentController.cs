using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Services.Implements;

namespace CoHabit.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IPayOSService _payOSService;
        private readonly IAuthService _authService;
        private readonly ISubcriptionService _subcriptionService;
        private readonly IUserSubcriptionService _userSubcriptionService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService, IPayOSService payOSService, IAuthService authService, ISubcriptionService subcriptionService, IUserSubcriptionService userSubcriptionService)
        {
            _paymentService = paymentService;
            _payOSService = payOSService;
            _logger = logger;
            _authService = authService;
            _subcriptionService = subcriptionService;
            _userSubcriptionService = userSubcriptionService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Payment>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayment();
            return Ok(payments);
        }

        [HttpGet("allUserPayment/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<Payment>>> GetAllUserPayments(string userId)
        {
            var payments = await _paymentService.GetAllUserPayment(userId);
            return Ok(payments);
        }

        [HttpGet("{paymentId}")]
        [Authorize]
        public async Task<ActionResult<Payment>> GetPayment(string paymentId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user");
            }
            var payment = await _paymentService.GetPayment(paymentId);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            // get current user id from token
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return BadRequest("Invalid user");
            //Parse userId to Guid
            var userId = Guid.TryParse(userIdStr, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Parse user ID failed");

            //Check if subcription exists
            var userSubcription = await _userSubcriptionService.GetActiveSubcriptionByUserIdAndSubId(userId, request.SubcriptionId);
            if (userSubcription != null)
            {
                return BadRequest("You already have an active subscription.");
            }

            var paymentId = _paymentService.GeneratePaymentId();

            // Use PayOSService to create payment link
            var orderCode = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var createPaymentLinkResponse = await _payOSService.CreatePaymentLinkAsync(request, orderCode);

            var payment = new Payment
            {
                PaymentId = paymentId,
                PaymentLinkId = createPaymentLinkResponse.PaymentLinkId ?? string.Empty,
                Price = request.Amount,
                Description = request.Description,
                Status = PaymentStatus.InProgress,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                UserId = userId,
                SubcriptionId = request.SubcriptionId
            };

            await _paymentService.CreatePayment(payment, userIdStr);

            return Ok(new CreatePaymentResponse
            {
                PaymentId = paymentId,
                CheckoutUrl = createPaymentLinkResponse.CheckoutUrl ?? string.Empty
            });
        }

        // [HttpPatch("update-status")]
        // public async Task<IActionResult> UpdatePaymentStatus()
        // {
        //     if (Request.QueryString.HasValue)
        //     {
        //         //Get payment info from query string
        //         var paymentResult = _payOSService.GetPaymentInfo(Request.Query);
        //         if (paymentResult == null || string.IsNullOrEmpty(paymentResult.PaymentLinkId))
        //         {
        //             return BadRequest("Invalid payment information");
        //         }

        //         //Find payment by paymentLinkId
        //         var payment = await _paymentService.GetPaymentByPaymentLinkId(paymentResult.PaymentLinkId);
        //         if (payment == null)
        //         {
        //             return NotFound("Payment not found");
        //         }

        //         //Get Subcription from subcriptionId
        //         var subcription = await _subcriptionService.GetSubcriptionById(payment.SubcriptionId);
        //         if (subcription == null)
        //         {
        //             return NotFound("Subcription not found");
        //         }
                
        //         // Update payment status based on the status from query
        //         if (paymentResult.Status == "PAID")
        //         {
        //             payment.Status = PaymentStatus.Success;

        //             // Create UserSubcription
        //             var userSubcription = new UserSubcription
        //             {
        //                 UserId = payment.UserId,
        //                 SubcriptionId = subcription.SubcriptionId,
        //                 StartDate = DateTime.Now,
        //                 EndDate = DateTime.Now.AddDays(subcription.DurationInDays),
        //                 IsActive = true
        //             };
        //             await _userSubcriptionService.AddUserSubcription(userSubcription);

        //             // Update user's role
        //             await _authService.AssignRoleAsync(payment.UserId, subcription.Name);
        //         }
        //         else if (paymentResult.Status == "CANCELLED")
        //         {
        //             payment.Status = PaymentStatus.Cancelled;
        //         }
        //         else
        //         {
        //             payment.Status = PaymentStatus.InProgress;
        //         }

        //         await _paymentService.UpdatePaymentStatus(payment);
        //         return Ok("Payment status updated");
        //     }
        //     return BadRequest("Missing query string");
        // }
    }
}