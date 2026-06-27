namespace Como.CRM.Api.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<T> ExecuteAsync<T>(
            Func<CancellationToken, Task<T>> action,
            CancellationToken cancellationToken = default);
    }
}
