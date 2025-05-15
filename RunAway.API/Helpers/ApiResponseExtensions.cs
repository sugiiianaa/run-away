using Microsoft.AspNetCore.Mvc;
using RunAway.Application.Commons;

namespace RunAway.API.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? ValidationErrors { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public static class ApiResponseExtensions
    {
        public static ActionResult<ApiResponse<T>> ToApiResponse<T>(this Result<T> result, int successStatusCode = 200, string successMessage = "Success")
        {
            if (result.IsSuccess)
            {
                var response = new ApiResponse<T>
                {
                    Success = true,
                    Message = successMessage,
                    Data = result.Value,
                    StatusCode = successStatusCode
                };
                return new ObjectResult(response) { StatusCode = successStatusCode };
            }

            return ToApiError<T>(ConvertErrorCodeToStatusCode(result.ErrorCode), result.ErrorMessage, result.ValidationErrors);
        }

        public static ActionResult<ApiResponse<T>> ToApiError<T>(int statusCode, string message, List<string>? validationErrors = null)
        {
            var response = new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = default,
                ValidationErrors = validationErrors
            };
            return new ObjectResult(response) { StatusCode = statusCode };
        }

        // Extension method to maintain compatibility with controllers directly creating errors
        public static ActionResult<ApiResponse<T>> ToApiError<T>(this ControllerBase controller, int statusCode, string message, List<string>? errors = null)
        {
            var response = new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = default,
                ValidationErrors = errors
            };
            return new ObjectResult(response) { StatusCode = statusCode };
        }

        private static int ConvertErrorCodeToStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFound => 404,

                ErrorCode.ValidationError or
                ErrorCode.InvalidArgument or
                ErrorCode.InvalidOperation => 400,

                ErrorCode.Unauthorized => 401,

                _ => 500
            };
        }
    }
}
