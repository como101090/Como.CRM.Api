namespace Como.CRM.Api.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public Guid PublicTenantId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}
