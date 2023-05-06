using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.Services.Data.Models
{
    public class DataFilterModel : IDataFilterModel
    {
        public int OrderId { get; set; }

        public int? PerformerId { get; set; }

        public int? CustomerId { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
