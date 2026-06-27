using Como.CRM.Api.DTOs.Tenants;

namespace Como.CRM.Api.Services.Abstractions;

public interface ITenantService
{
    Task<RegisterTenantResponse> RegisterAsync(RegisterTenantRequest request, CancellationToken cancellationToken = default);
}
