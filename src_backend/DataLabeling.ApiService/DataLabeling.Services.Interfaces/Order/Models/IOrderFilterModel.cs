using DataLabeling.Common.Order;

namespace DataLabeling.Services.Interfaces.Order.Models
{
    public interface IOrderFilterModel
    {
        public int? CustomerId { get; }

        public OrderType? OrderType { get; }

        public OrderPriority? MinPriority { get; }

        public OrderPriority? MaxPriority { get; }

        public bool? IsCompleted { get; }

        public bool? IsCanceled { get; }
    }
}
