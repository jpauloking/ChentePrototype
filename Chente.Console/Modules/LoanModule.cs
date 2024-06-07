using Chente.DataAccess;
using Chente.DataAccess.Repositories;
using Chente.DataAccess.Services;
using Chente.Domain.Models;
using static System.Console;

namespace Chente.Console.Modules;

internal class LoanModule
{
    private readonly ApplicationDbContextFactory applicationDbContextFactory;

    public LoanModule(ApplicationDbContextFactory applicationDbContextFactory)
    {
        this.applicationDbContextFactory = applicationDbContextFactory;
    }

    internal async Task DeleteLoan(int id)
    {
        // Todo - Implement delete loan. Should loan get deleted. What happens when a loan is deleted: Installments get deleted too
        LoanRepository repository = new(applicationDbContextFactory);
        await repository.DeleteAsync(id);
    }

    internal void DisplayLoan(Loan loan)
    {
        ForegroundColor = ConsoleColor.DarkBlue;
        WriteLine("\n\n\n---------------------------------");
        WriteLine("\nChente Loan\n");
        WriteLine("---------------------------------");
        WriteLine("{0,-24}{1}", "Loan number:", loan.LoanNumber);
        WriteLine("{0,-24}{1:D}", "Date opened:", loan.DateOpened);
        WriteLine("{0,-24}{1:C}", "Principal:", loan.Principal);
        WriteLine("{0,-24}{1:C}", "Amount:", loan.Amount);
        WriteLine("{0,-24}{1:C}", "AmountDue:", loan.AmountDue);
        WriteLine("{0,-24}{1}", "Installments:", loan.NumberOfInstallments);
        WriteLine("{0,-24}{1}", "Installments overdue:", loan.NumberOfInstallmentsOverDue);
        WriteLine("{0,-24}{1}", "Paid:", loan.IsPaid);
        WriteLine("{0,-24}{1}", "Overdue:", loan.IsOverDue);
        WriteLine("---------------------------------\n\n\n");
        ForegroundColor = ConsoleColor.White;
    }

    internal async Task<IEnumerable<Loan>> GetLoans()
    {
        LoanRepository repository = new(applicationDbContextFactory);
        var loans = (await repository
            .GetAsync())
            .Select(MapToLoanModel())
            .ToList();
        return loans;
    }

    internal async Task<IEnumerable<Loan>> GetLoans(Borrower borrower)
    {
        LoanRepository repository = new(applicationDbContextFactory);
        var loans = (await repository
            .GetAsync(MapToBorrowerDAO(borrower)))
            .Select(MapToLoanModel())
            .ToList();
        return loans;
    }

    internal async Task<Loan> GetLoan(int id)
    {
        LoanRepository repository = new(applicationDbContextFactory);
        DataAccess.Models.Loan loan = (await repository.GetAsync(id))!;
        return new Loan(loan.LoanNumber, loan.DateOpened, loan.Principal, loan.InterestRate, loan.DurationInDays, loan.AmountPerInstallment, loan.Installments.Select(MapToInstallmentModel()).ToList());
    }

    internal async Task UpdateLoan(Loan loan)
    {
        // Todo - Implement update loan. Should loan get updated. What is updated when loan is updated? What happens when a loan is updated: How are the installments affected by loan update?
        LoanRepository repository = new(applicationDbContextFactory);
        await repository.UpdateAsync(new DataAccess.Models.Loan
        {
            Id = DatabaseKeyManager.GetPrimaryKeyFrom(loan.LoanNumber),
            DateOpened = loan.DateOpened,
            Principal = loan.Principal,
            InterestRate = loan.InterestRate,
            DurationInDays = loan.DurationInDays,
            AmountPerInstallment = loan.AmountPerInstallment,
            Borrower = MapToBorrowerDAO(loan.Borrower)
        });
    }

    private static DataAccess.Models.Borrower MapToBorrowerDAO(Borrower borrower)
    {
        return new DataAccess.Models.Borrower
        {
            Id = DatabaseKeyManager.GetPrimaryKeyFrom(borrower.BorrowerNumber),
            FirstName = borrower.FirstName,
            LastName = borrower.LastName,
            EmailAddress = borrower.EmailAddress,
            PhoneNumber = borrower.PhoneNumber,
            Loans = borrower.Loans.Select(MapToLoanDAO()).ToList(),
        };
    }

    private static Func<DataAccess.Models.Loan, Loan> MapToLoanModel()
    {
        return l => new Loan(l.LoanNumber, l.DateOpened, l.Principal, l.InterestRate, l.DurationInDays, l.AmountPerInstallment, l.Installments.Select(MapToInstallmentModel()).ToList());
    }

    private static Func<DataAccess.Models.Installment, Installment> MapToInstallmentModel()
    {
        return i => new Installment(i.InstallmentNumber, i.DateDue, i.Amount, i.BeginningBalance, i.EndingBalance, i.AmountPaid);
    }

    private static Func<Loan, DataAccess.Models.Loan> MapToLoanDAO()
    {
        return l => new DataAccess.Models.Loan
        {
            DateOpened = l.DateOpened,
            Principal = l.Principal,
            InterestRate = l.InterestRate,
            DurationInDays = l.DurationInDays,
            AmountPerInstallment = l.AmountPerInstallment,
            Installments = l.Installments.Select(MapToInstallmentDAO()).ToList()
        };
    }

    private static Func<Installment, DataAccess.Models.Installment> MapToInstallmentDAO()
    {
        return i => new DataAccess.Models.Installment
        {
            DateDue = i.DateDue,
            Amount = i.Amount,
            AmountPaid = i.AmountPaid,
            BeginningBalance = i.BeginningBalance,
            EndingBalance = i.EndingBalance,
            DatePaid = i.DatePaid
        };
    }

}
