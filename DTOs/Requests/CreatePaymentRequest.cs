using System;

namespace CoHabit.API.DTOs.Requests
{
    public class CreatePaymentRequest
    {
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SubcriptionId { get; set; }
    }
}
