using DataLabeling.ApiService.Order.Models;
using DataLabeling.Services.Interfaces.Sallary.Models;
using DataLabeling.Services.Sallary.Models;

namespace DataLabeling.ApiService.Order.Mappers
{
    public static class PriceMappings
    {
        public static IOrderParametersModel MapToModel(this GetOrderPriceRequest request)
        {
            return new OrderParametersModel
            {
                DatSetRequiredCount = request.DatSetRequiredCount,
                Deadline = request.Deadline,
                Priority = request.Priority,
                Type = request.Type
            };
        }

        public static IOrderParametersModel MapToOrderParametersModel(this CreateOrderRequest request)
        {
            return new OrderParametersModel
            {
                DatSetRequiredCount = request.DatSetRequiredCount,
                Deadline = request.Deadline,
                Priority = request.Priority,
                Type = request.Type
            };
        }
    }
}
