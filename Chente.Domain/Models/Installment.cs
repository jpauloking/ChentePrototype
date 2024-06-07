using Chente.Domain.Exceptions;

namespace Chente.Domain.Models;

public class Installment
{
    public string InstallmentNumber { get; set; } = null!;
    public DateTime DateDue { get; set; }
    public decimal Amount { get; set; }
    public decimal BeginningBalance { get; private set; }
    public decimal EndingBalance { get; private set; }
    public decimal AmountPaid { get; private set; }
    public Loan Loan { get; set; } = default!;
    public DateTime DatePaid { get; set; }
    public decimal AmountDue => Amount - AmountPaid;
    public bool IsPaid => AmountDue == 0;
    public bool IsOverDue => !IsPaid && DateTime.Today.Subtract(DateDue) > TimeSpan.FromDays(0);
    public int DaysIsOverDueBy => IsOverDue ? DateTime.Today.Subtract(DateDue).Days : 0;

    public Installment(string installmentNumber, DateTime dateDue, decimal amount, decimal beginningBalance, decimal endingBalance, decimal amountPaid)
    {
        InstallmentNumber = installmentNumber;
        DateDue = dateDue;
        Amount = amount;
        BeginningBalance = beginningBalance;
        EndingBalance = endingBalance;
        AmountPaid = amountPaid;
    }

    public Installment(DateTime dateDue, decimal amount, decimal beginningBalance, decimal endingBalance, decimal amountPaid = 0)
    {
        DateDue = dateDue;
        Amount = amount;
        BeginningBalance = beginningBalance;
        EndingBalance = endingBalance;
        AmountPaid = amountPaid;
    }

    public decimal Pay(decimal amount, DateTime? datePaid = null!)
    {
        decimal balanceOverAmountDue = 0;
        if (IsPaid)
        {
            throw new InvalidPaymentException(InstallmentNumber, "Installment has arleady been paid");
        }
        if (amount > AmountDue)
        {
            balanceOverAmountDue = amount - AmountDue;
            amount = AmountDue;
        }
        DatePaid = datePaid ?? DateTime.Now;
        AmountPaid += amount;
        return balanceOverAmountDue;
    }

}
