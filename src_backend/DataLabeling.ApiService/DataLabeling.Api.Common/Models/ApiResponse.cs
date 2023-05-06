namespace DataLabeling.Api.Common.Models
{
    public class ApiResponse<T>
    {
        private ApiResponse(T data)
        {
            Payload = data;
        }

        public static ApiResponse<T> With(T data) => new(data);

        public T Payload { get; init; }
    }
}
