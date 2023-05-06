using DataLabeling.Common.Order;
using System;

namespace DataLabeling.ApiService.Order.Models
{
    public class GetOrderPriceRequest
    {
        public int DatSetRequiredCount { get; set; }

        public OrderType Type { get; set; }

        public OrderPriority Priority { get; set; }

        public DateTime Deadline { get; set; }
    }
}
