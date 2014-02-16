using NUnit.Framework;
using Sharpduino.Creators;
using Sharpduino.Exceptions;
using Sharpduino.Messages;
using Sharpduino.Messages.Send;
using Sharpduino.Messages.TwoWay;

namespace Sharpduino.Library.Tests.Creators
{
    [TestFixture]
    public class StaticMessageCreatorTest
    {
        [Test, TestCaseSource("messages")]
        public void Creates_Appropriate_Message(StaticMessage message)
        {
            var creator = new StaticMessageCreator();
            var bytes = creator.CreateMessage(message);
            Assert.AreEqual(bytes,message.Bytes);
        }

        [Test]
        public void Throws_Error_On_Wrong_Message()
        {
            var creator = new StaticMessageCreator();
            Assert.Throws<MessageCreatorException>(() => creator.CreateMessage(new AnalogMessage()));
        }

        private static object[] messages
        {
            get
            {
                return new object[]
                    {
                        new ProtocolVersionRequestMessage(),
                        new QueryFirmwareMessage(),
                        new ResetMessage(),
                        new QueryCapabilityMessage()
                    };
            }
        }
    }
}
