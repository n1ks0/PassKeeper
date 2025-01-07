using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PassKeeper.Core.Entities;
using PassKeeper.Core.Infrastructure;

namespace PassKeeper.Data.Repositories;

public class PostgresContext : DbContext, IDbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Secret> Secrets { get; set; }
    
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetAssembly(typeof(PostgresContext))
                ?? throw new InvalidOperationException("Could not find assembly PostgresContext"));
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : class
    {
        return await base.FindAsync<T>(id);
    }

    public async Task AddAsync<T>(T entity) where T : class
    {
        await Set<T>().AddAsync(entity);
    }

    public Task UpdateAsync<T>(T entity) where T : class
    {
        Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync<T>(Guid id) where T : class
    {
        var entity = await GetByIdAsync<T>(id);
        if (entity != null)
            Set<T>().Remove(entity);
    }

    public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
    {
        return await base.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> predicate,
        bool trackChanges = false, params string[] includeProperties)
        where T : class
    {
        var set = trackChanges ? Set<T>() : Set<T>().AsNoTracking();
        if (includeProperties != null) 
            set = includeProperties.Aggregate(set, (current, prop) => current.Include(prop));
        return set.Where(predicate);
    }

    public IQueryable<T> GetQueryable<T>(bool trackChanges = false) where T : class
    {
        return trackChanges ? Set<T>() : Set<T>().AsNoTracking();
    }

    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return await query.ToListAsync();
    }
    
    public async Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
    {
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}