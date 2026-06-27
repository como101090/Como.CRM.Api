using Como.CRM.Api.Common.Validation;
using Como.CRM.Api.Enums;

namespace Como.CRM.Api.Services.Abstractions
{
    public interface IErrorMessageService
    {
        string GetMessage(
        Type modelType,
        string propertyName,
        string code,
        ValidationRule? rule);
    }
}
