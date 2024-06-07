namespace Chente.Domain.Exceptions;

public class InvalidPaymentException : Exception
{
    public InvalidPaymentException(string installmentNumber)
    {
    }

    public InvalidPaymentException(string installmentNumber, string? message) : base(message)
    {
    }

    public InvalidPaymentException(string installmentNumber, string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
