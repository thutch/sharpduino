using System;
using System.Collections.Generic;
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
    public class AnalogMappingMessageHandlerTest : BaseSysexMessageHandlerTest<AnalogMappingMessageHandler>
    {
        private byte[] CreateBytes(params byte[] pinMappingBytes)
        {
            var bytes = new List<byte>()
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.ANALOG_MAPPING_RESPONSE
                            };
            bytes.AddRange(pinMappingBytes);
            bytes.Add(MessageConstants.SYSEX_END);

            return bytes.ToArray();
        }

        protected override byte SysexCommandByte
        {
            get { return SysexCommands.ANALOG_MAPPING_RESPONSE; }
        }

        protected override AnalogMappingMessageHandler CreateHandler()
        {
            return new AnalogMappingMessageHandler(mockBroker.Object);
        }

        [Test]
        public void Successful_Message()
        {
            var bytes = CreateBytes(0x7F, 0x7F, 0x00, 0x01);
            Test_Handler_Receives_Message_Successfully(bytes);
        }

        [Test]
        public void Handler_Resets_After_Successful_Message()
        {
            var bytes = CreateBytes(0x7F, 0x7F, 0x00, 0x01);
            Test_Handler_Resets_Successfully(bytes);
        }

        [Test]
        public void Successful_Message_Creates_Event_With_Right_Values()
        {
            var bytes = CreateBytes(0x7F, 0x7F, 0x00, 0x01);
            for (int i = 0; i < bytes.Length; i++)
                handler.Handle(bytes[i]);

            mockBroker.Verify(p => p.CreateEvent(It.Is<AnalogMappingMessage>(mes =>
                mes.PinMappings.Count == 4 &&
                mes.PinMappings[0] == 127 &&
                mes.PinMappings[1] == 127 &&
                mes.PinMappings[2] == 0 &&
                mes.PinMappings[3] == 1
                )),Times.Once());
        }

        [Test]
        public void Throws_On_Wrong_Mapping()
        {
            var bytes = CreateBytes(0xFF);
            handler.Handle(bytes[0]);
            handler.Handle(bytes[1]);
            Assert.Throws<MessageHandlerException>(() => handler.Handle(bytes[2]));
        }
    }
}
