namespace RunAway.Application.Commons
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string ErrorMessage { get; }
        public ErrorCode ErrorCode { get; }
        public int ApiResponseErrorCode { get; }
        public List<string>? ValidationErrors { get; }

        private Result(bool isSuccess, T? value, string errorMessage, ErrorCode errorCode, List<string>? validationErrors, int apiErrorResponseCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ValidationErrors = validationErrors;
            ApiResponseErrorCode = apiErrorResponseCode;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, string.Empty, ErrorCode.None, null, 0);
        }

        public static Result<T> Failure(string errorMessage, int apiResponseErrorCode = 0, ErrorCode errorCode = ErrorCode.UnknownError)
        {
            return new Result<T>(false, default, errorMessage, errorCode, null, apiResponseErrorCode);
        }

        public static Result<T> ValidationFailure(List<string> validationErrors, string errorMessage = "Validation failed")
        {
            return new Result<T>(false, default, errorMessage, ErrorCode.ValidationError, validationErrors, 400);
        }
    }

    public enum ErrorCode
    {
        None = 0,
        UnknownError = 1,
        ValidationError = 2,
        NotFound = 3,
        Unauthorized = 4,
        InvalidOperation = 10,
        InvalidArgument = 11

        //// Accommodation related errors
        //AccommodationNotFound = 40,

        //// Room related errors
        //RoomNotFound = 10,
        //RoomNotAvailable = 11,
        //InsufficientRoomsAvailable = 12,

        //// Availability related errors
        //NoAvailabilityRecordsFound = 13,
        //InvalidDateRange = 14,
        //DuplicateAvailabilityRecord = 15,

        //// User related errors
        //UserNotFound = 20,
        //UserNotAuthenticated = 21,

        //// Transaction related errors
        //TransactionFailed = 30,
        //PaymentFailed = 31
    }
}
