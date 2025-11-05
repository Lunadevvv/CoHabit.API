using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CoHabit.API.Helpers;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPayOSService _payOSService;
        private readonly ILogger<WebhookController> _logger;
        private readonly IAuthService _authService;
        private readonly ISubcriptionService _subcriptionService;
        private readonly IUserSubcriptionService _userSubcriptionService;
        public WebhookController(IPaymentService paymentService, IPayOSService payOSService, ILogger<WebhookController> logger, IAuthService authService, ISubcriptionService subcriptionService, IUserSubcriptionService userSubcriptionService)
        {
            _logger = logger;
            _paymentService = paymentService;
            _payOSService = payOSService;
            _authService = authService;
            _subcriptionService = subcriptionService;
            _userSubcriptionService = userSubcriptionService;
        }

        [HttpPost("payos")]
        public async Task<IActionResult> PayOSWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var doc = System.Text.Json.JsonDocument.Parse(body);
            var root = doc.RootElement;

            // Extract signature field from outer body
            if (!root.TryGetProperty("signature", out var signatureElem))
                return BadRequest("Missing signature");
            var signature = signatureElem.GetString();

            // Extract data element and compute HMAC over its canonical JSON
            if (!root.TryGetProperty("data", out var dataElem))
                return BadRequest("Missing data");

            var dataJson = dataElem.GetRawText(); // canonical JSON for data
            if (!_payOSService.VerifyWebhookSignature(dataJson, signature ?? string.Empty))
            {
                return Unauthorized();
            }
            _logger.LogInformation("Received valid PayOS webhook: {data}", dataJson);

            // Try to extract orderCode or paymentLinkId from data
            string? paymentLinkId = null;
            if (dataElem.TryGetProperty("paymentLinkId", out var linkIdElem))
            {
                paymentLinkId = linkIdElem.GetString();
                
            }
            if (string.IsNullOrEmpty(paymentLinkId)) return BadRequest("Missing payment identifier in data");

            // Find payment by paymentLinkId
            var payment = await _paymentService.GetPaymentByPaymentLinkId(paymentLinkId);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found for paymentLinkId: {PaymentLinkId}", paymentLinkId);
                return Ok(); // Return OK to acknowledge webhook
            }

            //Get Subcription from subcriptionId
            var subcription = await _subcriptionService.GetSubcriptionById(payment.SubcriptionId);
            if (subcription == null)
            {
                _logger.LogWarning("Subcription not found for subcriptionId: {SubcriptionId}", payment.SubcriptionId);
                return Ok(); // Return OK to acknowledge webhook
            }

            // Extract code and desc from webhook data
            string? code = null;
            string? desc = null;
            
            if (dataElem.TryGetProperty("code", out var codeElem))
            {
                code = codeElem.GetString();
            }
            if (dataElem.TryGetProperty("desc", out var descElem))
            {
                desc = descElem.GetString();
            }

            _logger.LogInformation("Processing payment {PaymentId} with code: {Code}, desc: {Desc}", 
                payment.PaymentId, code, desc);

            // Update payment status based on code
            // code = "00" means success
            if (code == "00")
            {
                payment.Status = PaymentStatus.Success;
                payment.UpdatedDate = DateTime.UtcNow;
                
                // Create user subscription
                var userSubscription = new UserSubcription
                {
                    UserId = payment.UserId,
                    SubcriptionId = payment.SubcriptionId,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(subcription.DurationInDays),
                    IsActive = true
                };

                await _userSubcriptionService.AddUserSubcription(userSubscription);
                // Update user's role
                await _authService.AssignRoleAsync(payment.UserId, subcription.Name);
                _logger.LogInformation("Payment {PaymentId} marked as Success and subscription created", payment.PaymentId);
            }
            else
            {
                // Any other code means failed
                payment.Status = PaymentStatus.Failed;
                payment.UpdatedDate = DateTime.UtcNow;
                _logger.LogWarning("Payment {PaymentId} marked as Failed with code: {Code}, desc: {Desc}", 
                    payment.PaymentId, code, desc);
            }

            await _paymentService.UpdatePaymentStatus(payment);
            
            return Ok();
        }
    }
}