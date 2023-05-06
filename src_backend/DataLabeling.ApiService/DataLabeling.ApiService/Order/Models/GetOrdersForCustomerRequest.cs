using DataLabeling.Common.Order;

namespace DataLabeling.ApiService.Order.Models
{
    public class GetOrdersForCustomerRequest
    {
        public OrderType? OrderType { get; set; }

        public OrderPriority? MinPriority { get; set; }

        public OrderPriority? MaxPriority { get; set; }

        public bool? IsCompleted { get; set; }

        public bool? IsCanceled { get; set; }
    }
}
