using System.Security.Claims;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Middleware;

public class TenantTokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TenantTokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICurrentTenantService currentTenant)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tokenTenantValue = context.User.FindFirstValue("tenant_id");

            if (!Guid.TryParse(tokenTenantValue, out var tokenTenantPublicId) ||
                tokenTenantPublicId == Guid.Empty)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token tenant is missing.");
                return;
            }

            if (!currentTenant.IsResolved ||
                tokenTenantPublicId != currentTenant.TenantPublicId)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Token tenant does not match request tenant.");
                return;
            }
        }

        await _next(context);
    }
}