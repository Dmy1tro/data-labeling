using DataLabeling.Common.Order;
using DataLabeling.Common.Shared;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.Order.Models;
using FluentValidation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DataLabeling.Services.Validators
{
    public class OrderValidator : AbstractValidator<IOrderModel>
    {
        public OrderValidator()
        {
            RuleFor(o => o.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(o => o.Deadline).GreaterThan(DateTime.UtcNow).WithMessage("Deadline is invalid.");
            RuleFor(o => o.DatSetRequiredCount).GreaterThan(0).WithMessage("Required count should be greater than 0.");
        }

        public static void CheckIsCompleted(IOrderModel order)
        {
            Guard.ObjectFound(order, "Order");

            if (!order.IsCompleted)
            {
                throw new Common.Exceptions.ValidationException("Order not completed.");
            }
        }

        public static void CheckCustomerMatch(IOrderModel order, int customerId)
        {
            Guard.ObjectFound(order, "Order");
            Guard.PropertyNotEmpty(customerId, nameof(customerId));

            if (order.CustomerId != customerId)
            {
                throw new Common.Exceptions.ValidationException("Customer is mismatch.");
            }
        }

        public static void CheckIsPaid(IOrderModel order)
        {
            Guard.ObjectFound(order, "Order");

            if (!order.OrderPaymentId.HasValue)
            {
                throw new Common.Exceptions.ValidationException("Order is not paid.");
            }
        }

        public static void CheckIsNotPaid(IOrderModel order)
        {
            Guard.ObjectFound(order, "Order");

            if (order.OrderPaymentId.HasValue)
            {
                throw new Common.Exceptions.ValidationException("Order is already paid.");
            }
        }

        public static void CheckType(IOrderModel order, OrderType type)
        {
            Guard.ObjectFound(order, "Order");

            if (order.Type != type)
            {
                throw new Common.Exceptions.ValidationException("Order type is mismatch.");
            }
        }

        public static void CheckVariantExists(IOrderModel order, string variant)
        {
            Guard.ObjectFound(order, "Order");

            if (!order.Variants.Contains(variant))
            {
                throw new Common.Exceptions.ValidationException("Variant is invalid.");
            }
        }

        public static void CheckPerformerCanPerfomOrder(OrderPriority orderPriority, Rating performerRating)
        {
            if (!Gradation[orderPriority].Contains(performerRating))
            {
                throw new Common.Exceptions.ValidationException("Not enough rating to perform order.");
            }
        }

        public static readonly ConcurrentDictionary<OrderPriority, IEnumerable<Rating>> Gradation = new()
        {
            [OrderPriority.Low] = new[] { Rating.Awful, Rating.Acceptable, Rating.Normal, Rating.Good, Rating.Perfect },
            [OrderPriority.Medium] = new[] { Rating.Normal, Rating.Good, Rating.Perfect },
            [OrderPriority.High] = new[] { Rating.Good, Rating.Perfect }
        };
    }
}
