using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharpduino.SerialProviders;

namespace Sharpduino.Library.Tests
{
    [TestFixture]
    public class EasyFirmataTest
    {
        [Test]
        public void EasyFirmata_Is_Initialized_Without_Errors()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            var firmata = new EasyFirmata(mockSerialProvider.Object);

            GC.WaitForPendingFinalizers();
        }

        [Test]
        public void EasyFirmata_Is_Disposed_Without_Errors()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var firmata = new EasyFirmata(mockSerialProvider.Object))
            {
                
            }

            mockSerialProvider.Verify(x => x.Dispose(),Times.Once());
        }
    }
}
