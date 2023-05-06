using DataLabeling.Common.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLabeling.Entities
{
    public class Order : AuditEntity
    {
        public int Id { get; set; }

        [Required]
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

        public string Variants { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer{ get; set; }

        public int? OrderPaymentId { get; set; }
        public OrderPayment OrderPayment { get; set; }

        public ICollection<Data> DataSet { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
