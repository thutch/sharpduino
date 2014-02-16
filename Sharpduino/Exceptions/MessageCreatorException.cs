using System;

namespace Sharpduino.Exceptions
{
    public class MessageCreatorException : Exception
    {
        public MessageCreatorException(string message) : base(message){}
    }
}
