using DataLabeling.Api.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DataLabeling.Api.Common.ApiResults
{
    public class ProblemResult : IActionResult
    {
        public ProblemResult(string detail, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            ErrorDetail = new ErrorDetail(detail, status);
        }

        public ErrorDetail ErrorDetail { get; init; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            response.ContentType = ErrorDetail.ContentType;
            response.StatusCode = (int)ErrorDetail.StatusCode;

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result = JsonConvert.SerializeObject(ErrorDetail, jsonSerializerSettings);

            await context.HttpContext.Response.WriteAsync(result);
        }
    }
}
