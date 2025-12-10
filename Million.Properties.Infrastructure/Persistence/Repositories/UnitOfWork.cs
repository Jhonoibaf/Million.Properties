using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Million.Properties.Application.Contracts.Persistence;


namespace Million.Properties.Infrastructure.Persistence.Repositories;

public class UnitOfWork(PropertiesDbContext context, IMapper mapper): IUnitOfWork , IAsyncDisposable
{
    #region Repositories
    public IPropertyRepository PropertyRepository => new PropertyRepository(context);
    public IPropertyImageRepository PropertyImageRepository => new PropertyImageRepository(context);
    #endregion

    #region Services
    public IMapper Mapper => mapper;

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            var currentTransaction = context.Database.CurrentTransaction;

            if (currentTransaction != null)
            {
                return await operation();
            }

            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    public async Task AddRangeEntity<T>(IEnumerable<T> entities) where T : class
    {
        context.Set<T>().AddRange(entities);
        await context.SaveChangesAsync();
    }

    public async Task RemoveRangeEntity<T>(IEnumerable<T> entities) where T : class
    {
        context.RemoveRange(entities);

        await context.SaveChangesAsync();
    }
    #endregion

    public void Dispose()
    {
        context.Dispose();
    }
    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }

}
