using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Sharpduino.Library.Tests.LiveTests.Creators
{
    [TestFixture]
    public class LiveToggleAnalogMessageReportCreatorTest
    {
        private SerialPort port;
        [TestFixtureSetUp]
        public void Init()
        {
            port = new SerialPort("COM3",57600,Parity.None,8,StopBits.One);
        }

        [TestFixtureTearDown]
        public void Finish()
        {
            port.Close();
            port.Dispose();
        }

        [Test]
        [Ignore("Not implemented")]
        public void Successfully_Toggles_Analog_Report_On()
        {
            // TODO : this should check if there are incoming analog reports
            // TODO : if not then it should toggle one pin on and wait to receive the report
            throw new NotImplementedException();
        }

        [Test]
        [Ignore("Not implemented")]
        public void Successfully_Toggles_Analog_Report_Off()
        {
            // TODO : this should toggle the analog report on and receive some messages then toggle it off
            throw new NotImplementedException();
        }
    }
}
