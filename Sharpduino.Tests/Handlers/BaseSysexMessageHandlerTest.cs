using NUnit.Framework;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;

namespace Sharpduino.Library.Tests
{
    public abstract class BaseSysexMessageHandlerTest<T> : BaseMessageHandlerTest<T> where T : IMessageHandler
    {
        private const byte SysexStartByte = MessageConstants.SYSEX_START;

        protected abstract byte SysexCommandByte { get; }

        /// <summary>
        /// This method checks to see if the handler ignores all other sysex messages
        /// Override with the TestAttribute
        /// </summary>
        [Test]
        public virtual void Does_Not_Handle_Other_Sysex_Messages()
        {
            handler.Handle(SysexStartByte);
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if (i != SysexCommandByte)
                    Assert.IsFalse(handler.CanHandle(i));
                else
                    Assert.IsTrue(handler.CanHandle(i));
            }
        }

        /// <summary>
        /// This method checks to see if the handler throws exceptions when forced other sysex messages
        /// Override with the TestAttribute
        /// </summary>
        [Test]
        public virtual void Throws_Error_If_Forced_Other_Sysex_Message()
        {            
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                // We assume that the handler was reset
                handler.Handle(SysexStartByte);

                if (i != SysexCommandByte)
                    Assert.Throws<MessageHandlerException>(() => handler.Handle(i));
                else
                {
                    Assert.DoesNotThrow(() => handler.Handle(i));
                    // This is one way to reset the handler
                    handler = CreateHandler();
                }
            }
        }
    }
}