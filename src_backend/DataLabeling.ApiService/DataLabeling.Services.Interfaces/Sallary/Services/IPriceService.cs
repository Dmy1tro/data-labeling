using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Sallary.Models;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.Sallary.Services
{
    public interface IPriceService
    {
        public Task<decimal> CalculateSallaryAsync(int performerId, OrderType orderType);

        public decimal CalculateOrderPriceAsync(IOrderParametersModel orderParametersModel);
    }
}
