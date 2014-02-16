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
    public class ToggleDigitalReportMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var creator = new ToggleDigitalReportMessageCreator();
            var bytes = creator.CreateMessage(new ToggleDigitalReportMessage {Port = 4, ShouldBeEnabled = true});

            Assert.AreEqual(bytes[0],MessageConstants.REPORT_DIGITAL_PORT | 4);
            Assert.AreEqual(bytes[1],1);
        }

        [Test]
        public void Creates_Appropriate_Message_Off()
        {
            var creator = new ToggleDigitalReportMessageCreator();
            var bytes = creator.CreateMessage(new ToggleDigitalReportMessage { Port = 4, ShouldBeEnabled = false });

            Assert.AreEqual(bytes[0], MessageConstants.REPORT_DIGITAL_PORT | 4);
            Assert.AreEqual(bytes[1], 0);
        }

        [Test]
        public void Throws_Error_On_Wrong_Port()
        {
            var creator = new ToggleDigitalReportMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new ToggleDigitalReportMessage {Port = 17}));
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new ToggleDigitalReportMessageCreator();
            Assert.Throws<MessageCreatorException>(() =>creator.CreateMessage(new AnalogMessage()));
        }
    }
}

