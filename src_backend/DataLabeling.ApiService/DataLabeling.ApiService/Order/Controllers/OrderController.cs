using DataLabeling.Api.Common.ApiResults;
using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Extensions;
using DataLabeling.ApiService.Order.Mappers;
using DataLabeling.ApiService.Order.Models;
using DataLabeling.Common.Order;
using DataLabeling.Infrastructure.ImageHelpers;
using DataLabeling.Services.Interfaces.Data.Services;
using DataLabeling.Services.Interfaces.FileStorage.Services;
using DataLabeling.Services.Interfaces.Order.Services;
using DataLabeling.Services.Interfaces.Sallary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPriceService _priceService;
        private readonly IDataService _dataService;
        private readonly IFileStorage _fileStorage;
        private readonly IUserAccessor _userAccessor;

        public OrderController(IOrderService orderService,
                               IPriceService sallaryService,
                               IDataService dataService,
                               IFileStorage fileStorage,
                               IUserAccessor userAccessor)
        {
            _orderService = orderService;
            _priceService = sallaryService;
            _dataService = dataService;
            _fileStorage = fileStorage;
            _userAccessor = userAccessor;
        }

        [HttpGet("price")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetOrderPriceAsync([FromQuery] GetOrderPriceRequest request)
        {
            var model = request.MapToModel();
            var price = _priceService.CalculateOrderPriceAsync(model);

            return this.OkApiResponse(price);
        }

        [HttpPost]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> CreateAsync([FromForm] CreateOrderRequest request)
        {
            if (request.Type == OrderType.LabelData)
            {
                _dataService.ValidateImageOriginality(request.ImageFiles.Select(i => (Func<Stream>)i.OpenReadStream));
            }

            var orderParametersModel = request.MapToOrderParametersModel();
            var price = _priceService.CalculateOrderPriceAsync(orderParametersModel);
            var model = request.MapToModel(_userAccessor.User.Id, price);
            var orderModel = await _orderService.CreateAsync(model);

            if (request.Type == OrderType.LabelData)
            {
                foreach (var image in request.ImageFiles)
                {
                    var hash = await ImageHelper.GetHashAsync(image.OpenReadStream);
                    var imgPath = await _fileStorage.UploadFileAsync(image.FileName, image.OpenReadStream);

                    await _dataService.AddRawDataAsync(orderModel.Id, imgPath, hash);
                }
            }

            return NoContent();
        }

        [HttpGet("progress/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetProgressAsync(int orderId)
        {
            var progress = await _orderService.GetProgressAsync(orderId);

            return this.OkApiResponse(progress);
        }

        [HttpPost("refresh-progress/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> RefreshProgressAsync(int orderId)
        {
            await _orderService.RefreshProgressAsync(orderId);

            return NoContent();
        }

        [HttpGet("for-customer/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetOrderAsync(int orderId)
        {
            var order = await _orderService.GetOrderForCustomerAsync(orderId, _userAccessor.User.Id);

            return this.OkApiResponse(order);
        }

        [HttpGet("for-performer/{orderId}")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> GetOrderForPerformerAsync(int orderId)
        {
            var orders = await _orderService.GetOrderForPerformerAsync(orderId, _userAccessor.User.Id);

            return this.OkApiResponse(orders);
        }


        [HttpGet("for-customer")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetOrdersForCustomerAsync([FromQuery] GetOrdersForCustomerRequest request)
        {
            var filter = request.MapToModel(_userAccessor.User.Id);
            var orders = await _orderService.GetOrdersAsync(filter);

            return this.OkApiResponse(orders);
        }

        [HttpGet("allowed-priorities")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> GetAllowedPrioritiesAsync()
        {
            var allowed = await _orderService.GetAllowedPriorities(_userAccessor.User.Id);

            return this.OkApiResponse(allowed);
        }

        [HttpGet("for-performer")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> GetOrdersForPerformerAsync([FromQuery] GetOrdersForPerformerRequest request)
        {
            var allowedPriorities = await _orderService.GetAllowedPriorities(_userAccessor.User.Id);
            var filter = request.MapToModel(allowedPriorities);
            var orders = await _orderService.GetOrdersAsync(filter);

            return this.OkApiResponse(orders);
        }

        [HttpPost("cancel/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> CancelAsync(int orderId)
        {
            await _orderService.CancelAsync(orderId, _userAccessor.User.Id);

            return NoContent();
        }

        [HttpGet("performers/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetPerformersAsync(int orderId)
        {
            var performers = await _orderService.GetPerformersForReview(orderId);

            return this.OkApiResponse(performers);
        }
    }
}
