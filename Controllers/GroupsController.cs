using Como.CRM.Api.Common;
using Como.CRM.Api.DTOs.Groups;
using Como.CRM.Api.Security;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Como.CRM.Api.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    [HasPermission(PermissionCodes.GroupsView)]
    public async Task<ActionResult<List<GroupDto>>> GetAll([FromQuery] bool? isProduct, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.GetAllAsync(isProduct, cancellationToken));
    }

    [HttpGet("{id:long}")]
    [HasPermission(PermissionCodes.GroupsView)]
    public async Task<ActionResult<GroupDto>> GetById(long id, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    [HasPermission(PermissionCodes.GroupsCreate)]
    public async Task<ActionResult<GroupDto>> Create(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await _groupService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:long}")]
    [HasPermission(PermissionCodes.GroupsEdit)]
    public async Task<ActionResult<GroupDto>> Update(long id, UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:long}")]
    [HasPermission(PermissionCodes.GroupsDelete)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _groupService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
