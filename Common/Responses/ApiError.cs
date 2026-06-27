using Como.CRM.Api.Common.Validation;

namespace Como.CRM.Api.Common.Responses
{
    public class ApiError
    {
        public string Field { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Message { get; set; } 

        public ValidationRule? Rule { get; set; }
    }
}
