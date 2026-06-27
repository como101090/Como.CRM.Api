namespace Como.CRM.Api.Domain.Entities;

public interface IHasTenant
{
    long TenantId { get; set; }
}
