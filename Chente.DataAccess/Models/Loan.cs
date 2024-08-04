using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chente.DataAccess.Models;

public class Loan
{
    public int Id { get; set; }
    [MaxLength(10)]
    [NotMapped]
    public string LoanNumber => $"LON-{Id}";
    [DataType(DataType.Date)]
    public DateTime DateOpened { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16,4)]
    public decimal Principal { get; set; }
    [Range(0, Int16.MaxValue)]
    public double InterestRate { get; set; }
    [Range(0, Int16.MaxValue)]
    public int DurationInDays { get; set; }
    [Range(0, Int16.MaxValue)]
    [Precision(16,4)]
    public decimal AmountPerInstallment { get; set; }
    public int BorrowerId { get; set; }
    public Borrower Borrower { get; set; } = default!;
    public ICollection<Installment> Installments { get; set; } = [];
}
