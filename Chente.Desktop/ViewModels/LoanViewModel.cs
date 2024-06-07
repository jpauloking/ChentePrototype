using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Chente.Desktop.ViewModels;

internal class LoanViewModel
{
    [MaxLength(10)]
    public string LoanNumber { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime DateOpened { get; set; }
    [DataType(DataType.Currency)]
    [Precision(16, 4)]
    public decimal Principal { get; set; }
    [Range(0, Int16.MaxValue)]
    public double InterestRate { get; set; }
    [Range(0, Int16.MaxValue)]
    public int DurationInDays { get; set; }
    [Range(0, Int16.MaxValue)]
    [Precision(16, 4)]
    public decimal AmountPerInstallment { get; set; }
    public BorrowerViewModel Borrower { get; set; } = null!;
    //public int NumberOfInstallments { get; set; }
    //public decimal Amount { get; set; }
    //public decimal AmountDue { get; set; }
    //public bool IsPaid { get; set; }
    //public DateTime DatePaid { get; set; }
    //public bool IsOverDue { get; set; }
    //public int NumberOfInstallmentsOverDue { get; set; }
}
