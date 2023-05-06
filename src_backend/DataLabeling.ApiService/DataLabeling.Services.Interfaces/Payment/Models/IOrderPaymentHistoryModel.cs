using DataLabeling.Services.Interfaces.Order.Models;

namespace DataLabeling.Services.Interfaces.Payment.Models
{
    public interface IOrderPaymentHistoryModel : IOrderPaymentModel
    {
        IOrderModel Order { get; }
    }
}
