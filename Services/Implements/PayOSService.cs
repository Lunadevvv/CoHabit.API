using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class PayOSService : IPayOSService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PayOSService> _logger;

        public PayOSService(IHttpClientFactory httpClientFactory, ILogger<PayOSService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<CreatePaymentLinkResponse> CreatePaymentLinkAsync(CreatePaymentRequest request, int orderCode)
        {
            var client = _httpClientFactory.CreateClient("payos");

            var cancelUrl = Environment.GetEnvironmentVariable("PayOS__CancelUrl") ?? string.Empty;
            var returnUrl = Environment.GetEnvironmentVariable("PayOS__ReturnUrl") ?? string.Empty;
            var checksumKey = Environment.GetEnvironmentVariable("PayOS__ChecksumKey") ?? string.Empty;
            var clientId = Environment.GetEnvironmentVariable("PayOS__ClientId");
            var apiKey = Environment.GetEnvironmentVariable("PayOS__ApiKey");

            var signatureData = $"amount={request.Amount}&cancelUrl={cancelUrl}&description={request.Description}&orderCode={orderCode}&returnUrl={returnUrl}";
            var signature = ComputeHmacSha256(checksumKey, signatureData);

            var payload = new
            {
                orderCode = orderCode,
                amount = request.Amount,
                description = request.Description,
                cancelUrl = cancelUrl,
                returnUrl = returnUrl,
                expiredAt = (int)DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds(),
                signature = signature
            };
            
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(clientId)) content.Headers.Add("x-client-id", clientId);
            if (!string.IsNullOrEmpty(apiKey)) content.Headers.Add("x-api-key", apiKey);

            var res = await client.PostAsync("v2/payment-requests", content);
            var body = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode) return new CreatePaymentLinkResponse();

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.TryGetProperty("data", out var data) && data.TryGetProperty("checkoutUrl", out var urlElem) && data.TryGetProperty("paymentLinkId", out var paymentLinkIdElem))
                {
                    return new CreatePaymentLinkResponse
                    {
                        PaymentLinkId = paymentLinkIdElem.GetString(),
                        CheckoutUrl = urlElem.GetString()
                    };
                }
            }
            catch { 
                return new CreatePaymentLinkResponse();
            }

            return new CreatePaymentLinkResponse();
        }

        public bool VerifyWebhookSignature(string dataJson, string signature)
        {
            var checksumKey = Environment.GetEnvironmentVariable("PayOS__ChecksumKey");
            if (string.IsNullOrEmpty(checksumKey))
            {
                _logger.LogWarning("PayOS checksum key is not configured, cannot verify webhook signature");
                return false;
            }
            if (string.IsNullOrEmpty(signature))
            {
                _logger.LogWarning("PayOS webhook signature is empty");
                return false;
            }
            
            try
            {
                // Parse the data JSON to extract fields
                using var doc = JsonDocument.Parse(dataJson);
                var root = doc.RootElement;
                
                // Convert to sorted dictionary
                var sortedData = new SortedDictionary<string, string>();
                
                foreach (var property in root.EnumerateObject())
                {
                    var key = property.Name;
                    var value = property.Value;
                    
                    string stringValue;
                    if (value.ValueKind == JsonValueKind.Null || value.ValueKind == JsonValueKind.Undefined)
                    {
                        stringValue = "";
                    }
                    else if (value.ValueKind == JsonValueKind.String)
                    {
                        stringValue = value.GetString() ?? "";
                    }
                    else if (value.ValueKind == JsonValueKind.Number)
                    {
                        stringValue = value.GetRawText();
                    }
                    else if (value.ValueKind == JsonValueKind.True || value.ValueKind == JsonValueKind.False)
                    {
                        stringValue = value.GetBoolean().ToString().ToLower();
                    }
                    else if (value.ValueKind == JsonValueKind.Array || value.ValueKind == JsonValueKind.Object)
                    {
                        stringValue = value.GetRawText();
                    }
                    else
                    {
                        stringValue = value.GetRawText();
                    }
                    
                    sortedData[key] = stringValue;
                }
                
                // Create signature string in format: key1=value1&key2=value2...
                var signatureData = string.Join("&", sortedData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                
                _logger.LogInformation("Webhook Signature Data: {SignatureData}", signatureData);
                _logger.LogInformation("Webhook Expected Signature: {Signature}", signature);
                
                var computed = ComputeHmacSha256(checksumKey, signatureData);
                _logger.LogInformation("Computed Signature: {Computed}", computed);
                
                return computed.Equals(signature, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying webhook signature");
                return false;
            }
        }

        private static string ComputeHmacSha256(string key, string data)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        // public ReturnURLQueryResponse GetPaymentInfo(IQueryCollection query)
        // {
        //     var res =
        //         new ReturnURLQueryResponse
        //         {
        //             PaymentLinkId = query["id"],
        //             Status = query["status"]
        //         };
        //     return res;
        // }
    }
}
