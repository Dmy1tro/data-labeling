using DataLabeling.Api.Common.Authentication;
using DataLabeling.Api.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataLabeling.Api.Common.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult OkApiResponse<T>(this ControllerBase controller, T value)
        {
            var res = ApiResponse<T>.With(value);
            return controller.Ok(res);
        }
    }
}
