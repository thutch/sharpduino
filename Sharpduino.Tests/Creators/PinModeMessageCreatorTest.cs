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
    public class PinModeMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var creator = new PinModeMessageCreator();
            var bytes = creator.CreateMessage(new PinModeMessage {Mode = PinModes.Analog, Pin = 4});

            Assert.AreEqual(bytes[0],MessageConstants.SET_PIN_MODE);
            Assert.AreEqual(bytes[1],4);
            Assert.AreEqual(bytes[2],(byte)PinModes.Analog);
        }

        [Test]
        public void Throws_Error_On_Wrong_Pin()
        {
            var creator = new PinModeMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new PinModeMessage { Mode = PinModes.Analog, Pin = 244 }));
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new PinModeMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }
    }
}
