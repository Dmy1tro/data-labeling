using DataLabeling.Services.Interfaces.Payment.Models;

namespace DataLabeling.Services.Payment.Models
{
    public class LiqPayDataModel : ILiqPayDataModel
    {
        public string DataHash { get; set; }

        public string SignatureHash { get; set; }
    }
}
