namespace Como.CRM.Api.Services.Abstractions;

public interface ICurrentTenantService
{
    long TenantId { get; }
    Guid TenantPublicId { get; }
    string? Host { get; }

    bool IsResolved { get; }
    bool IsSystemContext { get; }

    void SetTenant(long tenantId, Guid tenantPublicId, string host);
    void ClearTenant();

    void EnableSystemContext();
    void DisableSystemContext();
}