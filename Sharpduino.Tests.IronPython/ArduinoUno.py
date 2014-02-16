import sys
sys.path.append("c:\\projects\\sharpduino\\sharpduino\\bin\\debug")
import clr
clr.AddReference("Sharpduino")

# Import namespaces that we need
from Sharpduino import *
from Sharpduino.SerialProviders import *
from Sharpduino.Messages import *
from Sharpduino.Constants import *

ard = ArduinoUno("COM4")
ard.SetSamplingInterval(100)



from System.Threading import Thread

def blink():
    for i in range(50):    
        global flag        
        ard.SetDO(ArduinoUnoPins.D13,flag)
        flag = not flag
        Thread.Sleep(100)

flag = True
ard.SetPinMode(ArduinoUnoPins.D13,PinModes.Output)
blink()

ard.Dispose()
ard = None
del ard
