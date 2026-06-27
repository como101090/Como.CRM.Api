namespace Como.CRM.Api.Services.Abstractions;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? UserName { get; }
}
