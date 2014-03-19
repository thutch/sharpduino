using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.EventArguments;
using Sharpduino.Messages.Send;
using Sharpduino.Messages.TwoWay;
using Sharpduino.SerialProviders;

namespace Sharpduino.Tests.Consoles
{
    class Program
    {
        private static bool isInitialized = false;
        static void Main(string[] args)
        {
            ComPortProvider port = new ComPortProvider("COM3");
            using (var easyFirmata = new EasyFirmata(port))
            {
                //easyFirmata.Initialized += easyFirmata_Initialized;
                //easyFirmata.NewAnalogValue += easyFirmata_NewAnalogValue;

                while (!easyFirmata.IsInitialized) ;

                for (int i = 0; i < easyFirmata.Pins.Count; i++)
                {
                    var pin = easyFirmata.Pins[i];
                    Console.WriteLine("Pin:{0} Current Mode:{1}", i, pin.CurrentMode);
                    foreach (var s in pin.Capabilities)
                    {
                        Console.WriteLine("\t{0} : {1} Resolution", s.Key, s.Value);
                    }
                }


                #region Servo Test

//                easyFirmata.SendMessage(new ServoConfigMessage(){Pin = 9});
//                int val = 90;
//                ConsoleKey key;
//                do
//                {
//                    key = Console.ReadKey().Key;
//                    if ( key == ConsoleKey.UpArrow )
//                        easyFirmata.SendMessage(new AnalogMessage(){Pin = 9,Value = ++val});
//                    else if ( key == ConsoleKey.DownArrow )
//                        easyFirmata.SendMessage(new AnalogMessage() { Pin = 9, Value = --val });
//
//                    Thread.Sleep(100);
//
//                } while (key != ConsoleKey.Enter);

                #endregion

                #region I2C
                // Setup the I2C communication
//                var message = new I2CConfigMessage() { Delay = 200 };
//                easyFirmata.SendMessage(message);
//
//                /////////////////////////////////////////////////////////////
                // Testing the MCP3221 ADC from pickit serial i2c demo board
//                /////////////////////////////////////////////////////////////
//                var requestMessage = new I2CRequestMessage()
//                                         {
//                                             IsAddress10BitMode = false,
//                                             ReadWriteOptions = I2CReadWriteOptions.ReadOnce,
//                                             SlaveAddress = 0x9A >> 1,
//                                             Data = new List<int> {2}
//                                         };
//                
                // Register the I2C Handler
//                EventHandler<NewI2CMessageEventArgs> handler = (s, e) => 
//                    Console.WriteLine("MSB: {0} LSB: {1} Value: {2} Volts: {3}", 
//                    e.Data[0], e.Data[1], (e.Data[0] << 8) | e.Data[1], 5.0 / 4096 * ((e.Data[0] << 8) | e.Data[1]));
//                easyFirmata.NewI2CMessage += handler;
//
                // Read while we don't press a key
//                while (!Console.KeyAvailable)
//                {
//                    easyFirmata.SendMessage(requestMessage);
//                    Thread.Sleep(100);
//                }
//
//                Console.ReadKey();
//                Thread.Sleep(100);
//                easyFirmata.NewI2CMessage -= handler;
//
//
//                /////////////////////////////////////////////////////////////
                // Testing the MCP3221 ADC from pickit serial i2c demo board
//                /////////////////////////////////////////////////////////////
//
                // We want the Temperature register
//                requestMessage = new I2CRequestMessage()
//                                     {
//                                         IsAddress10BitMode = false,
//                                         ReadWriteOptions = I2CReadWriteOptions.Write,
//                                         SlaveAddress = 0x92 >> 1,
//                                         Data = new List<int>{0}
//                                     };
//                easyFirmata.SendMessage(requestMessage);
//
                // This is the message to get the data
//                requestMessage.ReadWriteOptions = I2CReadWriteOptions.ReadOnce;
//                requestMessage.Data = new List<int>{2};
//
//                handler = (s, e) => Console.WriteLine("MSB: {0} LSB: {1} Value: {2} Temp: {3}",
//                   e.Data[0], e.Data[1], (e.Data[0] << 8) | e.Data[1], (((e.Data[0] & 0x7F) << 2) | (e.Data[1] >> 6)) / 4.0);
//                easyFirmata.NewI2CMessage += handler;
//
                // Read while we don't press a key
//                while (!Console.KeyAvailable)
//                {
//                    easyFirmata.SendMessage(requestMessage);
//                    Thread.Sleep(100);
//                }
//                Console.ReadKey();
//                Thread.Sleep(100);
//                easyFirmata.NewI2CMessage -= handler;
//
//                ///////////////////////////////////////////////////////////////////////
                // Testing the MCP23008 I/O Expander from pickit serial i2c demo board
//                ///////////////////////////////////////////////////////////////////////
//
                // We want the Temperature register
//                requestMessage = new I2CRequestMessage()
//                {
//                    IsAddress10BitMode = false,
//                    ReadWriteOptions = I2CReadWriteOptions.Write,
//                    SlaveAddress = 0x40 >> 1,
//                    Data = new List<int> { 0, 0 }  // register 0 -> IODIR -> 0x00 all outputs
//                };
//                easyFirmata.SendMessage(requestMessage);
//
//                requestMessage.Data = new List<int>{5,0x20}; // register 5 -> IOCON -> 0x20 Disable Sequential operation
//                easyFirmata.SendMessage(requestMessage);
//
//                byte val = 0;
//                ConsoleKey key;
//                do
//                {
//                    key = Console.ReadKey().Key;
//                    if (key == ConsoleKey.UpArrow)
//                        requestMessage.Data = new List<int> {10, ++val}; // register 10 -> OLAT -> value...
//                    else if (key == ConsoleKey.DownArrow)
//                        requestMessage.Data = new List<int> { 10, --val };
//
//                    easyFirmata.SendMessage(requestMessage);
//                
//                    Thread.Sleep(100);
//                
//                } while (key != ConsoleKey.Enter);

                #endregion
            }

//            using (var arduino = new ArduinoUno("COM4"))
//            {
//                while (!arduino.IsInitialized) ;
//                arduino.SetPinMode(ArduinoUnoPins.A0, PinModes.Output);
//                arduino.SetDO(ArduinoUnoPins.A0, true);
//
//                Console.ReadKey();
//            }
//
//            using (var ardMega = new ArduinoMega("COM6"))
//            {
//                while (!ardMega.IsInitialized) ;
//                while (!ardMega.IsInitialized) ;
//                int a = 5;
//            }
        }

        static void easyFirmata_NewAnalogValue(object sender, NewAnalogValueEventArgs e)
        {
            Console.WriteLine("New Value for pin {0} : {1}",e.AnalogPin,e.NewValue);
        }

        static void easyFirmata_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("Firmata has initialized");
            isInitialized = true;
        }
    }
}
