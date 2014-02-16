using System.Linq;
using Moq;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;

namespace Sharpduino.Library.Tests
{
    public abstract class BaseMessageHandlerTest<T> where T : IMessageHandler
    {
        protected Mock<IMessageBroker> mockBroker;
        protected T handler;

        /// <summary>
        /// Method that should be overriden to create the handler that we want to test
        /// </summary>
        protected abstract T CreateHandler();
		
        /// <summary>
        /// Override this if you want a different setup than having
        /// a new mockbroker and instance of the handler for each test
        /// </summary>
        [SetUp]
        public virtual void SetupEachTest()
        {
            mockBroker = new Mock<IMessageBroker>();
            handler = CreateHandler();
        }

        /// <summary>
        /// This method checks to see if the handler ignores all other messages
        /// Override with the TestAttribute
        /// </summary>
        [Test]
        public virtual void Ignores_All_Other_Messages()
        {
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if (i != handler.START_MESSAGE)
                    Assert.IsFalse(handler.CanHandle(i));
                else
                    Assert.IsTrue(handler.CanHandle(i));
            }
        }

        /// <summary>
        /// This method checks to see if the handler throws exceptions when forced other messages
        /// Override with the TestAttribute
        /// </summary>
        [Test]
        public virtual void Throws_Error_If_Forced_Other_Message()
        {
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if (i != handler.START_MESSAGE)
                    Assert.Throws<MessageHandlerException>(() => handler.Handle(i));
                else
                    Assert.DoesNotThrow(() => handler.Handle(i));
            }
        }

        /// <summary>
        /// This method asserts if the handler resets after a successful message
        /// This is asserted by feeding a new message after the successful
        /// </summary>
        /// <param name="message">A byte array with the message</param>
        protected void Test_Handler_Resets_Successfully(byte[] message)
        {
            for (int i = 0; i < message.Length; i++)
                handler.Handle(message[i]);

            Assert.IsTrue(handler.CanHandle(message[0]));
            Assert.IsTrue(handler.Handle(message[0]));
        }

        /// <summary>
        /// This method asserts if the handler can accept a successful message
        /// This is asserted by feeding the full message
        /// </summary>
        /// <param name="message">A byte array with the message</param>
        protected void Test_Handler_Receives_Message_Successfully(byte[] message)
        {
            for (int i = 0; i < message.Length - 1; i++)
            {
                Assert.IsTrue(handler.CanHandle(message[i]));
                Assert.IsTrue(handler.Handle(message[i]));
            }

            Assert.IsFalse(handler.Handle(message.Last()));
        }
    }
}