using System.Text.Json;

namespace CoHabit.API.DTOs.Responses
{
    public class CreatePaymentResponse
    {
        public string PaymentId { get; set; } = string.Empty;
        public string CheckoutUrl { get; set; } = string.Empty;
    }
}
