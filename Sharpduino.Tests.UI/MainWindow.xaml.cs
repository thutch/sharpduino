using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Sharpduino.Base;
using Sharpduino.Constants;
using Sharpduino.EventArguments;
using Sharpduino.Messages;
using Sharpduino.Messages.Send;
using Sharpduino.Messages.TwoWay;
using Sharpduino.SerialProviders;

namespace Sharpduino.Tests.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EasyFirmata firmata;
        private DispatcherTimer timer;
        private bool firstTime = true;
        List<PinVM> Pins = new List<PinVM>();
        private byte digiOutput = 0x0f;

        public MainWindow()
        {
            InitializeComponent();

            var port = new ComPortProvider("COM3");
            firmata = new EasyFirmata(port);
            firmata.NewDigitalValue += new EventHandler<NewDigitalValueEventArgs>(firmata_NewDigitalValue);

            timer = new DispatcherTimer(TimeSpan.FromSeconds(1),DispatcherPriority.Normal, (s,e) => CheckFirmata(),this.Dispatcher);
            timer.Start();
        }

        void firmata_NewDigitalValue(object sender, NewDigitalValueEventArgs e)
        {
            if ( e.Port != 0 )
                return;

            for (int i = 0; i < e.Pins.Length; i++)
            {
                Pins[i].IsOn = e.Pins[i];
            }
        }

        private void CheckFirmata()
        {
            if ( firmata.IsInitialized )
            {
                if ( firstTime )
                {
                    foreach (var pin in firmata.Pins)
                    {
                        Pins.Add(new PinVM());
                    }
                    listBox1.ItemsSource = Pins;

                    firmata.SendMessage(new PinModeMessage() { Mode = PinModes.Input, Pin = 3 });
                    firmata.SendMessage(new PinModeMessage() { Mode = PinModes.Input, Pin = 4 });
                    firstTime = false;
                }
            }
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();           
            firmata.Dispose();
            base.OnClosing(e);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var newStates = BitHelper.PortVal2PinVals(digiOutput);
            firmata.SendMessage(new DigitalMessage() { PinStates = newStates, Port = 0 });
            digiOutput = (byte) (~digiOutput);
        }
    }

    internal class PinVM : INotifyPropertyChanged
    {
        private bool isOn;
        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; OnPropertyChanged("IsOn");}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if ( handler != null )
            {
                handler(this,new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
