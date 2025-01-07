using PassKeeper.Core.Entities;
using PassKeeper.Core.Infrastructure;
using PassKeeper.Core.Operation;
using PassKeeper.Core.Security;

namespace PassKeeper.Core.Account;

public class AccountManager(IDbContextFactory contextFactory, IEncode encoder) : IAccountManager
{
    public async Task<IOperationResult> LoginAsync(string email, string password)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var encodedPassword = await encoder.EncodeAsync(password).ConfigureAwait(false);
        var userQuery = dbContext
            .FindByCondition<User>(x => x.Email.ToLower() == email && x.Password == encodedPassword);
        var user = await dbContext.FirstOrDefaultAsync(userQuery).ConfigureAwait(false);
        return user == null ? OperationResult.Fail("Invalid email or password") : OperationResult.Success();
    }

    public async Task<IOperationResult> RegisterAsync(string username, string password, string email)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var user = await GetUserByEmailAsync(dbContext, email).ConfigureAwait(false);

        if (user != null)
            return OperationResult.Fail("User already exists");

        var encodedPassword = await encoder.EncodeAsync(password).ConfigureAwait(false);
        
        user = new User(username, email, encodedPassword, UserRole.User);

        await dbContext.AddAsync(user).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        
        return OperationResult.Success();
    }

    public async Task<IOperationResult> ChangePasswordAsync(string email, string oldPassword, string newPassword)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var user = await GetUserByEmailAsync(dbContext, email).ConfigureAwait(false);

        if (user == null)
            return OperationResult.Fail("User doesn't exists");
        
        var encodedOldPassword = await encoder.EncodeAsync(oldPassword).ConfigureAwait(false);
        
        if (user.Password != encodedOldPassword)
            return OperationResult.Fail("Wrong old password");
        
        var encodedNewPassword = await encoder.EncodeAsync(newPassword).ConfigureAwait(false);
        user.Password = encodedNewPassword;
        
        await dbContext.SaveChangesAsync();
        
        return OperationResult.Success();
    }

    public Task<IOperationResult> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<User> GetUserByEmailAsync(IDbContext dbContext, string email)
    {
        var userQuery = dbContext.FindByCondition<User>(x => x.Email.ToLower() == email.ToLower(), true);
        return await dbContext.FirstOrDefaultAsync(userQuery).ConfigureAwait(false);
    }
}