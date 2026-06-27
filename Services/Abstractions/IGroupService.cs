using Como.CRM.Api.DTOs.Groups;

namespace Como.CRM.Api.Services.Abstractions;

public interface IGroupService
{
    Task<List<GroupDto>> GetAllAsync(bool? isProduct, CancellationToken cancellationToken = default);
    Task<GroupDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateAsync(CreateGroupRequest request, CancellationToken cancellationToken = default);
    Task<GroupDto> UpdateAsync(long id, UpdateGroupRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
