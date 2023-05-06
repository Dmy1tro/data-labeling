using DataLabeling.Services.Interfaces.Review.Models;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.Review.Services
{
    public interface IReviewService
    {
        Task AddAsync(IReviewModel reviewModel);
    }
}
