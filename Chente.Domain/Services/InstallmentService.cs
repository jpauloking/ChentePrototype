using Chente.Domain.Models;

namespace Chente.Domain.Services;

internal class InstallmentService
{
    private readonly Loan loan;

    public InstallmentService(Loan loan)
    {
        this.loan = loan;
    }

    public IEnumerable<Installment> CreateInstallments()
    {
        decimal runningBalance = GetAmountDue();
        int daysBetweenInstallments = GetDaysBetweenInstallments();
        int numberOfInstallmentsCreated = 0;
        int daysSinceDateOpened = 0;
        List<Installment> installments = CreateInstallmentsForLoan(runningBalance, daysBetweenInstallments, numberOfInstallmentsCreated, daysSinceDateOpened);
        return installments;
    }

    private List<Installment> CreateInstallmentsForLoan(decimal runningBalance, int daysBetweenInstallments, int numberOfInstallmentsCreated, int daysSinceDateOpened)
    {
        List<Installment> installments = [];
        while (runningBalance > 0)
        {
            DateTime dateDue = loan.DateOpened.AddDays(daysSinceDateOpened);
            Installment installment = default!;
            if (runningBalance > loan.AmountPerInstallment)
            {
                decimal amount = loan.AmountPerInstallment;
                decimal beginningBalance = runningBalance;
                decimal endingBalance = runningBalance -= loan.AmountPerInstallment;
                installment = CreateInstallment(dateDue, amount, beginningBalance, endingBalance);
            }
            else
            {
                decimal beginningBalance = Math.Round(runningBalance, 4);
                decimal amount = beginningBalance;
                decimal endingBalance = runningBalance = 0;
                installment = CreateInstallment(dateDue, amount, beginningBalance, endingBalance);
            }
            installments.Add(installment);
            daysSinceDateOpened += daysBetweenInstallments;
            ++numberOfInstallmentsCreated;
        }
        return installments;
    }

    private Installment CreateInstallment( DateTime dateDue, decimal amount, decimal beginningBalance, decimal endingBalance)
    {
        return new Installment(dateDue, amount, beginningBalance, endingBalance);
    }

    private int GetDaysBetweenInstallments()
    {
        int numberOfInstallments = GetNumberOfInstallments();
        return loan.DurationInDays / numberOfInstallments;
    }

    private int GetNumberOfInstallments()
    {
        decimal amountDue = GetAmountDue();
        return (int)Math.Ceiling(amountDue / loan.AmountPerInstallment);
    }

    private decimal GetAmountDue()
    {
        decimal interestAccrued = GetInterestAccrued();
        return interestAccrued + loan.Principal;
    }

    private decimal GetInterestAccrued()
    {
        decimal dailyInterest = GetDailyInterest();
        return dailyInterest * loan.DurationInDays;
    }

    private decimal GetDailyInterest()
    {
        double oneDivideByThirty = 1D / 30D;
        decimal oneDivideByThirtyInDecimal = (decimal)oneDivideByThirty;
        double interestRateAsPercentage = loan.InterestRate * 0.01;
        decimal interestRateAsPercentageInDecimal = (decimal)interestRateAsPercentage;
        return oneDivideByThirtyInDecimal * loan.Principal * interestRateAsPercentageInDecimal;
    }
}
