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
using Sharpduino.Messages.Send;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Creators
{
    [TestFixture]
    public class ExtendedAnalogMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var creator = new ExtendedAnalogMessageCreator();
            var bytes = new byte[]
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.EXTENDED_ANALOG,
                                88,
                                53,
                                42,
                                MessageConstants.SYSEX_END
                            };

            var message = new ExtendedAnalogMessage(){Pin = 88, Value = BitHelper.BytesToInt(53,42)};

            var newBytes = creator.CreateMessage(message);
            Assert.AreEqual(bytes,newBytes);
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new ExtendedAnalogMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }

        [Test]
        public void Throws_Error_On_Wrong_Pin()
        {
            var creator = new ExtendedAnalogMessageCreator();
            var message = new ExtendedAnalogMessage() { Pin = 188, Value = BitHelper.BytesToInt(53, 42) };

            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(message));
        }
    }
}
