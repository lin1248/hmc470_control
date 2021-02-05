import RPi.GPIO
import time

# 串行数据输入引脚连接的GPIO口
DS_1 = 21
DS_2 = 4
DS_3 = 17
DS_4 = 27
DS_5 = 22
DS_6 = 5
DS_7 = 6
DS_8 = 13
DS_9 = 19
DS_10 = 26
DS = [DS_1,DS_2,DS_3,DS_4,DS_5,DS_6,DS_7,DS_8,DS_9,DS_10]
 
# 移位寄存器时钟控制引脚连接的GPIO口——上升沿有效
SHCP = 16
 
# 数据锁存器时钟控制引脚连接的GPIO口——上升沿有效
STCP = 20

_1db = [0,0,0,0,1,1,1,1]
_2db = [0,0,0,1,0,1,1,1]
_4db = [0,0,0,1,1,0,1,1]
_8db = [0,0,0,1,1,1,0,1]
_16db = [0,0,0,1,1,1,1,0]
_31db = [0,0,0,0,0,0,0,0]



# 初始化GPIO
def GPIO_init():
    RPi.GPIO.setmode(RPi.GPIO.BCM)

    RPi.GPIO.setup(DS_1, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_2, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_3, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_4, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_5, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_6, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_7, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_8, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_9, RPi.GPIO.OUT)
    RPi.GPIO.setup(DS_10, RPi.GPIO.OUT)
    RPi.GPIO.setup(STCP, RPi.GPIO.OUT)
    RPi.GPIO.setup(SHCP, RPi.GPIO.OUT)

    RPi.GPIO.output(STCP, False)
    RPi.GPIO.output(SHCP, False)

# 输出
def GPIO_out(data):
    for d in data:
        i=0
        while i < len(d): 
            RPi.GPIO.output(int(DS[i]), d[i])
            i=i+1
        # 输入一位
        RPi.GPIO.output(SHCP, True)
        RPi.GPIO.output(SHCP, False)
    
    # 操作595锁存器
    RPi.GPIO.output(STCP, True)
    RPi.GPIO.output(STCP, False)
    print("done!")

# 输出所有数据
def GPIO_control(data_list):
    all_data=[]
    for i in range(8):
        step_data=[]
        for data in data_list:
            
            if(data==b'1'):
               
                step_data.append(_1db[i])
            elif(data==b'2'):
                
                step_data.append(_2db[i])
            elif(data==b'3'):
               
                step_data.append(_4db[i])
            elif(data==b'4'):
               
                step_data.append(_8db[i])
            elif(data==b'5'):
                
                step_data.append(_16db[i])
            elif(data==b'6'):
                
                step_data.append(_31db[i])
        all_data.append(step_data)
    print(all_data)
    GPIO_out(all_data)


# 清除GPIO口
def GPIO_clear():
    RPi.GPIO.cleanup()

#
def main_driver(data_list):
    try:
        GPIO_init()
        GPIO_control(data_list)
        GPIO_clear()
    except KeyboardInterrupt:
        print("exit...")
        GPIO_clear()
