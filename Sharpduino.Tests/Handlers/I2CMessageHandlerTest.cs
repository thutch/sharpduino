using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Receive;

namespace Sharpduino.Library.Tests.Handlers
{
    [TestFixture]
    public class I2CMessageHandlerTest : BaseSysexMessageHandlerTest<I2CMessageHandler>
    {
        private byte[] CreateMessage(int slaveAdress, int register, params int[] data)
        {
            byte slaveLSB, slaveMSB;
            BitHelper.IntToBytes(slaveAdress, out slaveLSB, out slaveMSB);

            byte registerLSB, registerMSB;
            BitHelper.IntToBytes(register,out registerLSB,out registerMSB);

            var bytes = new List<byte>
                       {
                           MessageConstants.SYSEX_START,
                           SysexCommands.I2C_REPLY,
                           slaveLSB,
                           slaveMSB,
                           registerLSB,
                           registerMSB
                       };


            bytes.AddRange(BitHelper.IntArrayToBytesArray(data));
            bytes.Add(MessageConstants.SYSEX_END);

            return bytes.ToArray();
        }


        protected override byte SysexCommandByte
        {
            get { return SysexCommands.I2C_REPLY; }
        }

        protected override I2CMessageHandler CreateHandler()
        {
            return new I2CMessageHandler(mockBroker.Object);
        }

        [TestCase(new[]{554},TestName = "One Byte")]
        [TestCase(554,6,TestName = "Two Bytes")]
        [TestCase(554,6,1000,TestName = "Three Bytes")]
        [TestCase(new object[]{554,6,1000,45},TestName = "Four Bytes")]
        public void Successful_Message(params int[] caseBytes)
        {
            var bytes = CreateMessage(3, 5, caseBytes);
            Test_Handler_Receives_Message_Successfully(bytes);            
        }

        [TestCase(new[] { 554 }, TestName = "One Byte")]
        [TestCase(554, 6, TestName = "Two Bytes")]
        [TestCase(554, 6, 1000, TestName = "Three Bytes")]
        [TestCase(new object[] { 554, 6, 1000, 45 }, TestName = "Four Bytes")]
        public void Successful_Message_Raises_Event_With_Right_Data(params int[] caseBytes)
        {
            var bytes = CreateMessage(3, 5, caseBytes);

            for (int i = 0; i < bytes.Length; i++)
                handler.Handle(bytes[i]);

            mockBroker.Verify(p => p.CreateEvent(It.Is<I2CResponseMessage>(mes =>
                mes.SlaveAddress == 3 && mes.Register == 5 && 
                mes.Data.Count == caseBytes.Length)));
        }

        [Test]
        public void Handler_Resets_Successfully_After_Message()
        {
            var bytes = CreateMessage(3, 4, 5);
            Test_Handler_Resets_Successfully(bytes);
        }

        [Test]
        public void Exception_Thrown_On_Wrong_Address()
        {
            // Fails on lsb
            handler.Handle(MessageConstants.SYSEX_START);
            handler.Handle(SysexCommands.I2C_REPLY);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte) 128));

            // Fails on msb (we assume that the handler has reset after the error)
            handler.Handle(MessageConstants.SYSEX_START);
            handler.Handle(SysexCommands.I2C_REPLY);
            handler.Handle(0);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte)128));
        }

        [Test]
        public void Exception_Thrown_On_Wrong_Register()
        {
            // Fails on lsb
            handler.Handle(MessageConstants.SYSEX_START);
            handler.Handle(SysexCommands.I2C_REPLY);
            handler.Handle(1);
            handler.Handle(0);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte)128));

            // Fails on msb (we assume that the handler has reset after the error)
            handler.Handle(MessageConstants.SYSEX_START);
            handler.Handle(SysexCommands.I2C_REPLY);
            handler.Handle(0);
            handler.Handle(1);
            handler.Handle(0);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte)128));
        }

        [Test]
        public void Exception_Thrown_On_Wrong_Data()
        {
            // Fails on lsb
            var bytes = CreateMessage(1, 1, 4);
            for (int i = 0; i < 6; i++)
                handler.Handle(bytes[i]);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte)128));

            // Fails on msb (we assume that the handler has reset after the error)
            for (int i = 0; i < 7; i++)
                handler.Handle(bytes[i]);
            Assert.Throws<MessageHandlerException>(() => handler.Handle((byte)128));
        }
    }
}
