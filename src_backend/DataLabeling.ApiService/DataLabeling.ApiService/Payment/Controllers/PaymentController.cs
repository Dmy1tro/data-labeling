using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Extensions;
using DataLabeling.ApiService.Payment.Mappers;
using DataLabeling.ApiService.Payment.Models;
using DataLabeling.Services.Interfaces.Order.Services;
using DataLabeling.Services.Interfaces.Payment.Services;
using DataLabeling.Services.Payment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLabeling.ApiService.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IUserAccessor _userAccessor;

        public PaymentController(IPaymentService paymentService,
                                 IOrderService orderService,
                                 IUserAccessor userAccessor)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _userAccessor = userAccessor;
        }

        [HttpGet("liqpay-data/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetLiqPayDataAsync(int orderId)
        {
            var order = await _orderService.GetOrderForCustomerAsync(orderId, _userAccessor.User.Id);
            var liqPayData = await _paymentService.GetLiqPayData(order.Price,
                                                                $"{Request.Scheme}://{Request.Host}/api/payment/confirm-liqpay/{orderId}/{_userAccessor.User.Id}");

            return this.OkApiResponse(liqPayData);
        }

        [HttpGet("customer-history")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> GetCustomerHistoryAsync()
        {
            var history = await _paymentService.GetOrderPaymentHistory(_userAccessor.User.Id);

            // Response doesn't return base interface properties
            var response = history.Select(x => (object)x).ToList();

            return this.OkApiResponse(response);
        }

        [HttpGet("performer-history")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> GetPerformerHistoryAsync()
        {
            var history = await _paymentService.GetJobPaymentHistory(_userAccessor.User.Id);

            // Response doesn't return base interface properties
            var response = history.Select(x => (object)x).ToList();

            return this.OkApiResponse(response);
        }

        [HttpPost("confirm-liqpay/{orderId}/{customerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmLiqPayPaymentAsync(int orderId, int customerId)
        {
            var requestDictionary = Request.Form.Keys.ToDictionary(key => key, key => Request.Form[key]);

            var isValid = await _paymentService.VerifyLiqPaySignature(requestDictionary["data"], requestDictionary["signature"]);

            if (isValid)
            {
                var order = await _orderService.GetOrderAsync(orderId);

                await _paymentService.AddPayment(new OrderPaymentModel
                {
                    CustomerId = customerId,
                    OrderId = orderId,
                    Price = order.Price
                });
            }

            return Redirect("http://localhost:4200/orders/customer");
        }

        [HttpPost("confirm/{orderId}")]
        [Authorize(Policy = PolicyName.ForCustomer)]
        public async Task<IActionResult> ConfirmPaymentAsync(int orderId)
        {
            var order = await _orderService.GetOrderForCustomerAsync(orderId, _userAccessor.User.Id);

            await _paymentService.AddPayment(new OrderPaymentModel
            {
                CustomerId = _userAccessor.User.Id,
                OrderId = orderId,
                Price = order.Price
            });

            return NoContent();
        }

        [HttpPost("withdraw-money")]
        [Authorize(Policy = PolicyName.ForPerformer)]
        public async Task<IActionResult> WithdrawMoneyAsync(WithdrawMoneyRequest request)
        {
            var model = request.MapToModel(_userAccessor.User.Id);

            await _paymentService.AddPayment(model);

            return NoContent();
        }
    }
}
