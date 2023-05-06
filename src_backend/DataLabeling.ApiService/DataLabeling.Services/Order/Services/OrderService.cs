using DataLabeling.Common.Order;
using DataLabeling.Data.Context;
using DataLabeling.Infrastructure.Extensions;
using DataLabeling.Infrastructure.Guard;
using DataLabeling.Services.Interfaces.Order.Models;
using DataLabeling.Services.Interfaces.Order.Services;
using DataLabeling.Services.Interfaces.User.Models;
using DataLabeling.Services.Order.Extensions;
using DataLabeling.Services.Order.Mappers;
using DataLabeling.Services.User.Mappers;
using DataLabeling.Services.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.Services.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly DateLabelingDbContext _context;

        public OrderService(DateLabelingDbContext context)
        {
            _context = context;
        }

        public async Task CancelAsync(int orderId, int customerId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            Guard.ObjectFound(order);

            OrderValidator.CheckCustomerMatch(order.MapToModel(), customerId);

            order.IsCanceled = true;

            await _context.SaveChangesAsync();
        }

        public async Task<IOrderModel> CreateAsync(IOrderModel orderModel)
        {
            new OrderValidator().ValidateAndThrowException(orderModel);

            var mapped = orderModel.MapToEntity();
            var result = _context.Orders.Add(mapped);

            await _context.SaveChangesAsync();

            var created = result.Entity.MapToModel();

            return created;
        }

        public async Task<ICollection<OrderPriority>> GetAllowedPriorities(int performerId)
        {
            var performer = await _context.Performers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == performerId);
            Guard.ObjectFound(performer);

            var allowedPriorities = OrderValidator.Gradation
                .Where(g => g.Value.Contains(performer.Rating))
                .Select(g => g.Key)
                .ToList();

            return allowedPriorities;
        }

        public async Task<IOrderModel> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);

            return order.MapToModel();
        }

        public async Task<IOrderModel> GetOrderForCustomerAsync(int orderId, int customerId)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);
            Guard.ObjectFound(order);

            var model = order.MapToModel();
            OrderValidator.CheckCustomerMatch(model, customerId);

            return model;
        }

        public async Task<IOrderModel> GetOrderForPerformerAsync(int orderId, int performerId)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);
            Guard.ObjectFound(order);

            var performer = await _context.Performers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == performerId);
            Guard.ObjectFound(performer);

            var model = order.MapToModel();
            OrderValidator.CheckPerformerCanPerfomOrder(model.Priority, performer.Rating);

            return model;
        }

        public async Task<ICollection<IOrderModel>> GetOrdersAsync(IOrderFilterModel filter)
        {
            var result = await _context.Orders.AsNoTracking()
                .ApplyFilter(filter)
                .ToListAsync();

            var orders = result.Select(r => r.MapToModel()).ToList();

            return orders;
        }

        public async Task<ICollection<IUserModel>> GetPerformersForReview(int orderId)
        {
            var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == orderId);

            OrderValidator.CheckIsCompleted(order?.MapToModel());

            var performersId = await _context.DataSet.AsNoTracking()
                                                     .Where(d => d.OrderId == orderId)
                                                     .Select(d => d.PerformerId.Value)
                                                     .Distinct()
                                                     .ToListAsync();

            var reviewed = await _context.Reviews.AsNoTracking()
                .Where(r => r.OrderId == orderId && performersId.Contains(r.PerformerId))
                .Select(r => r.PerformerId)
                .ToListAsync();

            performersId = performersId.Except(reviewed).ToList();

            var performers = await _context.Performers.Where(p => performersId.Contains(p.Id)).ToListAsync();

            return performers.Select(p => p.MapToModel()).ToList();
        }

        public async Task<double> GetProgressAsync(int orderId)
        {
            var order = await _context.Orders.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            Guard.ObjectFound(order);

            var progress = await GetProgressAsync(order);

            return progress;
        }

        public async Task RefreshProgressAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            Guard.ObjectFound(order);
            var progress = await GetProgressAsync(order);

            if (progress >= order.DatSetRequiredCount)
            {
                order.IsCompleted = true;
            }

            order.CurrentProgress = progress;

            await _context.SaveChangesAsync();
        }

        private async Task<int> GetProgressAsync(Entities.Order order)
        {
            var dataSetCountQuery = _context.DataSet.AsNoTracking()
                .Where(d => d.OrderId == order.Id);

            if (order.Type == OrderType.LabelData)
            {
                dataSetCountQuery = dataSetCountQuery.Where(d => d.LabeledImageSource != null);
            }

            var count = await dataSetCountQuery.CountAsync();

            return count;
        }
    }
}
