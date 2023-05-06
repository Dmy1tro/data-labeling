using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Order.Models;

namespace DataLabeling.Services.Order.Models
{
    public class OrderFilterModel : IOrderFilterModel
    {
        public int? CustomerId { get; set; }

        public OrderType? OrderType { get; set; }

        public OrderPriority? MinPriority { get; set; }

        public OrderPriority? MaxPriority { get; set; }

        public bool? IsCompleted { get; set; }

        public bool? IsCanceled { get; set; }
    }
}
