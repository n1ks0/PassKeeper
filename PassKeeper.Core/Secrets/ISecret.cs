namespace PassKeeper.Core.Secrets;

public interface ISecret
{
    Guid Id { get; }
    Guid UserId { get; }
    string Key { get; }
    string Login { get; }
    string Password { get; }
}