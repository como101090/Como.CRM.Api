using Como.CRM.Api.Data;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Como.CRM.Api.Middleware;

/// <summary>
/// Tenant-ը որոշում է request-ի Host/Subdomain-ից։
///
/// Production:
/// https://crystaldent.crm.comocode.am/api/auth/login
///
/// Development:
/// https://crystaldent.localhost:7080/api/auth/login
///
/// Առանց tenant context-ի կարող են աշխատել միայն.
/// - Tenant գրանցում
/// - Swagger
/// - Health
/// - Root
///
/// Մնացած բոլոր API-ները պարտադիր պետք է ունենան tenant context։
/// </summary>
public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
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

        var rawHost = context.Request.Host.Host;
        var tenantHost = ExtractTenantFromHost(rawHost);

        if (string.IsNullOrWhiteSpace(tenantHost))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(
                "Invalid tenant. Use https://{tenant}.crm.comocode.am or https://{tenant}.localhost:7080.",
                context.RequestAborted);

            return;
        }

        var normalizedHost = NormalizeTenantHost(tenantHost);

        var tenant = await db.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.Host.ToLower() == normalizedHost &&
                    x.IsActive &&
                    !x.IsRemove,
                context.RequestAborted);

        if (tenant == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(
                "Tenant not found or inactive.",
                context.RequestAborted);

            return;
        }

        context.Items["TenantId"] = tenant.Id;
        context.Items["Subdomain"] = tenant.Host;
        context.Items["Host"] = rawHost;

        currentTenant.SetTenant(tenant.Id, tenant.PublicId, tenant.Host);

        await _next(context);
    }

    private static bool ShouldSkipTenantResolution(PathString path)
    {
        var value = path.Value?.ToLowerInvariant() ?? string.Empty;

        return value.StartsWith("/api/tenants/register")
               || value.StartsWith("/swagger")
               || value.StartsWith("/health")
               || value == "/"
               || value.StartsWith("/favicon");
    }

    private static string? ExtractTenantFromHost(string? host)
    {
        if (string.IsNullOrWhiteSpace(host))
            return null;

        host = host.Trim().ToLowerInvariant();

        if (host == "localhost" ||
            host == "127.0.0.1" ||
            host == "::1")
        {
            return null;
        }

        var parts = host.Split(
            '.',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length < 2)
            return null;

        // Development:
        // crystaldent.localhost:7080
        // Host = crystaldent.localhost
        // Tenant = crystaldent
        if (parts.Length >= 2 &&
            parts[^1].Equals("localhost", StringComparison.OrdinalIgnoreCase))
        {
            return NormalizeTenantHost(parts[0]);
        }

        // Production:
        // crystaldent.crm.comocode.am
        // Tenant = crystaldent
        if (parts.Length >= 4 &&
            parts[1].Equals("crm", StringComparison.OrdinalIgnoreCase) &&
            parts[2].Equals("comocode", StringComparison.OrdinalIgnoreCase) &&
            parts[3].Equals("am", StringComparison.OrdinalIgnoreCase))
        {
            return NormalizeTenantHost(parts[0]);
        }

        // crm.comocode.am → սա register/login portal է, tenant չկա
        // comocode.am կամ www.comocode.am → WordPress / company site, tenant չկա
        return null;
    }

    private static string NormalizeTenantHost(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}