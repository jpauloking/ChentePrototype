using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chente.DataAccess.Models;

public class Borrower
{
    public int Id { get; set; }
    [MaxLength(10)]
    [NotMapped]
    public string BorrowerNumber => $"BOR-{Id}";
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;
    [Phone]
    public string? PhoneNumber { get; set; }
    // Removed to prevent StackOverflowException when uaing AutoMapper
    // public ICollection<Loan> Loans { get; set; } = [];
}
