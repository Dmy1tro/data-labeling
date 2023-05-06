using DataLabeling.Services.Interfaces.Order.Models;
using DataLabeling.Services.Interfaces.Payment.Models;

namespace DataLabeling.Services.Payment.Models
{
    public class OrderPaymentHistoryModel : OrderPaymentModel, IOrderPaymentHistoryModel
    {
        public IOrderModel Order { get; set; }
    }
}
