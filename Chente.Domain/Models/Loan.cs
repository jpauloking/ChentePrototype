using Chente.Domain.Services;

namespace Chente.Domain.Models;

public class Loan
{
    private readonly IEnumerable<Installment> installments = [];

    public IEnumerable<Installment> Installments => installments;

    public string LoanNumber { get; set; } = null!;
    public DateTime DateOpened { get; set; }
    public decimal Principal { get; set; }
    public double InterestRate { get; set; }
    public int DurationInDays { get; set; }
    public decimal AmountPerInstallment { get; set; }
    public Borrower Borrower { get; set; } = default!;
    public int NumberOfInstallments => Installments.Count();
    public decimal Amount => Installments.Sum(i => i.Amount);
    public decimal AmountDue => Installments.Where(i => !(i.IsPaid)).Sum(i => i.Amount);
    public bool IsPaid => Installments.All(i => i.IsPaid);
    public DateTime DatePaid => IsPaid ? Installments.Max(i => i.DatePaid) : default!;
    public bool IsOverDue => Installments.Any(i => i.IsOverDue);
    public int NumberOfInstallmentsOverDue => Installments.Count(i => i.IsOverDue);

    public Loan(DateTime dateOpened, decimal principal, double interestRate, int durationInDays, decimal amountPerInstallment)
    {
        DateOpened = dateOpened;
        Principal = principal;
        InterestRate = interestRate;
        DurationInDays = durationInDays;
        AmountPerInstallment = amountPerInstallment;
        installments = CreateInstallments();
    }

    public Loan(string loanNumber, DateTime dateOpened, decimal principal, double interestRate, int durationInDays, decimal amountPerInstallment, List<Installment> installments)
    {
        LoanNumber = loanNumber;
        DateOpened = dateOpened;
        Principal = principal;
        InterestRate = interestRate;
        DurationInDays = durationInDays;
        AmountPerInstallment = amountPerInstallment;
        this.installments = installments;
    }

    public Loan(Borrower borrower, string loanNumber, DateTime dateOpened, decimal principal, double interestRate, int durationInDays, decimal amountPerInstallment, List<Installment> installments)
    {
        Borrower = borrower;
        LoanNumber = loanNumber;
        DateOpened = dateOpened;
        Principal = principal;
        InterestRate = interestRate;
        DurationInDays = durationInDays;
        AmountPerInstallment = amountPerInstallment;
        this.installments = installments;
    }

    private IEnumerable<Installment> CreateInstallments()
    {
        InstallmentService installmentService = new(this);
        return installmentService.CreateInstallments();
    }

}
