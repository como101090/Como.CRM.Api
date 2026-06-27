using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Auth;

namespace Como.CRM.Api.Services.Abstractions;

public interface IJwtTokenService
{
    LoginResponse CreateToken(Tenant tenant, AppUser user, List<string> permissions);
}
