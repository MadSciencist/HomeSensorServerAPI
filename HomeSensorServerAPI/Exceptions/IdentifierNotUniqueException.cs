using System;

namespace HomeSensorServerAPI.Exceptions
{
    public class IdentifierNotUniqueException : Exception
    {
        public IdentifierNotUniqueException()
        {
        }

        public IdentifierNotUniqueException(string message)
            : base(message)
        {
        }

        public IdentifierNotUniqueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
