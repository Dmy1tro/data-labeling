using DataLabeling.Common.Order;
using System;
using System.Collections.Generic;

namespace DataLabeling.Services.Interfaces.Order.Models
{
    public interface IOrderModel
    {
        public int Id { get; }

        public string Name { get; }

        public string Requirements { get; }

        public int DatSetRequiredCount { get; }

        public int CurrentProgress { get; }

        public bool IsCompleted { get; }

        public bool IsCanceled { get; }

        public string ZipPath { get; }

        public decimal Price { get; }

        public OrderType Type { get; }

        public OrderPriority Priority { get; }

        public DateTime Deadline { get; }

        public ICollection<string> Variants { get; set; }

        public int CustomerId { get; }

        public int? OrderPaymentId { get; }
    }
}
