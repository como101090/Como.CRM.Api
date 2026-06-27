using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Services.Implementations;

public class CurrentTenantService : ICurrentTenantService
{
    public long TenantId { get; private set; }
    public Guid TenantPublicId { get; private set; } = Guid.Empty;
    public string? Host { get; private set; }

    public bool IsResolved => TenantId > 0 && TenantPublicId != Guid.Empty;
    public bool IsSystemContext { get; private set; }

    public void SetTenant(long tenantId, Guid tenantPublicId, string host)
    {
        TenantId = tenantId;
        TenantPublicId = tenantPublicId;
        Host = host;
    }

    public void ClearTenant()
    {
        TenantId = 0;
        TenantPublicId = Guid.Empty;
        Host = null;
        IsSystemContext = false;
    }

    public void EnableSystemContext()
    {
        IsSystemContext = true;
    }

    public void DisableSystemContext()
    {
        IsSystemContext = false;
    }
}
