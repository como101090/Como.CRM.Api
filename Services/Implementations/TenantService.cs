using Como.CRM.Api.Common;
using Como.CRM.Api.Common.Business.Tenant;
using Como.CRM.Api.Common.EmailTemplates;
using Como.CRM.Api.Common.Exceptions;
using Como.CRM.Api.Data;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Tenants;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Como.CRM.Api.Services.Implementations;

public class TenantService : ITenantService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ITransactionService _transactionService;
    private readonly ICurrentLanguage _currentLanguage;
    private readonly IEmailService _emailService;

    public TenantService(
                    AppDbContext db,
                    IPasswordHasher<AppUser> passwordHasher,
                    IHttpContextAccessor httpContextAccessor,
                    ICurrentTenantService currentTenant,
                    ITransactionService transactionService,
                    IEmailService emailService,
                    ICurrentLanguage currentLanguage)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _currentTenant = currentTenant;
        _httpContextAccessor = httpContextAccessor;
        _transactionService = transactionService;
        _emailService = emailService;
     
        _currentLanguage = currentLanguage;
    }

    public async Task<RegisterTenantResponse> RegisterAsync(
        RegisterTenantRequest request,
        CancellationToken cancellationToken = default)
    {
        var host = await GenerateUniqueHostAsync(request.BrandName);


        if (await _db.Tenants.AnyAsync(z => z.BrandName == request.BrandName.Trim()))
            throw new BusinessException(
                           TenantBusinessCodes.BrendNameAlreadyExists,
                           StatusCodes.Status409Conflict);

        if (await _db.Tenants.AnyAsync(z => z.CompanyEmail.ToLower() == request.CompanyEmail.Trim().ToLower()))
            throw new BusinessException(
                           TenantBusinessCodes.CompanyEmailAlreadyExists,
                           StatusCodes.Status409Conflict);

        if (await _db.Tenants.AnyAsync(z => z.ContactPhone == request.ContactPhone.Trim()))
            throw new BusinessException(
                           TenantBusinessCodes.PhoneAlreadyExists,
                           StatusCodes.Status409Conflict);

        //if (await _db.Tenants.AnyAsync(z => z.LegalName == request.LegalName.Trim()))
        //    throw new BusinessException(
        //                   TenantBusinessCodes.LegalNameAlreadyExists,
        //                   StatusCodes.Status409Conflict);

        if (await _db.Tenants.AnyAsync(z => z.Host == host))
            throw new BusinessException(
                           TenantBusinessCodes.HostAlreadyExists,
                           StatusCodes.Status409Conflict);


        var res = await _transactionService.ExecuteAsync(async ct =>
        {
           
            var password = GeneratePassword();

            _currentTenant.EnableSystemContext();

            try
            {
                var tenant = new Tenant
                {
                    PublicId = Guid.NewGuid(),
                    //LegalName = request.LegalName.Trim(),
                    BrandName = request.BrandName.Trim(),
                    Host = host,
                    CompanyEmail = request.CompanyEmail.Trim(),
                    ContactPhone = request.ContactPhone.Trim(),
                    IsActive = true
                };

                _db.Tenants.Add(tenant);
                await _db.SaveChangesAsync(ct);

                _currentTenant.SetTenant(tenant.Id, tenant.PublicId, tenant.Host);

                var adminRole = new Role
                {
                    TenantId = tenant.Id,
                    Name = "Administrator",
                    IsSystem = true
                };

                _db.Roles.Add(adminRole);

                var adminUser = new AppUser
                {
                    TenantId = tenant.Id,
                    UserName = "Admin",
                    IsActive = true
                };

                adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, password);

                _db.Users.Add(adminUser);
                await _db.SaveChangesAsync(ct);

                _db.UserRoles.Add(new UserRole
                {
                    TenantId = tenant.Id,
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                });

                await _db.SaveChangesAsync(ct);

                await _db.Database.ExecuteSqlInterpolatedAsync($@"
                                        INSERT INTO RolePermissions (TenantId, RoleId, PermissionId)
                                        SELECT {tenant.Id}, {adminRole.Id}, Id
                                        FROM Permissions
                                        WHERE Id IS NOT NULL
                                    ", ct);

                var url = BuildTenantUrl(tenant.Host);

                return new
                {
                    tenant.Host,
                    Url = url,
                    CompanyEmail = tenant.CompanyEmail,
                    BrendName = tenant.BrandName,
                    AdminUserName = adminUser.UserName,
                    Password = password
                };
            }
            finally
            {
                _currentTenant.ClearTenant();
                _currentTenant.DisableSystemContext();
            }
        }, cancellationToken);

        var isSendEmailSuccess = await _emailService.SendAsync(
            res.CompanyEmail,
            "Como CRM",
            WelcomeEmailTemplate.Build(
                res.BrendName,
                res.Url,
                res.AdminUserName,
                res.Password,
                _currentLanguage.Language),
            true,
            cancellationToken);

        return new RegisterTenantResponse
        {
            Host = res.Host,
            Url = res.Url,
            CompanyEmail = res.CompanyEmail,
            BrendName = res.BrendName,
            IsSendEmailSuccess = isSendEmailSuccess
        };
    }


    private string BuildTenantUrl(string tenantHost)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null.");

        var request = httpContext.Request;

        var scheme = request.Scheme;
        var host = request.Host.Host;
        var port = request.Host.Port;

        var normalizedTenantHost = tenantHost.Trim().ToLowerInvariant();

        string urlHost;

        // Development
        // եկածը՝ crm.localhost կամ localhost
        // պետք է դառնա՝ crystaldent.localhost
        if (host.EndsWith("localhost", StringComparison.OrdinalIgnoreCase))
        {
            urlHost = $"{normalizedTenantHost}.crm.localhost";
        }
        // Production portal
        // եկածը՝ crm.comocode.am
        // պետք է դառնա՝ crystaldent.crm.comocode.am
        else if (host.Equals("crm.comocode.am", StringComparison.OrdinalIgnoreCase))
        {
            urlHost = $"{normalizedTenantHost}.crm.comocode.am";
        }
        // Եթե արդեն ուրիշ domain-ից է եկել, պահում ենք եկած host-ը
        // օրինակ՝ staging.crm.comocode.am → crystaldent.staging.crm.comocode.am
        else
        {
            urlHost = $"{normalizedTenantHost}.{host}";
        }

        var url = $"{scheme}://{urlHost}";

        if (port.HasValue)
            url += $":{port.Value}";

        return url;
    }



    private async Task<string> GenerateUniqueHostAsync(string brandName)
    {
        var baseHost = Regex.Replace(brandName.Trim(), "[^a-zA-Z0-9]", "");
        if (string.IsNullOrWhiteSpace(baseHost))
            baseHost = null;

        var host = baseHost.ToLowerInvariant();
        return host;
    }

    private static string GeneratePassword(int length = 20)
    {
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string symbols = "!@#$%^&*";
        var allChars = upper + lower + digits + symbols;

        var password = new List<char>
        {
            upper[RandomNumberGenerator.GetInt32(upper.Length)],
            lower[RandomNumberGenerator.GetInt32(lower.Length)],
            digits[RandomNumberGenerator.GetInt32(digits.Length)],
            symbols[RandomNumberGenerator.GetInt32(symbols.Length)]
        };

        for (var i = password.Count; i < length; i++)
            password.Add(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);

        for (var i = password.Count - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password.ToArray());
    }
}
