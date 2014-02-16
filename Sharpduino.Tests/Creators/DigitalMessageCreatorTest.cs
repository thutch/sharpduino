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
    public class DigitalMessageCreatorTest : BaseMessageCreatorTest
    {
        [Test]
        public override void Creates_Appropriate_Message()
        {
            var pins = new bool[]{true,false,true,true,false,false,true,false};
            var value = BitHelper.PinVals2PortVal(pins);

            var creator = new DigitalMessageCreator();
            var returnBytes = creator.CreateMessage(new DigitalMessage() {PinStates = pins, Port = 5});

            byte lsb, msb;
            BitHelper.IntToBytes(value,out lsb, out msb);

            Assert.AreEqual(returnBytes[0],(byte)(MessageConstants.DIGITAL_MESSAGE | 0x05));
            Assert.AreEqual(returnBytes[1],lsb);
            Assert.AreEqual(returnBytes[2],msb);            
        }

        [Test]
        public override void Throws_Error_On_Wrong_Message()
        {
            var creator = new DigitalMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }
    }
}
