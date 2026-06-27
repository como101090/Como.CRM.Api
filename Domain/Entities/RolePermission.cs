namespace Como.CRM.Api.Domain.Entities;

public class RolePermission : IHasTenant
{
    public long TenantId { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public long PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
