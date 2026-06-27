namespace Como.CRM.Api.Domain.Entities;

public abstract class BaseTenantEntity : IHasTenant, ISoftDeletable
{
    public long Id { get; set; }
    public long TenantId { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public long? CreateUserId { get; set; }
    public bool IsRemove { get; set; }
    public DateTime? RemoveDate { get; set; }
    public long? RemoveUserId { get; set; }
}
