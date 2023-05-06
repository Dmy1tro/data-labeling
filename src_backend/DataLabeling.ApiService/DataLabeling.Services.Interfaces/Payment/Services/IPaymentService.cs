using DataLabeling.Services.Interfaces.Payment.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.Payment.Services
{
    public interface IPaymentService
    {
        Task AddPayment(IOrderPaymentModel orderPaymentModel);

        Task AddPayment(IJobPaymentModel jobPaymentModel);

        Task<ICollection<IOrderPaymentHistoryModel>> GetOrderPaymentHistory(int customerId);

        Task<ICollection<IJobPaymentModel>> GetJobPaymentHistory(int performerId);

        Task<ILiqPayDataModel> GetLiqPayData(decimal price, string postDataUrl);

        Task<bool> VerifyLiqPaySignature(string data, string liqpaySignature);
    }
}
