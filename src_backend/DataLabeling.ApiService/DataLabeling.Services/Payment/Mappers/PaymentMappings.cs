using DataLabeling.Entities;
using DataLabeling.Services.Interfaces.Payment.Models;
using DataLabeling.Services.Order.Mappers;
using DataLabeling.Services.Payment.Models;

namespace DataLabeling.Services.Payment.Mappers
{
    public static class PaymentMappings
    {
        public static JobPayment MapToEntity(this IJobPaymentModel model)
        {
            return new JobPayment
            {
                Id = model.Id,
                PerformerId = model.PerformerId,
                BankCardNumber = model.BankCardNumber,
                Price = model.Price
            };
        }

        public static IJobPaymentModel MapToModel(this JobPayment entity)
        {
            return new JobPaymentModel
            {
                Id = entity.Id,
                PerformerId = entity.PerformerId,
                BankCardNumber = entity.BankCardNumber,
                Price = entity.Price,
                CreatedDate = entity.CreatedDate
            };
        }

        public static OrderPayment MapToEntity(this IOrderPaymentModel model)
        {
            return new OrderPayment
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                OrderId = model.OrderId,
                Price = model.Price
            };
        }

        public static IOrderPaymentModel MapToModel(this OrderPayment entity)
        {
            return new OrderPaymentModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                OrderId = entity.OrderId,
                Price = entity.Price,
                CreatedDate = entity.CreatedDate
            };
        }

        public static IOrderPaymentHistoryModel MapToHistoryModel(this OrderPayment entity)
        {
            return new OrderPaymentHistoryModel
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                OrderId = entity.OrderId,
                Price = entity.Price,
                CreatedDate = entity.CreatedDate,
                Order = entity.Order.MapToModel()
            };
        }
    }
}
