using Como.CRM.Api.Data;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Groups;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Como.CRM.Api.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly AppDbContext _db;
    private readonly ICurrentTenantService _tenant;
    private readonly ICurrentUserService _user;

    public GroupService(AppDbContext db, ICurrentTenantService tenant, ICurrentUserService user)
    {
        _db = db;
        _tenant = tenant;
        _user = user;
    }

    public async Task<List<GroupDto>> GetAllAsync(bool? isProduct, CancellationToken cancellationToken = default)
    {
        var query = _db.ProductServiceGroups.AsNoTracking().AsQueryable();

        if (isProduct.HasValue)
            query = query.Where(x => x.IsProduct == isProduct.Value);

        return await query
            .OrderBy(x => x.GroupName)
            .Select(x => new GroupDto
            {
                Id = x.Id,
                Code = x.Code,
                GroupName = x.GroupName,
                IsProduct = x.IsProduct,
                CreateDate = x.CreateDate
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<GroupDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductServiceGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Group not found.");

        return ToDto(entity);
    }

    public async Task<GroupDto> CreateAsync(CreateGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new ProductServiceGroup
        {
            TenantId = _tenant.TenantId,
            Code = request.Code.Trim(),
            GroupName = request.GroupName.Trim(),
            IsProduct = request.IsProduct,
            CreateUserId = _user.UserId
        };

        _db.ProductServiceGroups.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task<GroupDto> UpdateAsync(long id, UpdateGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductServiceGroups.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Group not found.");

        entity.Code = request.Code.Trim();
        entity.GroupName = request.GroupName.Trim();
        entity.IsProduct = request.IsProduct;

        await _db.SaveChangesAsync(cancellationToken);
        return ToDto(entity);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ProductServiceGroups.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException("Group not found.");

        entity.IsRemove = true;
        entity.RemoveDate = DateTime.UtcNow;
        entity.RemoveUserId = _user.UserId;

        await _db.SaveChangesAsync(cancellationToken);
    }

    private static GroupDto ToDto(ProductServiceGroup x) => new()
    {
        Id = x.Id,
        Code = x.Code,
        GroupName = x.GroupName,
        IsProduct = x.IsProduct,
        CreateDate = x.CreateDate
    };
}
