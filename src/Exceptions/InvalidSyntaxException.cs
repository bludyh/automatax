using System;

namespace Automatax.Exceptions
{
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException() : base() { }
        public InvalidSyntaxException(string message) : base(message) { }
        public InvalidSyntaxException(string message, Exception innerException) : base(message, innerException) { }
    }
}
