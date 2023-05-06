using DataLabeling.Services.Interfaces.Payment.Models;
using System;

namespace DataLabeling.Services.Payment.Models
{
    public class JobPaymentModel : IJobPaymentModel
    {
        public int Id { get; set; }

        public int PerformerId { get; set; }

        public string BankCardNumber { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
