using DataLabeling.ApiService.Order.Models;
using DataLabeling.Common.Order;
using DataLabeling.Services.Interfaces.Order.Models;
using DataLabeling.Services.Order.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataLabeling.ApiService.Order.Mappers
{
    public static class OrderMappings
    {
        public static IOrderModel MapToModel(this CreateOrderRequest request, int customerId, decimal price)
        {
            return new OrderModel
            {
                CustomerId = customerId,
                DatSetRequiredCount = request.DatSetRequiredCount,
                CurrentProgress = 0,
                Deadline = request.Deadline,
                Requirements = request.Requirements,
                IsCompleted = false,
                Name = request.Name,
                Price = price,
                Priority = request.Priority,
                Type = request.Type,
                IsCanceled = false,
                Variants = request.Variants
            };
        }

        public static IOrderFilterModel MapToModel(this GetOrdersForCustomerRequest request, int customerId)
        {
            return new OrderFilterModel
            {
                CustomerId = customerId,
                IsCompleted = request.IsCompleted,
                MaxPriority = request.MaxPriority,
                MinPriority = request.MinPriority,
                OrderType = request.OrderType,
                IsCanceled = request.IsCanceled
            };
        }

        public static IOrderFilterModel MapToModel(this GetOrdersForPerformerRequest request, IEnumerable<OrderPriority> priorities)
        {
            var filter = new OrderFilterModel
            {
                IsCompleted = false,
                IsCanceled = false,
                OrderType = request.OrderType
            };

            var orderedPriorities = priorities.OrderBy(p => (int)p).ToList();

            filter.MinPriority = orderedPriorities.First();
            filter.MaxPriority = orderedPriorities.Last();

            return filter;
        }
    }
}
