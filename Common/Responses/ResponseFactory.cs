namespace Como.CRM.Api.Common.Responses;

public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(
        T data,
        int statusCode = StatusCodes.Status200OK)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Data = data
        };
    }

    public static ApiResponse<T> Created<T>(T data)
    {
        return Success(data, StatusCodes.Status201Created);
    }

    public static ApiResponse<T> Error<T>(
        string errorCode,
        int statusCode,
        List<ApiError>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            ErrorCode = errorCode,
            Errors = errors
        };
    }

    public static PagedResponse<T> Paged<T>(
        List<T> data,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PagedResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }
}