using DataLabeling.Common.Shared;
using DataLabeling.Services.Interfaces.Review.Models;

namespace DataLabeling.Services.Review.Models
{
    public class ReviewModel : IReviewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int PerformerId { get; set; }

        public int OrderId { get; set; }

        public Rating Rating { get; set; }

        public string Message { get; set; }
    }
}
