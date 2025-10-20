using System.ComponentModel.DataAnnotations;

namespace Cp5.Net.Models;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public Guid UserId { get; set; }
}


