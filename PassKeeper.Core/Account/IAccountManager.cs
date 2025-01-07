using PassKeeper.Core.Operation;

namespace PassKeeper.Core.Account;

public interface IAccountManager
{
    Task<IOperationResult> LoginAsync(string email, string password);
    Task<IOperationResult> RegisterAsync(string username, string password, string email);
    Task<IOperationResult> ChangePasswordAsync(string email, string oldPassword, string newPassword);
    Task<IOperationResult> LogoutAsync();
}