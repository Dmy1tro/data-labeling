using DataLabeling.Services.Interfaces.Review.Models;

namespace DataLabeling.Services.Review.Mappers
{
    public static class ReviewMapperExtensions
    {
        public static Entities.Review MapToEntity(this IReviewModel model)
        {
            return new Entities.Review
            {
                CustomerId = model.CustomerId,
                Id = model.Id,
                Message = model.Message,
                PerformerId = model.PerformerId,
                Rating = model.Rating,
                OrderId = model.OrderId
            };
        }
    }
}
