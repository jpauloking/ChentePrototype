using System;
using System.Collections.Generic;
namespace Chente.Desktop.Exceptions;

internal class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException(string? message) : base(message)
    {
    }

    public AuthenticationFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
