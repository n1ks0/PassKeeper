namespace PassKeeper.Core.Operation;

public interface IOperationResult
{
    string Message { get; }
    Result Result { get; }
}