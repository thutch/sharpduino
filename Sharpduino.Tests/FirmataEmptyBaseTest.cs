using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.Creators;
using Sharpduino.EventArguments;
using Sharpduino.Exceptions;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.TwoWay;
using Sharpduino.SerialProviders;

namespace Sharpduino.Library.Tests
{
    internal class FirmataEmptyBaseStub : FirmataEmptyBase
    {
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

        public FirmataEmptyBaseStub(ISerialProvider serialProvider) : base(serialProvider)
        {
            // Do nothing
        }
    }

    [TestFixture]
    public class FirmataEmptyBaseTest
    {
        [Test]
        public void Should_Not_Have_Any_Handlers_By_Default()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
                Assert.IsEmpty(moqFirmataEmptyBase.Handlers);            
        }

        [Test]
        public void Should_Call_Dispose_On_The_Serial_Provider()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                
            }                           
            mockSerialProvider.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void Should_Send_Bytes_To_Handler_Successfully()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                moqFirmataEmptyBase.Handlers.Add(new AnalogMessageHandler(moqFirmataEmptyBase.Broker));
                mockSerialProvider.Raise(m => m.DataReceived += null, new DataReceivedEventArgs(new byte[] {MessageConstants.ANALOG_MESSAGE, 0x00}));
                // Wait for the parser thread to catch up
                Thread.Sleep(100);

                // See if the handler was successfully added to the appropriate ones
                Assert.AreSame(moqFirmataEmptyBase.Handlers[0], moqFirmataEmptyBase.CurrentHandlers[0]);
            }             
        }

        [Test]
        public void No_Appropriate_Handler_After_Successful_Message()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                moqFirmataEmptyBase.Handlers.Add(new AnalogMessageHandler(moqFirmataEmptyBase.Broker));
                mockSerialProvider.Raise(m => m.DataReceived += null, new DataReceivedEventArgs(new byte[] { MessageConstants.ANALOG_MESSAGE, 0x00,0x00 }));
                // Wait for the parser thread to catch up
                Thread.Sleep(100);

                // See if the handler was successfully added to the appropriate ones
                Assert.AreEqual(0, moqFirmataEmptyBase.CurrentHandlers.Count);
            }
        }

        [Test]
        public void Should_Send_Bytes_To_Multiple_Handlers_Successfully()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                moqFirmataEmptyBase.Handlers.Add(new AnalogMappingMessageHandler((moqFirmataEmptyBase.Broker)));
                moqFirmataEmptyBase.Handlers.Add(new SysexFirmwareMessageHandler((moqFirmataEmptyBase.Broker)));
                mockSerialProvider.Raise(m => m.DataReceived += null, 
                    new DataReceivedEventArgs(new [] { MessageConstants.SYSEX_START }));
                
                // Wait for the parser thread to catch up
                Thread.Sleep(100);

                // See if the handler was successfully added to the appropriate ones
                Assert.AreEqual(2,moqFirmataEmptyBase.CurrentHandlers.Count);

                mockSerialProvider.Raise(m => m.DataReceived += null,
                    new DataReceivedEventArgs(new [] { SysexCommands.ANALOG_MAPPING_RESPONSE }));

                // Wait for the parser thread to catch up
                Thread.Sleep(50);

                Assert.AreSame(moqFirmataEmptyBase.Handlers[0],moqFirmataEmptyBase.CurrentHandlers[0]);
                Assert.AreEqual(1,moqFirmataEmptyBase.CurrentHandlers.Count);
                
            }
        }

        [Test]
        public void Should_Reset_Handlers_When_They_Are_Discarded_From_The_AppropriateHandlers_List()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                var ana = new AnalogMappingMessageHandler(moqFirmataEmptyBase.Broker);
                var firm = new SysexFirmwareMessageHandler(moqFirmataEmptyBase.Broker);
                moqFirmataEmptyBase.Handlers.Add(ana);
                moqFirmataEmptyBase.Handlers.Add(firm);
                mockSerialProvider.Raise(m => m.DataReceived += null,
                    new DataReceivedEventArgs(new[] { MessageConstants.SYSEX_START, SysexCommands.ANALOG_MAPPING_RESPONSE }));

                // Wait for the parser thread to catch up
                Thread.Sleep(100);                
                
                // Assert that the firmware handler has been reset and can handle a sysex start message
                // if it wasn't reset it would require a SysexCommands.QUERY_FIRMWARE byte
                Assert.IsTrue(firm.CanHandle(MessageConstants.SYSEX_START));
            }
        }

        [Test]
        public void Should_Send_Messages_Using_The_SerialProvider()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            Func<IEnumerable<byte>, bool> f = b =>
                                                  {
                                                      var c = b.ToArray();
                                                      return c[0] == MessageConstants.ANALOG_MESSAGE && c[1] == 0x01 && c[2] == 0x02;
                                                  };
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                // Add the creator for the message
                moqFirmataEmptyBase.Creators.Add(typeof(AnalogMessage),new AnalogOutputMessageCreator());
                // Send a new message
                moqFirmataEmptyBase.SendMessage(new AnalogMessage() { Pin = 0x01, Value = 0x02 });
            }

            // Verify that the base class called the serialprovider's send method
            mockSerialProvider.Verify(x => x.Send(
                It.Is<IEnumerable<byte>>(b =>
                    b.First() ==
                        (MessageConstants.ANALOG_MESSAGE | 0x01) &&
                        b.Skip(1).First() == 0x02 &&
                        b.Skip(2).First() == 0x00)));
        }

        [Test]
        public void Should_Throw_Error_If_There_Is_Not_Suitable_MessageCreator()
        {
            var mockSerialProvider = new Mock<ISerialProvider>();
            using (var moqFirmataEmptyBase = new FirmataEmptyBaseStub(mockSerialProvider.Object))
            {
                Assert.Throws<FirmataException>(() => moqFirmataEmptyBase.SendMessage(new AnalogMessage()));
            }
        }
    }
}
