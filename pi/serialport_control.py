import serial
import time
from hmc470_driver import *
# open serial
ser = serial.Serial("/dev/ttyAMA0",115200)

# 解析数据
def getInfo(recv):
    
    if(recv[0]==35 and recv[-3]==36):
        s = recv[1:-3]
        l = s.split(str.encode(','))
        control(l)
        return str.encode('success!')
    return str.encode('useless data!')

# 控制衰减器
def control(data_list):
    main_driver(data_list)

def main():
    while True:
        count = ser.inWaiting()
        if count != 0:
            print("got it...")
            recv = ser.read(count)
            msg = getInfo(recv)
            ser.write(msg)
            print(recv)
            print(msg)
        ser.flushInput()
        time.sleep(0.1)

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        print("exit...")
        if ser != None:
            ser.close
