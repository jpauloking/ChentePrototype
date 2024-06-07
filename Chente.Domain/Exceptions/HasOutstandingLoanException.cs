namespace Chente.Domain.Exceptions;

public class HasOutstandingLoanException : Exception
{
    public HasOutstandingLoanException(string loanNumber)
    {
    }

    public HasOutstandingLoanException(string loanNumber, string? message) : base(message)
    {
    }

    public HasOutstandingLoanException(string loanNumber, string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
