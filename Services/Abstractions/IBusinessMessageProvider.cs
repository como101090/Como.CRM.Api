using Como.CRM.Api.Enums;

namespace Como.CRM.Api.Services.Abstractions
{
    public interface IBusinessMessageProvider
    {
        string GetMessage(string code, Language language);
    }
}
