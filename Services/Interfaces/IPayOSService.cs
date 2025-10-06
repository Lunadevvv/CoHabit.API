using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPayOSService
    {
        Task<CreatePaymentLinkResponse> CreatePaymentLinkAsync(CreatePaymentRequest request, int orderCode);

        bool VerifyWebhookSignature(string dataJson, string signature);
    }
}
