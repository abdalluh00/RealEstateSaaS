namespace RealEstate.Shared.Common
{
    public sealed class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = [];

        public static ApiResponse<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Fail(string error) =>
            new() { Success = false, Errors = [error] };

        public static ApiResponse<T> Fail(IReadOnlyList<string> errors) =>
            new() { Success = false, Errors = errors };

        // For paged responses
        public static ApiResponse<PagedResult<T>> OkPaged(PagedResult<T> data) =>
            new ApiResponse<PagedResult<T>>() { Success = true, Data = data };
    }
}