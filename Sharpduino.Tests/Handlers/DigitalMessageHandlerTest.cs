using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Moq;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Handlers
{
	[TestFixture]
	public class DigitalMessageHandlerTest : BaseMessageHandlerTest<DigitalMessageHandler>
	{
        protected override DigitalMessageHandler CreateHandler()
        {
            return new DigitalMessageHandler(mockBroker.Object);
        }

		[Test]
		public void Succesful_Digital_Message()
		{
		    var bytes = new byte[]
		                    {
		                        // Start Digital Message              Pin
		                        (byte) (handler.START_MESSAGE | 0x05),
		                        0x7F, // 0-6 bitmask
                                0x01, // 7-13 bitmask
		                    };

            mockBroker.Setup(p => p.CreateEvent(It.IsAny<DigitalMessage>()));

		    for (int i = 0; i < bytes.Length-1; i++)
		    {
		        Assert.IsTrue(handler.CanHandle(bytes[i]));
                Assert.IsTrue(handler.Handle(bytes[i]));
		    }

            Assert.IsTrue(handler.CanHandle(bytes.Last()));
		    Assert.IsFalse(handler.Handle(bytes.Last()));

            mockBroker.Verify(p => p.CreateEvent(It.Is<DigitalMessage>(mes =>
                mes.Port == (bytes[0] & MessageConstants.MESSAGEPINMASK) &&
                mes.PinStates.SequenceEqual(BitHelper.PortVal2PinVals((byte) BitHelper.BytesToInt(bytes[1],bytes[2]))))),
                Times.Once());

            // Check if the handler has reset
            Assert.IsTrue(handler.CanHandle(bytes[0]));
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
                {
                    Assert.Throws<MessageHandlerException>(() => handler.Handle(i));

                }
                else
                {
                    Assert.DoesNotThrow(() => handler.Handle(i));

                }
                handler = CreateHandler();
            }
        }
	}
}

