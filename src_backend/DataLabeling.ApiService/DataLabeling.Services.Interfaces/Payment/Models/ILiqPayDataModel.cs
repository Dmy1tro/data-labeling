namespace DataLabeling.Services.Interfaces.Payment.Models
{
    public interface ILiqPayDataModel
    {
        public string DataHash { get; }

        public string SignatureHash { get; }
    }
}
