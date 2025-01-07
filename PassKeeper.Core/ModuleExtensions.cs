using Microsoft.Extensions.DependencyInjection;
using PassKeeper.Core.Account;
using PassKeeper.Core.Security;

namespace PassKeeper.Core;

public static class ModuleExtensions
{
    public static void AddCoreModule(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAccountManager, AccountManager>();
        serviceCollection.AddSingleton<IEncode, Encoder>();
    }
}