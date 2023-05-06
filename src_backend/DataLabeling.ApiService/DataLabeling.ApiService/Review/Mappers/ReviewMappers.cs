using DataLabeling.ApiService.Review.Models;
using DataLabeling.Services.Interfaces.Review.Models;
using DataLabeling.Services.Review.Models;

namespace DataLabeling.ApiService.Review.Mappers
{
    public static class ReviewMappers
    {
        public static IReviewModel MapToModel(this AddReviewRequest request, int customerId)
        {
            return new ReviewModel
            {
                CustomerId = customerId,
                Message = request.Message,
                OrderId = request.OrderId,
                PerformerId = request.PerformerId,
                Rating = request.Rating
            };
        }
    }
}
