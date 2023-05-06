using DataLabeling.Common.Shared;

namespace DataLabeling.ApiService.Review.Models
{
    public class AddReviewRequest
    {
        public int PerformerId { get; set; }

        public int OrderId { get; set; }

        public Rating Rating { get; set; }

        public string Message { get; set; }
    }
}
