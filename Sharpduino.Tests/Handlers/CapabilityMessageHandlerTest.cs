using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sharpduino.Constants;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Receive;

namespace Sharpduino.Library.Tests.Handlers
{
    [TestFixture]
    public class CapabilityMessageHandlerTest : BaseSysexMessageHandlerTest<CapabilityMessageHandler>
    {
        private byte[] CreateMessageBytes(params byte[] capabilityBytes)
        {
            var bytes = new List<byte>
                            {
                                MessageConstants.SYSEX_START, // Start message
                                SysexCommands.CAPABILITY_RESPONSE, // Message Type                
                            };

            bytes.AddRange(capabilityBytes);
            bytes.Add(MessageConstants.SYSEX_END);

            return bytes.ToArray();
        }

        protected override byte SysexCommandByte
        {
            get { return SysexCommands.CAPABILITY_RESPONSE; }
        }

        protected override CapabilityMessageHandler CreateHandler()
        {
            return new CapabilityMessageHandler(mockBroker.Object);
        }

        [Test, TestCaseSource(typeof(MyFactoryClass),"CasesSuccess")]
        public void Successful_Message(params byte[] capabilityBytes)
        {
            var bytes = CreateMessageBytes(capabilityBytes);
            Test_Handler_Receives_Message_Successfully(bytes);
        }

        [Test, TestCaseSource(typeof (MyFactoryClass), "Cases")]
        public void Handler_Resets_After_Successful_Message(params byte[] capabilityBytes)
        {
            var bytes = CreateMessageBytes(capabilityBytes);
            Test_Handler_Resets_Successfully(bytes);
        }

        [Test, TestCaseSource(typeof(MyFactoryClass), "ReturnCases")]
        public int Successful_Message_Raises_Appropriate_Events(params byte[] capabilityBytes)
        {
            var bytes = CreateMessageBytes(capabilityBytes);
            var countMessage = 0;
            mockBroker.Setup(p => p.CreateEvent(It.IsAny<CapabilityMessage>())).Callback(() => countMessage++);

            for (int i = 0; i < bytes.Length; i++)
                handler.Handle(bytes[i]);

            return countMessage;
        }


        [Test]
        public void Successful_Message_Raises_Event_With_The_Right_Values()
        {
            var bytes = CreateMessageBytes(new byte[]
                                               {
                                                   (byte) PinModes.Input, 1, (byte) PinModes.Output, 1,
                                                   (byte) PinModes.Analog, 10,
                                                   MessageConstants.FINISHED_PIN_CAPABILITIES, 
                                                   (byte) PinModes.Analog, 10,
                                                   MessageConstants.FINISHED_PIN_CAPABILITIES
                                               });

            for (int i = 0; i < bytes.Length; i++)
                handler.Handle(bytes[i]);

            mockBroker.Verify(p => p.CreateEvent(
                It.Is<CapabilityMessage>(
                    mes => mes.PinNo == 0 &&
                           mes.Modes.Keys.Contains(PinModes.Input) && mes.Modes[PinModes.Input] == 1 &&
                           mes.Modes.Keys.Contains(PinModes.Output) && mes.Modes[PinModes.Output] == 1 &&
                           mes.Modes.Keys.Contains(PinModes.Analog) && mes.Modes[PinModes.Analog] == 10
                    )), Times.Once());

            mockBroker.Verify(p => p.CreateEvent(
                It.Is<CapabilityMessage>(
                    mes => mes.PinNo == 1 &&
                           mes.Modes.Keys.Contains(PinModes.Analog) && mes.Modes[PinModes.Analog] == 10
                    )), Times.Once());

            // Also verify that when we finish with the capabilities we raise a capabilities finished event
            mockBroker.Verify(p => p.CreateEvent(
                It.IsAny<CapabilitiesFinishedMessage>()),Times.Once());
        }

    }

    public class MyFactoryClass
    {
        public static IEnumerable ReturnCases
        {
            get
            {
                yield return new TestCaseData(new byte[] { (byte)PinModes.Input, 1, (byte)PinModes.Output, 1, (byte)PinModes.Analog, 10, MessageConstants.FINISHED_PIN_CAPABILITIES }).Returns(1).SetName("One Pin");
                yield return new TestCaseData(new byte[] { (byte) PinModes.Input, 1, (byte) PinModes.Output, 1,  (byte) PinModes.Analog, 10, MessageConstants.FINISHED_PIN_CAPABILITIES, (byte) PinModes.Analog, 10, MessageConstants.FINISHED_PIN_CAPABILITIES }).Returns(2).SetName("Two Pins");
                yield return new TestCaseData(new byte[] { 0xFF, 0})
                    .Throws(typeof(MessageHandlerException))
                    .SetName("Send Wrong PinMode")
                    .SetDescription("An exception is expected").Returns(0);
            }
        }

        public static IEnumerable Cases
        {
            get
            {
                yield return new TestCaseData(
                                 new[]{
                                     (byte) PinModes.Input, (byte) 1, (byte) PinModes.Output, (byte) 1,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES
                                 }).SetName("One Pin Simple");
                yield return new TestCaseData(new[]
                                 {
                                     (byte) PinModes.Input, (byte) 1, (byte) PinModes.Output, (byte) 1,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES
                                 }).SetName("Two Pins Simple");
            }
        }

        public static IEnumerable CasesSuccess
        {
            get
            {
                yield return new TestCaseData(
                                 new[]{
                                     (byte) PinModes.Input, (byte) 1, (byte) PinModes.Output, (byte) 1,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES
                                 }).SetName("One Pin Success");
                yield return new TestCaseData(new[]
                                 {
                                     (byte) PinModes.Input, (byte) 1, (byte) PinModes.Output, (byte) 1,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES,
                                     (byte) PinModes.Analog, (byte) 10, MessageConstants.FINISHED_PIN_CAPABILITIES
                                 }).SetName("Two Pins Success");
            }
        }
    }
}
