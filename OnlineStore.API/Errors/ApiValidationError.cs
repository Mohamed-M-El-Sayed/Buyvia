namespace OnlineStore.API.Errors
{
    public class ApiValidationError : ApiError
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationError(IEnumerable<string> errors)
            : base(400, "Validation failed.")
        {
            Errors = errors;
        }
    }
}
