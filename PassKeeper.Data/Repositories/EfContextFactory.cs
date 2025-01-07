using Microsoft.EntityFrameworkCore;
using PassKeeper.Core.Infrastructure;

namespace PassKeeper.Data.Repositories;

public class EfContextFactory(IDbContextFactory<PostgresContext> dbContextFactory) : DbContextFactory
{
    public override async Task<IDbContext> CreateDbContextAsync()
    {
        return await dbContextFactory.CreateDbContextAsync();
    }
}