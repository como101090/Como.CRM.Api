using System.Security.Claims;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public long? UserId
    {
        get
        {
            var value = _accessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return long.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? UserName =>
        _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    public Guid? TenantPublicId
    {
        get
        {
            var value = _accessor.HttpContext?
                .User
                .FindFirst("tenant_id")?.Value;

            return Guid.TryParse(value, out var tenantId)
                ? tenantId
                : null;
        }
    }

    public string? TenantHost =>
        _accessor.HttpContext?
            .User
            .FindFirst("tenant_host")?.Value;
}
