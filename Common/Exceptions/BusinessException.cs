using Como.CRM.Api.Common.Responses;

namespace Como.CRM.Api.Common.Exceptions;

public class BusinessException : Exception
{
    public int StatusCode { get; }

    public string ErrorCode { get; }

    public List<ApiError>? Errors { get; }

    public BusinessException(
        string errorCode,
        int statusCode = StatusCodes.Status400BadRequest)
        : base(errorCode)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }

    public BusinessException(
        string errorCode,
        List<ApiError> errors,
        int statusCode = StatusCodes.Status400BadRequest)
        : base(errorCode)
    {
        ErrorCode = errorCode;
        Errors = errors;
        StatusCode = statusCode;
    }
}