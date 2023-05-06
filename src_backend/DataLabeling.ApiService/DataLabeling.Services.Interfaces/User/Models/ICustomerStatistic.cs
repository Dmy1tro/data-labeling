namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface ICustomerStatistic
    {
        public int RawDataOrders { get; }

        public int LabelDataOrders { get; }

        public int TotalOrders => RawDataOrders + LabelDataOrders;
    }
}
