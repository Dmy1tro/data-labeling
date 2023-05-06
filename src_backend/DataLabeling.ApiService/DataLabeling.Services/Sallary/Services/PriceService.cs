using DataLabeling.Common.Order;
using DataLabeling.Common.Sallary;
using DataLabeling.Common.Shared;
using DataLabeling.Data.Context;
using DataLabeling.Infrastructure.Date;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.Sallary.Models;
using DataLabeling.Services.Interfaces.Sallary.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.Services.Sallary.Services
{
    public class PriceService : IPriceService
    {
        private readonly DateLabelingDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PriceService(DateLabelingDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public decimal CalculateOrderPriceAsync(IOrderParametersModel orderModel)
        {
            var datasetCountPrice = 0.001m * orderModel.DatSetRequiredCount;
            var typeCoff = GetBaseCoff(orderModel.Type);
            var priorityCoff = GetPriorityCoff(orderModel.Priority);
            var deadlinePrice = 300m / (orderModel.Deadline - _dateTimeProvider.UtcNow).Duration().Days;

            var total = deadlinePrice + datasetCountPrice * typeCoff * priorityCoff;

            return Math.Round(total, 2);
        }

        public async Task<decimal> CalculateSallaryAsync(int performerId, OrderType orderType)
        {
            var performer = await _context.Performers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == performerId);
            Guard.ObjectFound(performer);

            var completedJobsCount = await _context.DataSet
                .Where(d => d.PerformerId == performerId)
                .CountAsync();

            var basePrice = GetBasePrice(orderType);
            var coff = GetCoefficient(performer.Rating);

            var price = coff * (basePrice +  0.0001m * completedJobsCount);

            return price;
        }

        private decimal GetBasePrice(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.CollectData => SallaryPrice.UploadRawDataPrice,
                OrderType.LabelData => SallaryPrice.LabelDataPrice,
                _ => throw new NotSupportedException(nameof(orderType))
            };
        }

        private decimal GetBaseCoff(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.CollectData => 1.3m,
                OrderType.LabelData => 1.6m,
                _ => throw new NotSupportedException(nameof(orderType))
            };
        }

        private decimal GetPriorityCoff(OrderPriority orderPriority)
        {
            return orderPriority switch
            {
                OrderPriority.Low => 1.3m,
                OrderPriority.Medium => 1.6m,
                OrderPriority.High => 1.9m,
                _ => throw new NotSupportedException(nameof(orderPriority))
            };
        }

        private decimal GetCoefficient(Rating rating)
        {
            return rating switch
            {
                Rating.Awful => 1m, // 0%
                Rating.Acceptable => 1.001m, // 1%
                Rating.Normal => 1.05m, // 5%
                Rating.Good => 1.15m, // 15%
                Rating.Perfect => 1.2m, // 20%
                _ => throw new NotSupportedException(nameof(rating))
            };
        }
    }
}
