#This is for 

# Get the Suitable references
import sys
sys.path.append("c:\\projects\\sharpduino\\sharpduino\\bin\\debug")
import clr
clr.AddReference("Sharpduino")

# Import namespaces that we need
from Sharpduino import *
from Sharpduino.SerialProviders import *
from Sharpduino.Messages import *
from Sharpduino.Constants import *

#create the serial provider
port = ComPortProvider("COM3")

#create the firmata class
firm = EasyFirmata(port)

#Here we should wait
while firm.IsInitialized == False:
	pass

#create the message we want to send
mes = ServoConfigMessage()
mes.Pin = 9

#send the message
firm.SendMessage(mes)

# Go to another Angle
mes2 = AnalogMessage()
mes2.Pin = 9
mes2.Value = 92
firm.SendMessage(mes2)

firm.Dispose()
firm = None
del firm
