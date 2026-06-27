namespace Como.CRM.Api.Domain.Entities;

public class Role : BaseTenantEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
