using System.ComponentModel.DataAnnotations;

namespace Chente.DataAccess.Models;
public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    public string Username { get; set; } = null!;
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;
    [Phone]
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = null!;
    [MaxLength(50)]
    public string Role { get; set; } = null!;
}
