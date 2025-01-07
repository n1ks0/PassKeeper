using PassKeeper.Core.Operation;

namespace PassKeeper.Core.Secrets;

public interface ISecretOperation
{
    Task<IOperationResult> Add(Guid userId, string key, string login, string password);
    Task<IOperationResult> Delete(Guid userId, string key);
    Task<IOperationResult> Update(Guid userId, string key, string login, string password);
}