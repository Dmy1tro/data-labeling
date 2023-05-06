using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.ApiService.Data.Models
{
    public class GetDataSetRequest
    {
        public int OrderId { get; set; }

        public int? PerformerId { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
