using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chente.DataAccess.Models;

public class Installment
{
    public int Id { get; set; }
    [MaxLength(10)]
    [NotMapped]
    public string InstallmentNumber => $"INS-{Id}";
    [DataType(DataType.Date)]
    public DateTime DateDue { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16,4)]
    public decimal Amount { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal BeginningBalance { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal EndingBalance { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal AmountPaid { get; set; }
    [DataType(DataType.Date)]
    public DateTime DatePaid { get; set; }
    public Loan Loan { get; set; } = default!;
}
