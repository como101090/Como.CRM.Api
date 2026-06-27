using Como.CRM.Api.Data;
using Como.CRM.Api.Services.Abstractions;

namespace Como.CRM.Api.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;

    public TransactionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<T> ExecuteAsync<T>(
    Func<CancellationToken, Task<T>> action,
    CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}