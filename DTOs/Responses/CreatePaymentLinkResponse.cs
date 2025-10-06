using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class CreatePaymentLinkResponse
    {
        public string? PaymentLinkId { get; set; }
        public string? CheckoutUrl { get; set; }
    }
}