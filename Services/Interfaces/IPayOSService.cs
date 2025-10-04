using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPayOSService
    {
        /// <summary>
        /// Create a payment link on PayOS and return the checkoutUrl (or null if not present)
        /// </summary>
        Task<string?> CreatePaymentLinkAsync(CreatePaymentRequest request, int orderCode);

        /// <summary>
        /// Verify the webhook signature. dataJson should be the canonical JSON of the `data` element from webhook.
        /// </summary>
        bool VerifyWebhookSignature(string dataJson, string signature);
    }
}
