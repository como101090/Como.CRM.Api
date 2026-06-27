namespace Como.CRM.Api.DTOs.Tenants;

public class RegisterTenantResponse
{
    public string Host { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string BrendName { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;

    public bool IsSendEmailSuccess { get; set; }
}
