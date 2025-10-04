using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CoHabit.API.Services.Implements
{
    public class PayOSService : IPayOSService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PayOSConfig _config;

        public PayOSService(IHttpClientFactory httpClientFactory, IOptions<PayOSConfig> options)
        {
            _httpClientFactory = httpClientFactory;
            _config = options.Value;
        }

        public async Task<string?> CreatePaymentLinkAsync(CreatePaymentRequest request, int orderCode)
        {
            var client = _httpClientFactory.CreateClient("payos");

            var signatureData = $"amount={request.Amount}&cancelUrl={request.CancelUrl}&description={request.Description}&orderCode={orderCode}&returnUrl={request.ReturnUrl}";
            var signature = ComputeHmacSha256(_config.ChecksumKey ?? string.Empty, signatureData);

            var payload = new
            {
                orderCode = orderCode,
                amount = request.Amount,
                description = request.Description,
                cancelUrl = request.CancelUrl,
                returnUrl = request.ReturnUrl,
                signature = signature
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(_config.ClientId)) content.Headers.Add("x-client-id", _config.ClientId);
            if (!string.IsNullOrEmpty(_config.ApiKey)) content.Headers.Add("x-api-key", _config.ApiKey);

            var res = await client.PostAsync("v2/payment-requests", content);
            var body = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode) return null;

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.TryGetProperty("data", out var data) && data.TryGetProperty("checkoutUrl", out var urlElem))
                {
                    return urlElem.GetString();
                }
            }
            catch { /* swallow */ }

            return null;
        }

        public bool VerifyWebhookSignature(string dataJson, string signature)
        {
            if (string.IsNullOrEmpty(_config.ChecksumKey)) return true;
            if (string.IsNullOrEmpty(signature)) return false;
            var computed = ComputeHmacSha256(_config.ChecksumKey, dataJson);
            return string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeHmacSha256(string key, string data)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
