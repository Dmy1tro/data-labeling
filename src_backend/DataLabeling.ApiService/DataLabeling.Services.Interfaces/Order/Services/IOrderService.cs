using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Order.Models;
using DataLabeling.Services.Interfaces.User.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.Order.Services
{
    public interface IOrderService
    {
        Task<IOrderModel> CreateAsync(IOrderModel orderModel);

        Task RefreshProgressAsync(int orderId);

        Task<double> GetProgressAsync(int orderId);

        Task<IOrderModel> GetOrderAsync(int orderId);

        Task<IOrderModel> GetOrderForCustomerAsync(int orderId, int customerId);

        Task<IOrderModel> GetOrderForPerformerAsync(int orderId, int performerId);

        Task<ICollection<IOrderModel>> GetOrdersAsync(IOrderFilterModel filter);

        Task<ICollection<OrderPriority>> GetAllowedPriorities(int performerId);

        Task CancelAsync(int orderId, int customerId);

        Task<ICollection<IUserModel>> GetPerformersForReview(int orderId);
    }
}
