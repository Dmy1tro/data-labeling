using DataLabeling.Services.Interfaces.Order.Models;
using System.Linq;

namespace DataLabeling.Services.Order.Extensions
{
    public static class OrderQueryExtensions
    {
        public static IQueryable<Entities.Order> ApplyFilter(this IQueryable<Entities.Order> query, 
                                                             IOrderFilterModel filterModel)
        {
            if (filterModel.CustomerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == filterModel.CustomerId);
            }

            if (filterModel.IsCompleted.HasValue)
            {
                query = query.Where(o => o.IsCompleted == filterModel.IsCompleted);
            }

            if (filterModel.IsCanceled.HasValue)
            {
                query = query.Where(o => o.IsCanceled == filterModel.IsCanceled);
            }

            if (filterModel.MinPriority.HasValue)
            {
                query = query.Where(o => o.Priority >= filterModel.MinPriority);
            }

            if (filterModel.MaxPriority.HasValue)
            {
                query = query.Where(o => o.Priority <= filterModel.MaxPriority);
            }

            if (filterModel.OrderType.HasValue)
            {
                query = query.Where(o => o.Type == filterModel.OrderType);
            }

            return query;
        }
    }
}
