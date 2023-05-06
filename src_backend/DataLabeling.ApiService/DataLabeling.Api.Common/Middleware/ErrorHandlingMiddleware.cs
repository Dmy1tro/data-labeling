using DataLabeling.Api.Common.Models;
using DataLabeling.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DataLabeling.Api.Common.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var timeStamp = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.ff");

            switch (exception)
            {
                case BusinessLogicException ex:
                    await WriteErrorAsync(context, ex.Message, HttpStatusCode.BadRequest);
                    _logger.LogError(exception, $"{timeStamp}: {ex.Message}");
                    break;

                default:
                    await WriteErrorAsync(context, $"Something went wrong", HttpStatusCode.InternalServerError);
                    _logger.LogError(exception, $"{timeStamp}: {exception.Message}");
                    break;
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, string error, HttpStatusCode statusCode)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializedError = JsonConvert.SerializeObject(new ErrorDetail(error, statusCode), jsonSerializerSettings);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(serializedError);
        }
    }
}
