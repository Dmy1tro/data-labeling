using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class CustomerStatistic : ICustomerStatistic
    {
        public int RawDataOrders { get; set; }

        public int LabelDataOrders { get; set; }
    }
}
