using System;

namespace DataLabeling.Services.Interfaces.Payment.Models
{
    public interface IOrderPaymentModel
    {
        public int Id { get; }

        public int OrderId { get; }

        public int CustomerId { get; }

        public decimal Price { get; }

        public DateTime CreatedDate { get; }
    }
}
