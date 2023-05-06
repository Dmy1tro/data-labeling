using DataLabeling.Api.Common.Authentication;
using DataLabeling.ApiService.Review.Mappers;
using DataLabeling.ApiService.Review.Models;
using DataLabeling.DAL.Services.Interfaces.User.Services;
using DataLabeling.Services.Interfaces.Review.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;
        private readonly IUserAccessor _userAccessor;

        public ReviewController(IReviewService reviewService,
                                IUserService userService,
                                IUserAccessor userAccessor)
        {
            _reviewService = reviewService;
            _userService = userService;
            _userAccessor = userAccessor;
        }

        [HttpPost]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> AddReviewAsync(AddReviewRequest request)
        {
            var model = request.MapToModel(_userAccessor.User.Id);

            await _reviewService.AddAsync(model);
            await _userService.RefreshPerformerRating(request.PerformerId);

            return NoContent();
        }
    }
}
