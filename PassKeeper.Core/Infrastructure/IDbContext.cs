using System.Linq.Expressions;

namespace PassKeeper.Core.Infrastructure;

public interface IDbContext : IDisposable, IAsyncDisposable
{
    Task<T> GetByIdAsync<T>(Guid id) where T : class;
    Task AddAsync<T>(T entity) where T : class;
    Task UpdateAsync<T>(T entity) where T : class;
    Task DeleteAsync<T>(Guid id) where T : class;
    Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> predicate, bool trackChanges = false,
        params string[] includeProperties) where T : class;
    Task<T> FindFirstByConditionAsync<T>(Expression<Func<T, bool>> predicate, bool trackChanges = false,
        params string[] includeProperties) where T : class;
    IQueryable<T> GetQueryable<T>(bool trackChanges = false) where T : class;

    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
    Task<int> SaveChangesAsync();
}