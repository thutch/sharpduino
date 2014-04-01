using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sharpduino.Constants;
using Sharpduino.Messages.Receive;

namespace Sharpduino.Library.Tests
{
    [TestFixture]
    public class PinTest
    {

        private Pin Pin(CapabilityMessage message)
        {
            return new Pin(message);
        }


        [Test]
        public void CreationTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);

            Assert.NotNull(pin);
            Assert.IsTrue(pin.Port == 0);

        }

        [Test]
        public void HasPinCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsTrue(pin.HasPinCapability(PinModes.Input));
            Assert.IsFalse(pin.HasPinCapability(PinModes.Output));
            
        }

        [Test]
        public void ResolutionOfCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsTrue(pin.ResolutionOfCapability(PinModes.Input)== 1);
            Assert.IsTrue(pin.ResolutionOfCapability(PinModes.Output) == -1);

        }

        [Test]
        public void HasDigitalCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;
            capability.Modes[PinModes.Output] = 1;

            var pin = Pin(capability);
            Assert.IsTrue(pin.HasDigitalCapability());

        }

        [Test]
        public void HasInputCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsTrue(pin.HasInputCapability());

        }

        [Test]
        public void IsInputModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            //CurrentMode is set to PinModes.Input by default 
            //http://stackoverflow.com/questions/1165402/initial-value-of-an-enum
            Assert.IsTrue(pin.IsInputMode());

            pin.CurrentMode = PinModes.I2C;
            Assert.IsFalse(pin.IsInputMode());

        }

        [Test]
        public void HasOutputCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasOutputCapability());

            pin.Capabilities[PinModes.Output] = 1;
            Assert.IsTrue(pin.HasOutputCapability());

        }

        [Test]
        public void IsOutputModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Output] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsOutputMode());

            pin.CurrentMode = PinModes.Output;
            Assert.IsTrue(pin.IsOutputMode());

        }

        [Test]
        public void HasAnalogCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasAnalogCapability());

            pin.Capabilities[PinModes.Analog] = 1;
            Assert.IsTrue(pin.HasAnalogCapability());

        }

        [Test]
        public void IsAnalogModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Analog] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsAnalogMode());

            pin.CurrentMode = PinModes.Analog;
            Assert.IsTrue(pin.IsAnalogMode());

        }

        [Test]
        public void HasPWMCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasPWMCapability());

            pin.Capabilities[PinModes.PWM] = 1;
            Assert.IsTrue(pin.HasPWMCapability());

        }

        [Test]
        public void IsPWMModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Analog] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsPWMMode());

            pin.CurrentMode = PinModes.PWM;
            Assert.IsTrue(pin.IsPWMMode());

        }

        [Test]
        public void HasServoCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasServoCapability());

            pin.Capabilities[PinModes.Servo] = 1;
            Assert.IsTrue(pin.HasServoCapability());

        }

        [Test]
        public void IsServoModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Analog] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsServoMode());

            pin.CurrentMode = PinModes.Servo;
            Assert.IsTrue(pin.IsServoMode());

        }

        [Test]
        public void HasShiftCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasShiftCapability());

            pin.Capabilities[PinModes.Shift] = 1;
            Assert.IsTrue(pin.HasShiftCapability());

        }

        [Test]
        public void IsShiftModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Analog] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsShiftMode());

            pin.CurrentMode = PinModes.Shift;
            Assert.IsTrue(pin.IsShiftMode());

        }

        [Test]
        public void HasI2CCapabilityTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Input] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.HasI2CCapability());

            pin.Capabilities[PinModes.I2C] = 1;
            Assert.IsTrue(pin.HasI2CCapability());

        }

        [Test]
        public void IsI2CModeTest()
        {
            var capability = new CapabilityMessage();
            capability.PinNo = 1;
            capability.Modes[PinModes.Analog] = 1;

            var pin = Pin(capability);
            Assert.IsFalse(pin.IsI2CMode());

            pin.CurrentMode = PinModes.I2C;
            Assert.IsTrue(pin.IsI2CMode());

        }

    }
}
