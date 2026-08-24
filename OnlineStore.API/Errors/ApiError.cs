namespace OnlineStore.API.Errors
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = default!;
        public ApiError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
