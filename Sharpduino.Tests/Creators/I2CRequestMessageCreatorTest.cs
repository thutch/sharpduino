using System.Collections.Generic;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Creators;
using Sharpduino.Exceptions;
using Sharpduino.Messages;
using Sharpduino.Messages.Send;

namespace Sharpduino.Library.Tests.Creators
{
    [TestFixture]
    public class I2CRequestMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var bytes = new byte[]
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.I2C_REQUEST,                                
                                0x34,
                                0x18, // 7bit, stop reading
                                MessageConstants.SYSEX_END
                            };

            var message = new I2CRequestMessage
                              {
                                  IsAddress10BitMode = false,
                                  ReadWriteOptions = I2CReadWriteOptions.StopReading,
                                  SlaveAddress = BitHelper.BytesToInt(0x34, 0x0)
                              };

            TestMessage(bytes,message);
        }

        [Test]
        public void Creates_Other_Appropriate_Message()
        {
            var bytes = new byte[]
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.I2C_REQUEST,                                
                                0x34,
                                0x10, // 7bit, read continuously
                                MessageConstants.SYSEX_END
                            };

            var message = new I2CRequestMessage
            {
                IsAddress10BitMode = false,
                ReadWriteOptions = I2CReadWriteOptions.ReadContinuously,
                SlaveAddress = BitHelper.BytesToInt(0x34, 0x00)
            };

            TestMessage(bytes, message);
        }

        [Test]
        public void Creates_Appropriate_Message_With_10bit_Address()
        {
            var bytes = new byte[]
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.I2C_REQUEST,                                
                                0x34,
                                0x23, // 10bit, write
                                0x71,
                                0x77,
                                MessageConstants.SYSEX_END
                            };

            var message = new I2CRequestMessage
            {
                IsAddress10BitMode = true,
                ReadWriteOptions = I2CReadWriteOptions.Write,
                SlaveAddress = BitHelper.BytesToInt(0x34, 0x3)
            };
            message.Data.Add(BitHelper.BytesToInt(0x71, 0x77));

            TestMessage(bytes, message);
        }

        private void TestMessage(byte[] bytes, I2CRequestMessage message)
        {
            var creatore = new I2CRequestMessageCreator();
            var newBytes = creatore.CreateMessage(message);

            Assert.AreEqual(bytes,newBytes);
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new I2CRequestMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new I2CConfigMessage()));
        }
    }
}