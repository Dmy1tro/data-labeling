using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Order.Models;
using System;
using System.Collections.Generic;

namespace DataLabeling.Services.Order.Models
{
    public class OrderModel : IOrderModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Requirements { get; set; }

        public int DatSetRequiredCount { get; set; }

        public int CurrentProgress { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCanceled { get; set; }

        public string ZipPath { get; set; }

        public decimal Price { get; set; }

        public OrderType Type { get; set; }

        public OrderPriority Priority { get; set; }

        public DateTime Deadline { get; set; }

        public ICollection<string> Variants { get; set; }

        public int CustomerId { get; set; }

        public int? OrderPaymentId { get; set; }
    }
}
