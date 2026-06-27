namespace Como.CRM.Api.Domain.Entities;

public class AuditLog : IHasTenant
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public long EntityId { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public long? UserId { get; set; }
    public DateTime OperationDate { get; set; } = DateTime.UtcNow;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}
