using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<PayOSConfig> _payosConfig;
    private readonly Services.Interfaces.IPayOSService _payOSService;

        public WebhookController(IPaymentService paymentService, IOptions<PayOSConfig> payosConfig, Services.Interfaces.IPayOSService payOSService)
        {
            _paymentService = paymentService;
            _payosConfig = payosConfig;
            _payOSService = payOSService;
        }

        [HttpPost("payos")]
        public async Task<IActionResult> PayOSWebhook()
        {
            using var reader = new System.IO.StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var doc = System.Text.Json.JsonDocument.Parse(body);
            var root = doc.RootElement;

            // Extract signature field from outer body
            if (!root.TryGetProperty("signature", out var signatureElem))
                return BadRequest("Missing signature");
            var signature = signatureElem.GetString();
            var checksumKey = _payosConfig.Value.ChecksumKey ?? string.Empty;

            // Extract data element and compute HMAC over its canonical JSON
            if (!root.TryGetProperty("data", out var dataElem))
                return BadRequest("Missing data");

            var dataJson = dataElem.GetRawText(); // canonical JSON for data
            if (!_payOSService.VerifyWebhookSignature(dataJson, signature ?? string.Empty))
            {
                return Unauthorized();
            }

            // Try to extract orderCode or paymentLinkId from data
            string? paymentId = null;
            if (dataElem.TryGetProperty("paymentLinkId", out var linkIdElem))
            {
                paymentId = linkIdElem.GetString();
            }
            else if (dataElem.TryGetProperty("orderCode", out var orderCodeElem))
            {
                // orderCode is an integer - we'll use its string representation to match our paymentId if used
                paymentId = orderCodeElem.GetRawText();
            }
            if (string.IsNullOrEmpty(paymentId)) return BadRequest("Missing payment identifier in data");

            PaymentStatus newStatus = PaymentStatus.InProgress;
            if (dataElem.TryGetProperty("status", out var statusElem))
            {
                var s = statusElem.GetString();
                if (!string.IsNullOrEmpty(s))
                {
                    if (s.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase)) newStatus = PaymentStatus.Success;
                    else if (s.Equals("FAILED", StringComparison.OrdinalIgnoreCase)) newStatus = PaymentStatus.Failed;
                    else if (s.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase)) newStatus = PaymentStatus.Cancelled;
                    else newStatus = PaymentStatus.InProgress;
                }
            }

            var payment = await _paymentService.GetPayment(paymentId);
            if (payment == null) return NotFound();
            payment.Status = newStatus;
            await _paymentService.UpdatePaymentStatus(payment);

            return Ok();
        }

        private static bool ValidateHmacSha256(string key, string data, string? signatureHeader)
        {
            if (string.IsNullOrEmpty(key)) return true; // can't validate
            if (string.IsNullOrEmpty(signatureHeader)) return false;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            var computed = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            return computed == signatureHeader.ToLowerInvariant();
        }
    }
}