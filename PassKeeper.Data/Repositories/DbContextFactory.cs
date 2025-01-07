using Microsoft.Extensions.Configuration;
using PassKeeper.Core.Infrastructure;

namespace PassKeeper.Data.Repositories;

public abstract class DbContextFactory : IDbContextFactory
{
    public abstract Task<IDbContext> CreateDbContextAsync();
}