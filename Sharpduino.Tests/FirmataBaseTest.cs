using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharpduino.Base;
using Sharpduino.Creators;
using Sharpduino.Handlers;
using Sharpduino.SerialProviders;

namespace Sharpduino.Library.Tests
{
    internal class StubFirmataBase : FirmataBase
    {
        public StubFirmataBase(ISerialProvider serialProvider) : base(serialProvider)
        {}

        public MessageBroker Broker
        {
            get { return MessageBroker; }
        }

        public List<IMessageHandler> CurrentHandlers
        {
            get { return AppropriateHandlers; }
        }

        public List<IMessageHandler> Handlers
        {
            get { return AvailableHandlers; }
        }

        public Dictionary<Type, IMessageCreator> Creators
        {
            get { return MessageCreators; }
        }
    }

    [TestFixture]
    public class FirmataBaseTest
    {
        [Test]
        public void Initializes_Basic_Handlers()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            var stubBase = new StubFirmataBase(mockSerialProvider.Object);

            // These numbers should reflect the number of creators and handlers in
            // the Sharpduino.Base.Handlers and the Sharpduino.Base.Creators namespace
            // if we put others (maybe in a new protocol version) this test should fail
            Assert.AreEqual(9,stubBase.Handlers.Count);
            Assert.AreEqual(16,stubBase.Creators.Count);
        }
    }
}
