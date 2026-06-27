namespace Como.CRM.Api.Common.Responses
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }

        public bool Success { get; set; }

        public T? Data { get; set; }

        public string? ErrorCode { get; set; }

        public List<ApiError>? Errors { get; set; }
    }
}
