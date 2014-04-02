using System;
using System.Collections.Generic;
using Sharpduino.Constants;
using Sharpduino.Messages.Receive;

namespace Sharpduino
{
    public class Pin
    {
        public PinModes CurrentMode { get; set; }

        //Key PinModes, Value is resolution
        public Dictionary<PinModes, int> Capabilities { get; set; }

        public int CurrentValue { get; set;}

        public byte PinNo { get; set; }
        public int Port { get; set; }

        protected Pin()
        {
            Capabilities = new Dictionary<PinModes, int>();
            CurrentValue = 0;
        }

        public Pin(CapabilityMessage message) : this()
        {
            foreach (var mes in message.Modes)
                Capabilities[mes.Key] = mes.Value;
            PinNo = message.PinNo;
            if(HasDigitalCapability())
                Port = (byte) PinNo/8;
        }

        public bool HasPinCapability(PinModes mode)
        {
            return Capabilities.ContainsKey(mode);
        }

        public int ResolutionOfCapability(PinModes mode)
        {
            if (HasPinCapability(mode))
            {
                return Capabilities[mode];
            }

            return -1;
        }

        public bool HasDigitalCapability()
        {
            return (HasPinCapability(PinModes.Input) || HasPinCapability(PinModes.Output));
        }

        public bool HasInputCapability()
        {
            return HasPinCapability(PinModes.Input);
        }

        public bool IsInputMode()
        {
            return CurrentMode == PinModes.Input;
        }
        public bool HasOutputCapability()
        {
            return HasPinCapability(PinModes.Output);
        }

        public bool IsOutputMode()
        {
            return CurrentMode == PinModes.Output;
        }

        public bool HasAnalogCapability()
        {
            return HasPinCapability(PinModes.Analog);
        }

        public bool IsAnalogMode()
        {
            return CurrentMode == PinModes.Analog;
        }

        public bool HasPWMCapability()
        {
            return HasPinCapability(PinModes.PWM);
        }

        public bool IsPWMMode()
        {
            return CurrentMode == PinModes.PWM;
        }

        public bool HasServoCapability()
        {
            return HasPinCapability(PinModes.Servo);
        }

        public bool IsServoMode()
        {
            return CurrentMode == PinModes.Servo;
        }

        public bool HasShiftCapability()
        {
            return HasPinCapability(PinModes.Shift);
        }

        public bool IsShiftMode()
        {
            return CurrentMode == PinModes.Shift;
        }

        public bool HasI2CCapability()
        {
            return HasPinCapability(PinModes.I2C);
        }

        public bool IsI2CMode()
        {
            return CurrentMode == PinModes.I2C;
        }

    }
}