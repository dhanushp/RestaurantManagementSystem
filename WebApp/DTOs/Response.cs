namespace WebApp.DTOs
{
    public record Response<T>(bool Success = false, string Message = "", T? Data = default, ErrorCode? ErrorCode = null)
    {
        // Factory Methods
        public static Response<T> SuccessResponse(string message, T data) => new Response<T>(true, message, data);
        public static Response<T> ErrorResponse(string message, ErrorCode? errorCode = null) => new Response<T>(false, message, default, errorCode);
        public static Response<T> ErrorResponse(Exception exception) => new Response<T>(false, exception.Message);
    }
}
