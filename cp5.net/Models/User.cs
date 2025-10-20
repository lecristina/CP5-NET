using System.ComponentModel.DataAnnotations;

namespace Cp5.Net.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    public Role Role { get; set; } = Role.Reader;
}


