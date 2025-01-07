namespace PassKeeper.Core.Entities;

public interface IUser
{
    Guid Id { get; }
    string Name { get; }
    string Email { get; }
    string Password { get; }
    UserRole Role { get; }
    bool IsVerified { get; }
}