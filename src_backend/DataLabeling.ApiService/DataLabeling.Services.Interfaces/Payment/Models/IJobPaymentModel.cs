using System;

namespace DataLabeling.Services.Interfaces.Payment.Models
{
    public interface IJobPaymentModel
    {
        public int Id { get; }

        public string BankCardNumber { get; }

        public int PerformerId { get; }

        public decimal Price { get; }

        public DateTime CreatedDate { get; }
    }
}
