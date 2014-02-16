using System;

namespace Sharpduino.Exceptions
{
    public class InvalidFirmataMessageException : Exception
    {
        public InvalidFirmataMessageException(string message) : base(message)
        {
            
        }
    }
}