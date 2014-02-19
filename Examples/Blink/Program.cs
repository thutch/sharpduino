using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpduino;
using System.Threading;

namespace Blink
{
    class Program
    {
        static void Main(string[] args)
        {
            //Linux serial port
            var arduino = new ArduinoUno("/dev/ttyACM0");

            //Windows serial port
            //var arduino = new ArduinoUno("COM3");

            arduino.SetPinMode(Sharpduino.Constants.ArduinoUnoPins.D13, Sharpduino.Constants.PinModes.Output);

            while (true)
            {
                arduino.SetDO(Sharpduino.Constants.ArduinoUnoPins.D13, true);
                Thread.Sleep(1000);
                arduino.SetDO(Sharpduino.Constants.ArduinoUnoPins.D13, false);
                Thread.Sleep(1000);
            }
        }
    }
}
