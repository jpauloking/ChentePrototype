using Chente.DataAccess;
using Chente.DataAccess.Repositories;
using Chente.DataAccess.Services;
using Chente.Domain.Models;
using Chente.Domain.Exceptions;
using static System.Console;

namespace Chente.Console.Modules;

internal class BorrowerModule
{
    private readonly ApplicationDbContextFactory applicationDbContextFactory;

    public BorrowerModule(ApplicationDbContextFactory applicationDbContextFactory)
    {
        this.applicationDbContextFactory = applicationDbContextFactory;
    }

    internal async Task AddLoanToBorrower(Borrower borrower, Loan loan)
    {
        try
        {
            borrower.AddLoan(loan);
            await UpdateBorrower(borrower);
        }
        catch (HasOutstandingLoanException)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine("Failed to add loan. Borrower has outstanding loan. Please clear the outstanding loan before opening a new one.");
            ForegroundColor = ConsoleColor.White;
        }
    }

    internal async Task CreateBorrower(Borrower borrower)
    {
        BorrowerRepository repository = new(applicationDbContextFactory);
        await repository.CreateAsync(MapToBorrowerDAO(borrower));
    }

    internal async Task DeleteBorrower(int id)
    {
        // Todo - Implement delete borrower. Should loans get deleted too? What happens when a loan is deleted: Installments get deleted too.
        BorrowerRepository repository = new(applicationDbContextFactory);
        await repository.DeleteAsync(id);
    }

    internal void DisplayBorrower(Borrower borrower)
    {
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("\n\n\n---------------------------------");
        WriteLine("\nChente Borrower\n");
        WriteLine("---------------------------------");
        WriteLine("{0,-16}{1}", "Number:", borrower.BorrowerNumber);
        WriteLine("{0,-16}{1} {2}", "Name:", borrower.FirstName, borrower.LastName);
        WriteLine("{0,-16}{1}", "Email address:", borrower.EmailAddress);
        WriteLine("{0,-16}{1}", "Phone number:", borrower.PhoneNumber);
        WriteLine("---------------------------------\n\n\n");
        ForegroundColor = ConsoleColor.White;
    }

    internal async Task<IEnumerable<Borrower>> GetBorrowers()
    {
        BorrowerRepository repository = new(applicationDbContextFactory);
        var borrowers = (await repository
            .GetAsync())
            .Select(MapToBorrowerModel)
            .ToList();
        return borrowers;
    }

    internal async Task<Borrower> GetBorrower(int id)
    {
        BorrowerRepository repository = new(applicationDbContextFactory);
        DataAccess.Models.Borrower borrower = (await repository.GetAsync(id))!;
        return MapToBorrowerModel(borrower);
    }

    internal async Task UpdateBorrower(Borrower borrower)
    {
        // Todo - Implement update borrower.
        BorrowerRepository repository = new(applicationDbContextFactory);
        await repository.UpdateAsync(MapToBorrowerDAO(borrower));
    }

    private static Borrower MapToBorrowerModel(DataAccess.Models.Borrower borrower)
    {
        return new Borrower(borrower.BorrowerNumber, borrower.FirstName, borrower.LastName, borrower.EmailAddress, borrower.PhoneNumber, borrower.Loans
            .Select(l => MapToLoanModel(l)).ToList());
    }

    private static Loan MapToLoanModel(DataAccess.Models.Loan loan)
    {
        return new Loan(loan.LoanNumber, loan.DateOpened, loan.Principal, loan.InterestRate, loan.DurationInDays, loan.AmountPerInstallment, loan.Installments.Select(i => MapToInstallmentModel(i)).ToList());
    }

    private static Installment MapToInstallmentModel(DataAccess.Models.Installment installment)
    {
        return new Installment(installment.InstallmentNumber, installment.DateDue, installment.Amount, installment.BeginningBalance, installment.EndingBalance, installment.AmountPaid);
    }

    private static DataAccess.Models.Borrower MapToBorrowerDAO(Borrower borrower)
    {
        return new DataAccess.Models.Borrower
        {
            Id = !string.IsNullOrEmpty(borrower.BorrowerNumber) ? DatabaseKeyManager.GetPrimaryKeyFrom(borrower.BorrowerNumber) : 0,
            FirstName = borrower.FirstName,
            LastName = borrower.LastName,
            EmailAddress = borrower.EmailAddress,
            PhoneNumber = borrower.PhoneNumber,
            Loans = borrower.Loans.Select(l => MapToLoanDAO(l)).ToList(),
        };
    }

    private static DataAccess.Models.Loan MapToLoanDAO(Loan loan)
    {
        return new DataAccess.Models.Loan
        {
            Id = !string.IsNullOrEmpty(loan.LoanNumber) ? DatabaseKeyManager.GetPrimaryKeyFrom(loan.LoanNumber) : 0,
            DateOpened = loan.DateOpened,
            Principal = loan.Principal,
            InterestRate = loan.InterestRate,
            DurationInDays = loan.DurationInDays,
            AmountPerInstallment = loan.AmountPerInstallment,
            Installments = loan.Installments.Select(i => MapToInstallmentDAO(i)).ToList()
        };
    }

    private static DataAccess.Models.Installment MapToInstallmentDAO(Installment installment)
    {
        return new DataAccess.Models.Installment
        {
            Id = !string.IsNullOrEmpty(installment.InstallmentNumber) ? DatabaseKeyManager.GetPrimaryKeyFrom(installment.InstallmentNumber) : 0,
            DateDue = installment.DateDue,
            Amount = installment.Amount,
            AmountPaid = installment.AmountPaid,
            BeginningBalance = installment.BeginningBalance,
            EndingBalance = installment.EndingBalance,
            DatePaid = installment.DatePaid
        };
    }
}
