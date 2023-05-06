using DataLabeling.Common.Shared;

namespace DataLabeling.Services.Interfaces.Review.Models
{
    public interface IReviewModel
    {
        public int Id { get; }

        public int CustomerId { get; }

        public int PerformerId { get; }

        public int OrderId { get;}

        public Rating Rating { get; }

        public string Message { get; }
    }
}
