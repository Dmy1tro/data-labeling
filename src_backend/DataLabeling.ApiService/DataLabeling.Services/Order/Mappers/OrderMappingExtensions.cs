using DataLabeling.Services.Interfaces.Order.Models;
using DataLabeling.Services.Order.Models;
using System;

namespace DataLabeling.Services.Order.Mappers
{
    public static class OrderMappingExtensions
    {
        public static Entities.Order MapToEntity(this IOrderModel orderModel)
        {
            return new Entities.Order
            {
                CustomerId = orderModel.CustomerId,
                DatSetRequiredCount = orderModel.DatSetRequiredCount,
                CurrentProgress = orderModel.CurrentProgress,
                Deadline = orderModel.Deadline,
                Requirements = orderModel.Requirements,
                Id = orderModel.Id,
                IsCompleted = orderModel.IsCompleted,
                Name = orderModel.Name,
                ZipPath = orderModel.ZipPath,
                OrderPaymentId = orderModel.OrderPaymentId,
                Price = orderModel.Price,
                Priority = orderModel.Priority,
                Type = orderModel.Type,
                IsCanceled = orderModel.IsCanceled,
                Variants = string.Join("|", orderModel.Variants ?? Array.Empty<string>())
            };
        }

        public static IOrderModel MapToModel(this Entities.Order order)
        {
            if (order is null) return null;

            return new OrderModel
            {
                CustomerId = order.CustomerId,
                DatSetRequiredCount = order.DatSetRequiredCount,
                CurrentProgress = order.CurrentProgress,
                Deadline = order.Deadline,
                Requirements = order.Requirements,
                Id = order.Id,
                IsCompleted = order.IsCompleted,
                Name = order.Name,
                OrderPaymentId = order.OrderPaymentId,
                Price = order.Price,
                ZipPath = order.ZipPath,
                Priority = order.Priority,
                Type = order.Type,
                IsCanceled = order.IsCanceled,
                Variants = string.IsNullOrEmpty(order.Variants) 
                    ? Array.Empty<string>()
                    : order.Variants.Split("|")
            };
        }
    }
}
