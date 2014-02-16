using System;

namespace Sharpduino.Exceptions
{
    public class MessageHandlerException : Exception
    {
        public MessageHandlerException(string message) : base(message)
        {
        }
    }
}