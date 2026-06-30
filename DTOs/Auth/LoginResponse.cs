namespace Como.CRM.Api.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; }

    public DateTime ExpiresAtUtc { get; set; }
    public List<string> Permissions { get; set; } = new();
}
