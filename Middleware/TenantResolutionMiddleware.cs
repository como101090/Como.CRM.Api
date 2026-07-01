using Como.CRM.Api.Common.Business.Tenant;
using Como.CRM.Api.Common.Exceptions;
using Como.CRM.Api.Data;
using Como.CRM.Api.Options;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Como.CRM.Api.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppOptions _appOptions;

    public TenantResolutionMiddleware(
        RequestDelegate next,
        IOptions<AppOptions> appOptions)
    {
        _next = next;
        _appOptions = appOptions.Value;
    }

    public async Task InvokeAsync(
        HttpContext context,
        AppDbContext db,
        ICurrentTenantService currentTenant)
    {
        currentTenant.ClearTenant();

        if (ShouldSkipTenantResolution(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var rawHost = context.Request.Host.Value;
        var tenantHost = ExtractTenantFromHost(rawHost);

        if (string.IsNullOrWhiteSpace(tenantHost)) 
            throw new BusinessException(TenantBusinessCodes.HostInvalid, StatusCodes.Status404NotFound);

        var normalizedHost = NormalizeTenantHost(tenantHost);

        var tenant = await db.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.Host.ToLower() == normalizedHost,
                context.RequestAborted);

        if (tenant == null)
            throw new BusinessException(TenantBusinessCodes.TenantNotFound, StatusCodes.Status404NotFound);

        if(!tenant.IsActive || tenant.IsRemove)
            throw new BusinessException(TenantBusinessCodes.TenantSuspended, StatusCodes.Status404NotFound);



        context.Items["TenantId"] = tenant.Id;
        context.Items["Subdomain"] = tenant.Host;
        context.Items["Host"] = tenantHost;

        currentTenant.SetTenant(tenant.Id, tenant.PublicId, tenant.Host);

        await _next(context);
    }

    private bool ShouldSkipTenantResolution(PathString path)
    {
        var value = path.Value?.ToLowerInvariant() ?? string.Empty;

        return value.StartsWith("/api/tenants/register")
               || value.StartsWith("/swagger")
               || value.StartsWith("/health")
               || value == "/"
               || value.StartsWith("/favicon");
    }

    private string? ExtractTenantFromHost(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
            return null;

        host = NormalizeTenantHost(host);

        var baseDomain = NormalizeTenantHost(_appOptions.BaseDomain);

        if (host == baseDomain)
            return null;

        var suffix = "." + baseDomain;

        if (!host.EndsWith(suffix))
            return null;

        var tenantHost = host[..^suffix.Length];

        if (string.IsNullOrWhiteSpace(tenantHost))
            return null;

        if (tenantHost.Contains('.'))
            return null;

        return NormalizeTenantHost(tenantHost);
    }

    private static string NormalizeTenantHost(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}