using DataLabeling.Common.Exceptions;
using DataLabeling.Data.Context;
using DataLabeling.Services.Interfaces.Review.Models;
using DataLabeling.Services.Interfaces.Review.Services;
using DataLabeling.Services.Order.Mappers;
using DataLabeling.Services.Review.Mappers;
using DataLabeling.Services.Validators;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataLabeling.Services.Review.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DateLabelingDbContext _context;

        public ReviewService(DateLabelingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IReviewModel reviewModel)
        {
            var review = reviewModel.MapToEntity();
            var order = await _context.Orders.FindAsync(review.OrderId);
            var model = order?.MapToModel();

            OrderValidator.CheckCustomerMatch(model, reviewModel.CustomerId);

            var isReviewExists = await _context.Reviews.AsNoTracking()
                .AnyAsync(r => r.CustomerId == review.CustomerId && 
                               r.PerformerId == review.PerformerId && 
                               r.OrderId == review.OrderId);

            if (isReviewExists)
            {
                throw new ValidationException("You cannot add review twice for the same performer and the same order.");
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}
