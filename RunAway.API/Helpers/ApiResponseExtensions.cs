using Microsoft.AspNetCore.Mvc;

namespace RunAway.API.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public static class ApiResponseExtensions
    {
        public static ActionResult<ApiResponse<T>> ToApiResponse<T>(this T data, int statusCode = 200, string message = "Success")
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };

            return new ObjectResult(response) { StatusCode = statusCode };
        }

        public static ActionResult<ApiResponse<object>> ToApiError(this ControllerBase controller, int statusCode, string message, List<string> errors = null)
        {
            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Errors = errors ?? new List<string>()
            };

            return controller.StatusCode(statusCode, response);
        }
    }
}
