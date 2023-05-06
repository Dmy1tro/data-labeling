using DataLabeling.Api.Common.ApiResults;
using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Extensions;
using DataLabeling.ApiService.User.Mappers;
using DataLabeling.ApiService.User.Models;
using DataLabeling.DAL.Services.Interfaces.User.Services;
using DataLabeling.Services.Interfaces.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IUserAccessor _userAccessor;

        public UserController(IUserService userService, 
                              IJwtService jwtService,
                              IUserAccessor userAccessor)
        {
            _userService = userService;
            _jwtService = jwtService;
            _userAccessor = userAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest model)
        {
            var authResult = await _userService.Authenticate(model.Email, model.Password, model.UserType);

            if (!authResult.IsSuccess) return new ProblemResult(authResult.FailureReason);

            var token = _jwtService.GenerateToken(authResult.User);
            var response = authResult.ToLoginResponseModel(token);

            return this.OkApiResponse(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var registerModel = request.ToRegisterModel();
            var registerResult = await _userService.Register(registerModel);

            if (!registerResult.IsSuccess) return new ProblemResult(registerResult.FailureReason);

            var user = registerResult.User;
            await _userService.ConfirmEmailAsync(user.Email, user.PinConfirmation, user.UserType);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmAsync([FromQuery] ConfirmRequest request)
        {
            await _userService.ConfirmEmailAsync(request.Email, request.Pin, request.UserType);

            return NoContent();
        }

        [Authorize(Policy = PolicyName.ForPerformer)]
        [HttpGet("performer-info")]
        public async Task<IActionResult> GetPerformerInfoAsync()
        {
            var performer = await _userService.GetPerformerAsync(_userAccessor.User.Id);

            var response = performer.ToResponse();

            return this.OkApiResponse(response);
        }

        [Authorize(Policy = PolicyName.ForPerformer)]
        [HttpGet("performer-statistic")]
        public async Task<IActionResult> GetPerformerStatisticAsync()
        {
            var statistic = await _userService.GetPerformerStatisticAsync(_userAccessor.User.Id);

            return this.OkApiResponse(statistic);
        }

        [Authorize(Policy = PolicyName.ForCustomer)]
        [HttpGet("customer-statistic")]
        public async Task<IActionResult> GetCustomerStatisticAsync()
        {
            var statistic = await _userService.GetCustomerStatisticAsync(_userAccessor.User.Id);

            return this.OkApiResponse(statistic);
        }
    }
}
