using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Moq;
using NUnit.Framework;
using Sharpduino.Creators;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Receive;
using Sharpduino.Messages.Send;

namespace Sharpduino.Library.Tests.LiveTests.Handlers
{
    [TestFixture]
    public class LiveSysexFirmwareMessageHandlerTest : LiveBaseMessageHandlerTest<SysexFirmwareMessageHandler>
    {
        protected override SysexFirmwareMessageHandler CreateHandler()
        {
            return new SysexFirmwareMessageHandler(mockBroker.Object);
        }

        [Test]
        public void Receive_Firmware_Message_From_Live_Arduino_Running_Standard_Firmata_2_3()
        {
            var smc = new StaticMessageCreator();
            var bytes = smc.CreateMessage(new QueryFirmwareMessage());
            port.Write(bytes,0,bytes.Length);

            // Wait for the arduino to reply
            Thread.Sleep(100);

            while (port.BytesToRead > 0)
            {
                var incomingByte = (byte) port.ReadByte();
                Assert.IsTrue(handler.CanHandle(incomingByte));
                handler.Handle(incomingByte);
            }

            mockBroker.Verify( p => p.CreateEvent(
                   It.Is<SysexFirmwareMessage>(mes => mes.MajorVersion == 2 && mes.MinorVersion == 3)),Times.Once());
        }
    }
}
