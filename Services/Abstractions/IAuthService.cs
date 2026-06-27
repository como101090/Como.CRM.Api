using Como.CRM.Api.DTOs.Auth;

namespace Como.CRM.Api.Services.Abstractions;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
