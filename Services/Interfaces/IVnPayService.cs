using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(VnPayRequest model);
        VnPayResponse GetPaymentResult(IQueryCollection collections);
    }
}