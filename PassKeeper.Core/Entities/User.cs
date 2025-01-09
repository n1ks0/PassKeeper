using System.ComponentModel.DataAnnotations.Schema;

namespace PassKeeper.Core.Entities;

public class User : IUser
{
    public User(string name, string email, string password, UserRole role)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Role = role;
    }

    /// <summary>
    /// ID
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public ICollection<Secret> Secrets { get; set; } = new List<Secret>();
}
