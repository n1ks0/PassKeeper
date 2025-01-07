using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PassKeeper.Core.Infrastructure;
using PassKeeper.Data.Repositories;

namespace PassKeeper.Data;

public static class ModuleExtensions
{
    public static void AddDataModule(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddPooledDbContextFactory<PostgresContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });
        serviceCollection.AddSingleton<IDbContextFactory, DbContextFactory>();
    }
}