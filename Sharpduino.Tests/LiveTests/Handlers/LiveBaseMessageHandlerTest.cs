using System.IO.Ports;
using NUnit.Framework;
using Sharpduino.Handlers;

namespace Sharpduino.Library.Tests.LiveTests.Handlers
{
    public abstract class LiveBaseMessageHandlerTest<T> : BaseMessageHandlerTest<T> where T : IMessageHandler
    {
        protected SerialPort port;

        [TestFixtureSetUp]
        public void Init()
        {
            port = new SerialPort("COM3", 57600, Parity.None, 8, StopBits.One);
            port.Open();
        }

        [TestFixtureTearDown]
        public void Finish()
        {
            port.Close();
            port.Dispose();
        }
    }
}