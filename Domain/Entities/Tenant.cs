namespace Como.CRM.Api.Domain.Entities;

public class Tenant
{
    public long Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public string LegalName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string CompanyEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public bool IsRemove { get; set; } = false;
}
