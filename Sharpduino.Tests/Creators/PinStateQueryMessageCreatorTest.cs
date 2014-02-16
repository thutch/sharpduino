using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharpduino.Constants;
using Sharpduino.Creators;
using Sharpduino.Exceptions;
using Sharpduino.Messages;
using Sharpduino.Messages.Send;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Creators
{
    [TestFixture]
    public class PinStateQueryMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var bytes = new byte[]
                {
                    MessageConstants.SYSEX_START,
                    SysexCommands.PIN_STATE_QUERY,
                    86,
                    MessageConstants.SYSEX_END
                };

            var message = new PinStateQueryMessage {Pin = 86};
            var creator = new PinStateQueryMessageCreator();
            var newBytes = creator.CreateMessage(message);
            Assert.AreEqual(bytes,newBytes);
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new PinStateQueryMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }

        [Test]
        public void Throws_Error_On_Wrong_Pin()
        {
            var creator = new PinStateQueryMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new PinStateQueryMessage{Pin = 130}));
        }
    }
}
