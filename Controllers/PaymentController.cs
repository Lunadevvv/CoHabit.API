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

namespace CoHabit.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;
    private readonly IOptions<PayOSConfig> _payOSConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Services.Interfaces.IPayOSService _payOSService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService, IOptions<PayOSConfig> payOSConfig, IHttpClientFactory httpClientFactory, Services.Interfaces.IPayOSService payOSService)
        {
            _payOSConfig = payOSConfig;
            _paymentService = paymentService;
            _httpClientFactory = httpClientFactory;
            _payOSService = payOSService;
            _logger = logger;
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

            var paymentId = _paymentService.GeneratePaymentId();

            var payment = new Payment
            {
                PaymentId = paymentId,
                Price = request.Amount,
                Description = request.Description,
                Status = PaymentStatus.InProgress,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                UserId = Guid.Parse(userIdStr)
            };

            await _paymentService.CreatePayment(payment, userIdStr);

            // Use PayOSService to create payment link
            var orderCode = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var checkoutUrl = await _payOSService.CreatePaymentLinkAsync(request, orderCode);

            return Ok(new CoHabit.API.DTOs.Responses.CreatePaymentResponse
            {
                PaymentId = paymentId,
                CheckoutUrl = checkoutUrl ?? string.Empty
            });
        }

        
    }
}