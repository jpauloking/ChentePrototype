using Chente.Domain.Exceptions;

namespace Chente.Domain.Models;

public class Borrower
{
    public string BorrowerNumber { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public List<Loan> Loans { get; private set; } = [];

    public Borrower(string borrowerNumber, string firstName, string lastName, string emailAddress, string? phoneNumber, List<Loan> loans)
    {
        BorrowerNumber = borrowerNumber;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Loans = loans;
    }

    public Borrower(string firstName, string lastName, string emailAddress, string? phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
    }
    
    public Borrower(string firstName, string lastName, string emailAddress, string? phoneNumber, List<Loan> loans)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Loans = loans;
    }

    public void AddLoan(Loan loan)
    {
        Loan? loanFromDatabase = GetOutstandingLoanIfAny();
        bool hasOutstandingLoan = loanFromDatabase is not null;
        if (hasOutstandingLoan)
        {
            throw new HasOutstandingLoanException(loanFromDatabase!.LoanNumber, "Has outstanding loan. Cannot add loan if borrower has outstanding loan.");
        }
        else
        {
            Loan? loanFromListOfBorrowersLoans = GetLoanFromListOfBorrowersLoansIfAny(loan);
            bool isLoanAdded = loanFromListOfBorrowersLoans is not null;
            if (!isLoanAdded)
            {
                loan.Borrower = this;
                Loans.Add(loan);
            }
        }
    }

    private Loan? GetLoanFromListOfBorrowersLoansIfAny(Loan loan)
    {
        return Loans.FirstOrDefault(l => l.LoanNumber == loan.LoanNumber);
    }

    private Loan? GetOutstandingLoanIfAny()
    {
        return Loans.FirstOrDefault(l => !(l.IsPaid));
    }

}
