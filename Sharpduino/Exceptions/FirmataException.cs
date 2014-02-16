using System;

namespace Sharpduino.Exceptions
{
    public class FirmataException : Exception
    {
        public FirmataException(string message) : base(message){}
    }
}