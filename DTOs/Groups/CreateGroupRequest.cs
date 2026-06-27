using System.ComponentModel.DataAnnotations;

namespace Como.CRM.Api.DTOs.Groups;

public class CreateGroupRequest
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string GroupName { get; set; } = string.Empty;

    public bool IsProduct { get; set; } = true;
}
