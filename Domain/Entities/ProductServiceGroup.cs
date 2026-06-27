namespace Como.CRM.Api.Domain.Entities;

public class ProductServiceGroup : BaseTenantEntity
{
    public string Code { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public bool IsProduct { get; set; } = true;
}
