using Como.CRM.Api.Common.Business.Auth;
using Como.CRM.Api.Common.Business.Tenant;
using Como.CRM.Api.Common.Exceptions;
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
    private readonly ICurrentUserService _currentUserService;


    public AuthService(
        AppDbContext db,
        ICurrentTenantService tenant,
        IPasswordHasher<AppUser> passwordHasher,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUserService)
    {
        _db = db;
        _tenant = tenant;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _currentUserService = currentUserService;
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


    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId;
        var userName = _currentUserService.UserName;
        var tenantPublicId = _currentUserService.TenantPublicId;
        var host = _currentUserService.TenantHost;

        if(tenantPublicId == null)
            throw new BusinessException(TenantBusinessCodes.TenantNotFound, StatusCodes.Status404NotFound);

        if (host == null)
            throw new BusinessException(TenantBusinessCodes.TenantNotFound, StatusCodes.Status404NotFound);

        var tenant = await _db.Tenants
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.PublicId == tenantPublicId && x.Host == host, ct);

        if (tenant == null)
            throw new BusinessException(TenantBusinessCodes.TenantNotFound, StatusCodes.Status404NotFound);

        if (!tenant.IsActive || tenant.IsRemove)
            throw new BusinessException(TenantBusinessCodes.TenantSuspended, StatusCodes.Status403Forbidden);


        if (userId == null)
            throw new BusinessException(AuthBusinessCodes.UserNotFound, StatusCodes.Status404NotFound);

        if (userName == null)
            throw new BusinessException(AuthBusinessCodes.UserNotFound, StatusCodes.Status404NotFound);

        var user = await _db.Users.FirstOrDefaultAsync(x => x.TenantId == tenant.Id && x.UserName == userName && x.Id == userId, ct);

        if (user == null)
            throw new BusinessException(AuthBusinessCodes.UserNotFound, StatusCodes.Status404NotFound);

        if(!user.IsActive || user.IsRemove)
            throw new BusinessException(AuthBusinessCodes.UserNotFound, StatusCodes.Status404NotFound);

        var currentPasswordOk = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);

        if (currentPasswordOk == PasswordVerificationResult.Failed)
        {
            throw new BusinessException(
                AuthBusinessCodes.CurrentPasswordIsWrong,
                StatusCodes.Status400BadRequest);
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        //user.UpdateDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}
