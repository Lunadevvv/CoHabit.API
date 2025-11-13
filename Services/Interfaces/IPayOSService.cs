using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPayOSService
    {
        Task<CreatePaymentLinkResponse> CreatePaymentLinkAsync(CreatePaymentRequest request, int orderCode);
        // ReturnURLQueryResponse GetPaymentInfo(IQueryCollection query);
        bool VerifyWebhookSignature(string dataJson, string signature);
    }
}
