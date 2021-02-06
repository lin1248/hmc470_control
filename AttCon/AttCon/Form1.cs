using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;


namespace AttCon
{
    public partial class Form1 : Form
    {
        #region
        //private SerialPort sp = new SerialPort();
        
        private int baudRate = 115200;
        private int dataBit = 8;
        private int parityBit = 0;
        private int stopBit = 1;
        private string portName = null;

       

        /// <summary>  
        /// 串口接收数据委托  
        /// </summary>  
        private delegate void rxHandler(string str);

        /// <summary>  
        /// 编码类型  
        /// </summary>  
        private Encoding EncodingType { get; set; } = Encoding.ASCII;
        
        /// <summary>
        /// 接受数据的方法注册到委托实例中
        /// </summary>
        

        /// <summary>
        /// 串口发送数据
        /// </summary>
        /// <param name="sendData"></param>
        private void SendData(string sendData)
        {
            try
            {
                sp.Encoding = EncodingType;
                sp.Write(sendData);
            }
            catch (Exception e)
            {
                richTextBox1_toString(Function.paramvalue.error, "数据发送错误：" + e + "\r\n");
                throw e;
            }
        }

        /// <summary>
        /// 设置portname
        /// </summary>
        private void SetSerialPortName()
        {
            string buffer = Function.GetComName(comboBox1.Items[comboBox1.SelectedIndex].ToString());
            if (buffer.Length > 3)
            {
                portName = buffer;
                sp.PortName = portName;
            }
            else
            {
                richTextBox1_toString(Function.paramvalue.error, "串口名不存在!!!\r\n");
            }
        }

        /// <summary>
        /// 在comboBox1上显示所有串口
        /// </summary>
        private void comboBox1_viewCom()
        {
            comboBox1.Items.Clear(); //清除Combox中的项
            String[] str = Function.GetSerialPort();
            foreach (string comstr in str)
            {
                comboBox1.Items.Add(comstr); //添加Com名称
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; //默认的索引
            }
        }

        /// <summary>
        /// 设置serialport属性
        /// </summary>
        private void SetsSerialport()
        {
            if (comboBox1.Items.Count > 0)
                //设置PortName
                SetSerialPortName();
            sp.BaudRate = baudRate;
            sp.DataBits = dataBit;
            sp.Parity = (Parity)parityBit;
            sp.StopBits = (StopBits)stopBit;

        }

        /// <summary>
        /// 信息写入texBox1
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="err_info"></param>
        private void richTextBox1_toString(Function.paramvalue flag, string _info = null)
        {
            int _flag = (int)flag;
            string info;
            if (_flag == 1)
            {
                richTextBox1.SelectionColor = Color.Black;
                info = "当前串口为：" + portName + "\r\n"
                    + "波特率：" + baudRate + "\r\n"
                    + "数据位：" + dataBit + "\r\n"
                    + "停止位：" + stopBit + "\r\n"
                    + "奇偶校验位：" + parityBit + "\r\n";
                richTextBox1.AppendText(info);
            }
            else if (_flag == 2)
            {
                richTextBox1.SelectionColor = Color.Black;
                info = "当前串口设置为：" + portName + "\r\n";
                richTextBox1.AppendText(info);
            }
            else if (_flag == 3)
            {
                richTextBox1.SelectionColor = Color.Green;
                info = "串口已打开\r\n";
                richTextBox1.AppendText(info);
            }
            else if (_flag == 4)
            {
                richTextBox1.SelectionColor = Color.Orange;
                info = "串口已关闭\r\n";
                richTextBox1.AppendText(info);
            }
            else if (_flag == 5)
            {
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText("发送的数据为：" +_info);
            }
            else if (_flag == 6)
            {
                richTextBox1.SelectionColor = Color.Blue;
                richTextBox1.AppendText("接受的数据为：" + _info);
            }
            else if (_flag == 7)
            {
                richTextBox1.SelectionColor = Color.Black;
                info = "刷新中...\r\n" ;
                foreach(string com in Function.GetSerialPort())
                {
                    info += com + "\r\n";
                }
                info += "刷新完成。";
                richTextBox1.AppendText(info);
            }
            else if (_flag < 0)
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText(_info);
            }
            
        }




        /// <summary>
        /// 获取radiobuttonGroup的值，默认获取全部的radiobutton值
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<string> GetAllRadioButtonValue()
        {
            List<string> list = new List<string>();
            //输出所有RadioButton的值
            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                
                GroupBox gb = groupBox1.Controls[i] as GroupBox;
                list.Add(Function.GetRadioButtonText(gb));
                //richTextBox1_toString(Function.paramvalue.error, "衰减器编号:" + gb.Name.ToString() +"\r\n");

            }

            return list;
        }

        /// <summary>
        /// 串口成功打开后执行的操作
        /// </summary>
        private void changeAfterOpen()
        {
            button1.Text = "关闭串口";
            richTextBox1_toString(Function.paramvalue.open);
            button2.Enabled = true;
            button3.Enabled = false;
            comboBox1.Enabled = false;
        }

        /// <summary>
        /// 串口成功关闭后执行的操作
        /// </summary>
        private void changeAfterClose()
        {
            button1.Text = "打开串口";
            richTextBox1_toString(Function.paramvalue.close);
            button2.Enabled = false;
            button3.Enabled = true;
            comboBox1.Enabled = true;
        }
        #endregion
        /////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////// 分界线 ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        public Form1()
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //显示所有com
            comboBox1_viewCom();
            //serialport一些属性是固定的
            SetsSerialport();
            //输出初始信息
            richTextBox1_toString(Function.paramvalue.init);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //显示所有com
            comboBox1_viewCom();
            //输出刷新内容
            richTextBox1_toString(Function.paramvalue.refresh);
        }

        /// <summary>
        /// comboBox1中内容改变时，改变serialport的name属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //设置Serial PortName
            SetSerialPortName();
            //输出信息
            richTextBox1_toString(Function.paramvalue.changePort);
        }

        /// <summary>
        /// 打开serialport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!sp.IsOpen)
            {
                try
                {
                    sp.Open();
                    changeAfterOpen();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                try
                {
                    sp.Close();
                    changeAfterClose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string str_tx = "#";
            string data_tx = "";
            List<string> allData = GetAllRadioButtonValue();
            // 由于控件添加的先后顺序问题，此时需要倒序输出
            allData.Reverse();
            foreach (string data in allData)
            {
                if ("0dB".Equals(data))
                {
                    data_tx = "0";
                }
                else if ("1dB".Equals(data))
                {
                    data_tx = "1";
                }
                else if ("2dB".Equals(data))
                {
                    data_tx = "2";
                }
                else if ("4dB".Equals(data))
                {
                    data_tx = "3";
                }
                else if ("8dB".Equals(data))
                {
                    data_tx = "4";
                }
                else if ("16dB".Equals(data))
                {
                    data_tx = "5";
                }
                else if ("31dB".Equals(data))
                {
                    data_tx = "6";
                }
                str_tx += data_tx;
            }
            str_tx += "$\r\n";
            richTextBox1_toString(Function.paramvalue.tx,str_tx);
            SendData(str_tx);
        }

        private void rx2rich(string rStr)
        {
            richTextBox1_toString(Function.paramvalue.rx, rStr+"\r\n");
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytes_num = sp.BytesToRead;
            byte[] buffer = new byte[bytes_num];
            sp.Read(buffer,0,bytes_num);
            string rStr = System.Text.Encoding.UTF8.GetString(buffer);
            rxHandler rxhandler = new rxHandler(rx2rich);
            richTextBox1.Invoke(rxhandler, new object[] { rStr });
            
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
