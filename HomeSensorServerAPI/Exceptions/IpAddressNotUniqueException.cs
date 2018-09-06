using System;

namespace HomeSensorServerAPI.Exceptions
{
    public class IpAddressNotUniqueException : Exception
    {
        public IpAddressNotUniqueException()
        {
        }

        public IpAddressNotUniqueException(string message)
            : base(message)
        {
        }

        public IpAddressNotUniqueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
