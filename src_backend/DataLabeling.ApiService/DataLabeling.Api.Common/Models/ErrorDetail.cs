using System.Net;

namespace DataLabeling.Api.Common.Models
{
    public record ErrorDetail(string Detail, HttpStatusCode StatusCode)
    {
        public string ContentType => "application/json";
    }
}
