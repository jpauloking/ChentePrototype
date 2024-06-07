using Chente.DataAccess;
using Chente.DataAccess.Repositories;
using Chente.DataAccess.Services;
using Chente.Domain.Exceptions;
using Chente.Domain.Models;
using static System.Console;

namespace Chente.Console.Modules;

internal class InstallmentModule
{
    private readonly ApplicationDbContextFactory applicationDbContextFactory;

    public InstallmentModule(ApplicationDbContextFactory applicationDbContextFactory)
    {
        this.applicationDbContextFactory = applicationDbContextFactory;
    }

    internal void DisplayLoanInstallments(List<Installment> installments)
    {
        ForegroundColor = ConsoleColor.Magenta;
        WriteLine("\n\n\n---------------------------------");
        WriteLine("\nChente Installments\n");
        WriteLine("---------------------------------");
        foreach (var i in installments)
        {
            WriteLine("{0,-16}{1}", "Number:", i.InstallmentNumber);
            WriteLine("{0,-16}{1:d}", "Date Due:", i.DateDue);
            WriteLine("{0,-16}{1:C}", "Amount:", i.Amount);
            WriteLine("{0,-16}{1:C}", "Begin Bal:", i.BeginningBalance);
            WriteLine("{0,-16}{1:C}", "Amount Due:", i.AmountDue);
            WriteLine("{0,-16}{1:C}", "Amount Paid:", i.AmountPaid);
            WriteLine("{0,-16}{1:C}", "End Bal:", i.EndingBalance);
            ForegroundColor = ConsoleColor.Green;
            WriteLine("{0,-16}{1}", "Is Paid:", i.IsPaid);
            ForegroundColor = ConsoleColor.Red;
            WriteLine("{0,-16}{1}", "Is overdue:", i.IsOverDue);
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("{0,-16}{1} days", "Overdue by:", i.DaysIsOverDueBy);
            ForegroundColor = ConsoleColor.Magenta;
            WriteLine("---");
        }
        ForegroundColor = ConsoleColor.White;
    }

    internal async Task<IEnumerable<Installment>> GetInstallments()
    {
        InstallmentRepository repository = new(applicationDbContextFactory);
        List<Installment> installments = (await repository.GetAsync()).Select(MapToLoanModel()).ToList();
        return installments;
    }

    internal async Task<IEnumerable<Installment>> GetInstallments(Loan loan)
    {
        InstallmentRepository repository = new(applicationDbContextFactory);
        IEnumerable<DataAccess.Models.Installment> installmentsFromDatabase = (await repository.GetAsync(MapToLoanDAO(loan)));
        List<Installment> installments = installmentsFromDatabase.Select(i => new Installment(i.InstallmentNumber, i.DateDue, i.Amount, i.BeginningBalance, i.EndingBalance, i.AmountPaid)).ToList();
        return installments;
    }

    internal async Task<Installment> GetInstallment(int id)
    {
        InstallmentRepository repository = new(applicationDbContextFactory);
        DataAccess.Models.Installment installment = (await repository.GetAsync(id))!;
        return new Installment(installment.InstallmentNumber, installment.DateDue, installment.Amount, installment.BeginningBalance, installment.EndingBalance, installment.AmountPaid);
    }

    internal async void PayInstallment(Installment installment, decimal amountOfPayment, DateTime dateOfPayment)
    {
        try
        {
            decimal balanceOverAmountDue = installment.Pay(amountOfPayment, dateOfPayment);
            InstallmentRepository repository = new(applicationDbContextFactory);
            await repository.UpdateAsync(new DataAccess.Models.Installment
            {
                Id = DatabaseKeyManager.GetPrimaryKeyFrom(installment.InstallmentNumber),
                DateDue = installment.DateDue,
                Amount = installment.Amount,
                AmountPaid = installment.AmountPaid,
                DatePaid = installment.DatePaid,
                BeginningBalance = installment.BeginningBalance,
                EndingBalance = installment.EndingBalance,
            });
            if (balanceOverAmountDue > 0)
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("{0,-16} {1}", "Amount returned to borrower:", balanceOverAmountDue);
                ForegroundColor = ConsoleColor.White;
            }
        }
        catch (InvalidPaymentException)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Failed to pay installment. The installment was already paid. You can only pay unpaid installments");
            if (OperatingSystem.IsWindows())
            {
                Beep(3000, 1);
            }
            ForegroundColor = ConsoleColor.White;
        }
    }

    private static DataAccess.Models.Loan MapToLoanDAO(Loan loan)
    {
        return new DataAccess.Models.Loan
        {
            Id = DatabaseKeyManager.GetPrimaryKeyFrom(loan.LoanNumber),
            DateOpened = loan.DateOpened,
            Principal = loan.Principal,
            InterestRate = loan.InterestRate,
            DurationInDays = loan.DurationInDays,
            AmountPerInstallment = loan.AmountPerInstallment,
        };
    }

    private static Func<DataAccess.Models.Installment, Installment> MapToLoanModel()
    {
        return i => new Installment(i.InstallmentNumber, i.DateDue, i.Amount, i.BeginningBalance, i.EndingBalance, i.AmountPaid);
    }
}
