namespace PassKeeper.Core.Operation;

public struct OperationResult : IOperationResult
{
    public string Message { get; set; }
    public Result Result { get; set; }

    public static OperationResult Success() => new OperationResult { Result = Result.Success };

    public static OperationResult Fail(string message) =>
        new OperationResult { Result = Result.Fail, Message = message };
}