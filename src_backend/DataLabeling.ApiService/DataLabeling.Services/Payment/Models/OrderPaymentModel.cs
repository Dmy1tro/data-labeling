using DataLabeling.Services.Interfaces.Payment.Models;
using System;

namespace DataLabeling.Services.Payment.Models
{
    public class OrderPaymentModel : IOrderPaymentModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
