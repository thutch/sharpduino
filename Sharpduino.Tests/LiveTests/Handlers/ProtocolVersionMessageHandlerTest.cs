using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Moq;
using NUnit.Framework;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Receive;

namespace Sharpduino.Library.Tests.LiveTests.Handlers
{
    [TestFixture]
    public class ProtocolVersionMessageHandlerTest : LiveBaseMessageHandlerTest<ProtocolVersionMessageHandler>
    {
        protected override ProtocolVersionMessageHandler CreateHandler()
        {
            return new ProtocolVersionMessageHandler(mockBroker.Object);
        }

        [Test]
        public void Receive_Protocol_Version_Message_From_Live_Arduino_Running_Standard_Firmata_2_3()
        {
            /*  0  query protocol version (0xF9)
             */
            port.Write(new byte[] { handler.START_MESSAGE }, 0, 1);

            // Wait for the arduino to reply
            Thread.Sleep(100);

            while (port.BytesToRead > 0)
            {
                var incomingByte = (byte)port.ReadByte();
                Assert.IsTrue(handler.CanHandle(incomingByte));
                handler.Handle(incomingByte);
            }

            mockBroker.Verify(p => p.CreateEvent(It.IsAny<ProtocolVersionMessage>()), Times.Once());
        }
    }
}
