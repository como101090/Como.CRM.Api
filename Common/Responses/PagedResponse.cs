namespace Como.CRM.Api.Common.Responses
{
    public class PagedResponse<T> : ApiResponse<List<T>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
