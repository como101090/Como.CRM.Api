namespace Como.CRM.Api.DTOs.Groups;

public class GroupDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public bool IsProduct { get; set; }
    public DateTime CreateDate { get; set; }
}
