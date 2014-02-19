using System;
using System.Linq;
using System.Reflection;
using Sharpduino.Creators;
using Sharpduino.Handlers;
using Sharpduino.Messages;
using Sharpduino.Messages.Send;
using Sharpduino.SerialProviders;
using System.Collections.Generic;

namespace Sharpduino.Base
{
    /// <summary>
    /// This is a firmata base class that adds all known message handlers.
    /// It is useful for the full firmata implementation
    /// </summary>
    public abstract class FirmataBase : FirmataEmptyBase
    {
        protected FirmataBase(ISerialProvider serialProvider) : base(serialProvider)
        {
            AddBasicMessageHandlers();
            AddBasicMessageCreators();
        }

        private void AddBasicMessageCreators()
        {
            //TODO: Find a workaround for the GetGenericTypeDefinition
            //string @namespace = "Sharpduino.Creators";
            //var messageCreators = (from t in Assembly.GetExecutingAssembly().GetTypes()
            //                       where t.IsClass && !t.IsAbstract && //We are searching for a non-abstract class 
            //                             t.Namespace == @namespace && //in the namespace we provide  
            //                             t.BaseType.GetGenericArguments()[0] != typeof(StaticMessage) && // Do not include the static message creator
            //              t.GetInterfaces().Any(x => x.GetGenericTypeDefinition() == typeof(IMessageCreator<>)) //that implements IMessageCreator<>
            //                       select t).ToList();

            //// Create an instance for each type we found and add it to the MessageCreators with 
            //// the Message Type that it creates as a key
            //messageCreators.ForEach(
            //    t => MessageCreators[t.BaseType.GetGenericArguments()[0]] = (IMessageCreator)Activator.CreateInstance(t));

            List<IMessageCreator> messageCreators = new List<IMessageCreator>();
            messageCreators.Add(new AnalogOutputMessageCreator());
            messageCreators.Add(new DigitalMessageCreator());
            messageCreators.Add(new ExtendedAnalogMessageCreator());
            messageCreators.Add(new I2CConfigMessageCreator());
            messageCreators.Add(new I2CRequestMessageCreator());
            messageCreators.Add(new PinModeMessageCreator());
            messageCreators.Add(new PinStateQueryMessageCreator());
            messageCreators.Add(new SamplingIntervalMessageCreator());
            messageCreators.Add(new ServoConfigMessageCreator());
            messageCreators.Add(new ToggleAnalogReportMessageCreator());
            messageCreators.Add(new ToggleDigitalReportMessageCreator());

            messageCreators.ForEach(t => MessageCreators[t.GetType().BaseType.GetGenericArguments().FirstOrDefault()] = (IMessageCreator)t);

            // This is the special case for the static message creator
            StaticMessageCreator staticMessageCreator = new StaticMessageCreator();

            // Get only the StaticMessage derived types
            string  @namespace = "Sharpduino.Messages";
            var staticMessages = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                  where t.IsClass && t.BaseType == typeof (StaticMessage)
                                  select t).ToList();

            // Add them to the MessageCreators dictionary
            staticMessages.ForEach(t => MessageCreators[t] = staticMessageCreator);
        }



        private void AddBasicMessageHandlers()
        {
            string @namespace = "Sharpduino.Handlers";
            var messageCreators = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where t.IsClass && !t.IsAbstract &&                     //We are searching for a non-abstract class 
                                         t.Namespace == @namespace &&                        //in the namespace we provide
                                         t.GetInterfaces().Any(x => x == typeof(IMessageHandler)) //that implements IMessageHandler
                                   select t).ToList();

            // Create an instance for each type we found and add it to the AvailableHandlers\
            messageCreators.ForEach(
                t => AvailableHandlers.Add((IMessageHandler)Activator.CreateInstance(t,MessageBroker)));
        }
    }
}