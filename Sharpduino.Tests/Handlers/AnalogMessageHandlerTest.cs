using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Handlers
{
    [TestFixture]
    public class AnalogMessageHandlerTest : BaseMessageHandlerTest<AnalogMessageHandler>
    {		
        protected override AnalogMessageHandler CreateHandler()
        {
            return new AnalogMessageHandler(mockBroker.Object);
        }

        [Test]
        public void Successfull_Analog_Message()
        {
            var bytes = new byte[]
                            {
								// Analog Message 					 Pin
                                (byte) (handler.START_MESSAGE | 0x01), 
                                0x01, //LSB bits 0-6
                                0x71, //MSB bits 0-6
                            };

            for (int index = 0; index < bytes.Length - 1; index++)
            {
                var b = bytes[index];
                Assert.IsTrue(handler.CanHandle(b));
                Assert.IsTrue(handler.Handle(b));
            }

            Assert.IsTrue(handler.CanHandle(bytes.Last()));
            Assert.IsFalse(handler.Handle(bytes.Last()));

            // Check to see if the handler has reset and can handle a new analog message
            Assert.IsTrue(handler.CanHandle(bytes[0]));

            mockBroker.Verify(
                p => p.CreateEvent(
                    It.Is<AnalogMessage>(
                    mes => mes.Pin == (bytes[0] & MessageConstants.MESSAGEPINMASK) && 
					mes.Value == BitHelper.BytesToInt(bytes[1],bytes[2]))),Times.Once());
        }


        [Test]
        public override void Ignores_All_Other_Messages()
        {
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if ((i & MessageConstants.MESSAGETYPEMASK) != handler.START_MESSAGE)
                    Assert.IsFalse(handler.CanHandle(i));
                else
                    Assert.IsTrue(handler.CanHandle(i));
            }
        }

        [Test]
        public override void Throws_Error_If_Forced_Other_Message()
        {
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if ((i & MessageConstants.MESSAGETYPEMASK) != handler.START_MESSAGE)
                    Assert.Throws<MessageHandlerException>(() => handler.Handle(i));
                else
                    Assert.DoesNotThrow(() => handler.Handle(i));
                handler = CreateHandler();
            }
        }
    }
}
