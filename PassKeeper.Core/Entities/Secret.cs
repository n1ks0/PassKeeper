using System.ComponentModel.DataAnnotations.Schema;
using PassKeeper.Core.Secrets;

namespace PassKeeper.Core.Entities;

public class Secret : ISecret
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Key { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}