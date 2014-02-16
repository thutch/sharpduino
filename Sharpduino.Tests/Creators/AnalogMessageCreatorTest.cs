using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Creators;
using Sharpduino.Exceptions;
using Sharpduino.Messages;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Creators
{
    [TestFixture]
    public class AnalogMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var creator = new AnalogOutputMessageCreator();
            var bytes = creator.CreateMessage(new AnalogMessage {Pin = 3, Value = 5});
            byte lsb, msb;
            BitHelper.IntToBytes(5, out lsb, out msb);
            Assert.AreEqual(bytes[0],MessageConstants.ANALOG_MESSAGE | 3);
            Assert.AreEqual(bytes[1],lsb);
            Assert.AreEqual(bytes[2],msb);
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new AnalogOutputMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new DigitalMessage()));
        }
    }
}
