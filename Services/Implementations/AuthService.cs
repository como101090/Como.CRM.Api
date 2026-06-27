using Como.CRM.Api.Data;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Auth;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Como.CRM.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ICurrentTenantService _tenant;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        AppDbContext db,
        ICurrentTenantService tenant,
        IPasswordHasher<AppUser> passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _db = db;
        _tenant = tenant;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (!_tenant.IsResolved)
            throw new UnauthorizedAccessException("Tenant context is required for login.");

        var tenant = await _db.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == _tenant.TenantId && x.IsActive, cancellationToken)
            ?? throw new UnauthorizedAccessException("Tenant not found or inactive.");

        var user = await _db.Users
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                        .ThenInclude(x => x.Permission)
            .FirstOrDefaultAsync(x => x.UserName == request.UserName && !x.IsRemove, cancellationToken);
            //?? throw new UnauthorizedAccessException("Invalid username or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("User is inactive.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var permissions = user.UserRoles
            .Where(ur => !ur.Role.IsRemove)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .Distinct()
            .ToList();

        return _jwtTokenService.CreateToken(tenant, user, permissions);
    }
}
