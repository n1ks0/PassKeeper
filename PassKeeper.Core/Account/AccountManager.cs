using PassKeeper.Core.Entities;
using PassKeeper.Core.Infrastructure;
using PassKeeper.Core.Operation;

namespace PassKeeper.Core.Account;

public class AccountManager(IDbContextFactory contextFactory) : IAccountManager
{
    public async Task<IOperationResult> LoginAsync(string email, string password)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var user = await dbContext
            .FindFirstByConditionAsync<User>(x => x.Email.ToLower() == email && x.Password == password);
        return user == null ? OperationResult.Fail("Invalid email or password") : OperationResult.Success();
    }

    public async Task<IOperationResult> RegisterAsync(string username, string password, string email)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var user = await GetUserByEmailAsync(dbContext, email);

        if (user != null)
            return OperationResult.Fail("User already exists");

        user = new User(username, email, password, UserRole.User);

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }

    public async Task<IOperationResult> ChangePasswordAsync(string email, string oldPassword, string newPassword)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var user = await GetUserByEmailAsync(dbContext, email);

        if (user == null)
            return OperationResult.Fail("User doesn't exists");
        
        if (user.Password != oldPassword)
            return OperationResult.Fail("Wrong old password");
            
        user.Password = newPassword;
        await dbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }

    public Task<IOperationResult> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<User> GetUserByEmailAsync(IDbContext dbContext, string email)
    {
        return await dbContext.FindFirstByConditionAsync<User>(x => x.Email.ToLower() == email.ToLower(), true);
    }
}