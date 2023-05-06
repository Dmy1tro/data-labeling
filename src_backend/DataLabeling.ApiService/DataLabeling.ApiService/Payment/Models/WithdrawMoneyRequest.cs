using System.ComponentModel.DataAnnotations;

namespace DataLabeling.ApiService.Payment.Models
{
    public class WithdrawMoneyRequest
    {
        [Required]
        public string BankCardNumber { get; set; }

        public decimal Price { get; set; }
    }
}
