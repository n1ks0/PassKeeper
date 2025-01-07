namespace PassKeeper.Core.Infrastructure;

public interface IDbContextFactory
{
    Task<IDbContext> CreateDbContextAsync();
}