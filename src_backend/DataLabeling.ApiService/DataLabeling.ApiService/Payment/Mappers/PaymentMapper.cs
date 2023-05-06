using DataLabeling.ApiService.Payment.Models;
using DataLabeling.Services.Interfaces.Payment.Models;
using DataLabeling.Services.Payment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.Payment.Mappers
{
    public static class PaymentMapper
    {
        public static IJobPaymentModel MapToModel(this WithdrawMoneyRequest request, int performerId)
        {
            return new JobPaymentModel
            {
                PerformerId = performerId,
                BankCardNumber = request.BankCardNumber,
                Price = request.Price
            };
        }
    }
}
