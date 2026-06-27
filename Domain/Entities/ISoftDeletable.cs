namespace Como.CRM.Api.Domain.Entities;

public interface ISoftDeletable
{
    bool IsRemove { get; set; }
}
