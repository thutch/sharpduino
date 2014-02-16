using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Receive;

namespace Sharpduino.Library.Tests.Handlers
{
    [TestFixture]
    public class PinStateMessageHandlerTest : BaseSysexMessageHandlerTest<PinStateMessageHandler>
    {
        private readonly byte[] sampleStateBytes = new byte[]{0x14,0x76,0x02};

        private byte[] CreateMessageBytes(params byte[] pinStateBytes)
        {
            var bytes = new List<byte>
            {
                MessageConstants.SYSEX_START, // Start message
                SysexCommands.PIN_STATE_RESPONSE, // Message Type
                4, // Pin No
                (byte) PinModes.Analog, // Current Pin Mode                
            };

            bytes.AddRange(pinStateBytes);
            bytes.Add(MessageConstants.SYSEX_END);

            return bytes.ToArray();
        }

        protected override byte SysexCommandByte
        {
            get { return SysexCommands.PIN_STATE_RESPONSE; }
        }

        protected override PinStateMessageHandler CreateHandler()
        {
            return new PinStateMessageHandler(mockBroker.Object);
        }

        [TestCase((byte)0x14, (byte)0x76, (byte)0x02)]
        [TestCase((byte)0x14, (byte)0x76)]
        [TestCase(new[] { (byte)0x14 })]
        public void Successful_PinState_Message(params byte[] pinStateBytes)
        {
            var bytes = CreateMessageBytes(pinStateBytes);
            Test_Handler_Receives_Message_Successfully(bytes);
        }

        [TestCase((byte)0x14, (byte)0x76, (byte)0x02)]
        [TestCase((byte)0x14, (byte)0x76)]
        [TestCase(new[]{(byte)0x14})]
        public void Message_Raises_Appropriate_Event(params byte[] pinStateBytes)
        {
            var bytes = CreateMessageBytes(pinStateBytes);
            for (int i = 0; i < bytes.Length; i++)
            {
                handler.Handle(bytes[i]);
            }

            int counter = 0;
            int value = pinStateBytes.Sum(val => val << 7*counter++);

            mockBroker.Verify(p => p.CreateEvent(It.Is <PinStateMessage>(
                mes => mes.Mode == PinModes.Analog && mes.PinNo == 4 &&
                mes.State == value)),Times.Once());
        }

        [Test]
        public void Handler_Resets_After_Successful_Message()
        {
            var bytes = CreateMessageBytes(sampleStateBytes);
            Test_Handler_Resets_Successfully(bytes);
        }

        [Test]
        public void Message_Fails_Due_To_Wrong_PinNo()
        {
            var bytes = CreateMessageBytes(sampleStateBytes);

            handler.Handle(bytes[0]);
            handler.Handle(bytes[1]);
            Assert.Throws<MessageHandlerException>(() => handler.Handle(0xF3));
        }

        [Test]
        public void Message_Fails_Due_To_Wrong_PinMode()
        {
            var bytes = CreateMessageBytes(sampleStateBytes);

            handler.Handle(bytes[0]);
            handler.Handle(bytes[1]);
            handler.Handle(bytes[2]);
            Assert.Throws<MessageHandlerException>(() => handler.Handle(0xF3));
        }

        [Test]
        public void Message_Fails_Due_To_No_PinState()
        {
            var bytes = CreateMessageBytes();

            for (int i = 0; i < 4; i++)
            {
                handler.Handle(bytes[i]);
            }

            Assert.Throws<MessageHandlerException>(() => handler.Handle(bytes[4]));
        }
    }
}
