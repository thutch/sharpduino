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
    public class SamplingIntervalMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var bytes = new byte[]
                            {
                                MessageConstants.SYSEX_START,
                                SysexCommands.SAMPLING_INTERVAL,
                                34,
                                54,
                                MessageConstants.SYSEX_END
                            };

            var message = new SamplingIntervalMessage() {Interval = BitHelper.BytesToInt(34, 54)};
            var creator = new SamplingIntervalMessageCreator();
            var newBytes = creator.CreateMessage(message);
            Assert.AreEqual(bytes,newBytes);
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new SamplingIntervalMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }
    }
}
