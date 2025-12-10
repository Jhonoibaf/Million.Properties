using AutoMapper;

namespace Million.Properties.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IMapper Mapper { get; }
    IPropertyRepository PropertyRepository { get; }
    IPropertyImageRepository PropertyImageRepository { get; }
    Task AddRangeEntity<T>(IEnumerable<T> entities) where T : class;
    Task RemoveRangeEntity<T>(IEnumerable<T> entities) where T : class;
    Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation);
}
