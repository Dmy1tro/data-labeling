using DataLabeling.ApiService.Data.Models;
using DataLabeling.Services.Data.Models;
using DataLabeling.Services.Interfaces.Data.Models;

namespace DataLabeling.ApiService.Data.Mappers
{
    public static class DataMapper
    {
        public static IDataFilterModel MapToFilterModel(this GetDataSetRequest request, int customerId)
        {
            return new DataFilterModel
            {
                CustomerId = customerId,
                OrderId = request.OrderId,
                PerformerId = request.PerformerId,
                Skip = request.Skip,
                Take = request.Take
            };
        }
    }
}
