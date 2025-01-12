namespace PassKeeper.Core.Operation;

public struct OperationResult : IOperationResult
{
    public string Message { get; private init; }
    public Result Result { get; private init; }

    public static OperationResult Success() => new() { Result = Result.Success };

    public static OperationResult Fail(string message) =>
        new OperationResult { Result = Result.Fail, Message = message };
}