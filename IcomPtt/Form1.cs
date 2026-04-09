using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace IcomPtt
{
    public partial class Form1 : Form
    {
        bool xmit = false;
        byte[] bytesxmit;
        byte[] bytesrecv;
        Properties.Settings appSettings;

        public Form1()
        {
            InitializeComponent();
        }

        private void serialPortIn_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            if (e.EventType == SerialPinChange.CtsChanged)
            {
                CheckPtt();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            appSettings = new Properties.Settings();

            string[] ports = SerialPort.GetPortNames();
            foreach (string portName in ports)
            {
                comboBox1.Items.Add(portName);
                comboBox2.Items.Add(portName);
            }
            comboBox3.Items.Add("57600");
            comboBox3.Items.Add("38400");
            comboBox3.Items.Add("19200");
            comboBox3.Items.Add("9600");
            comboBox3.Items.Add("4800");
            comboBox3.Items.Add("2400");
            comboBox3.Items.Add("1200");

            comboBox4.Items.Add("88"); //IC-7100
            comboBox4.Items.Add("76"); //IC-7200
            comboBox4.Items.Add("7C"); //IC-9100

            comboBox1.SelectedItem = appSettings.ControlPort;
            comboBox2.SelectedItem = appSettings.RadioPort;
            comboBox3.SelectedItem = appSettings.RadioPortBuad;
            comboBox4.SelectedItem = appSettings.RadioAddr;

            this.label1.Text = "PTT off";
        }

        private void CheckPtt()
        {
            if (serialPortIn.IsOpen & serialPortOut.IsOpen)
            {
                if ((serialPortIn.CtsHolding == true) | this.xmit)
                {
                    serialPortOut.Write(this.bytesxmit, 0, 8);
                    label1.Text = "PTT on";
                }
                else
                {
                    serialPortOut.Write(this.bytesrecv, 0, 8);
                    label1.Text = "PTT off";
                }
            }
            else
            {
                MessageBox.Show("Serial ports are closed, please check settings and click Open.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPortIn.IsOpen)
            {
                serialPortIn.Close();
            }
            serialPortIn.PortName = comboBox1.SelectedItem.ToString();
            serialPortIn.Open();

            if (serialPortOut.IsOpen)
            {
                serialPortOut.Close();
            }
            serialPortOut.PortName = comboBox2.SelectedItem.ToString();
            serialPortOut.BaudRate = Convert.ToInt32(comboBox3.SelectedItem);
            serialPortOut.DataBits = 8;
            serialPortOut.StopBits = StopBits.One;
            serialPortOut.Parity = Parity.None;
            serialPortOut.Open();

            this.UpdateLabels();
            this.CreateMessages();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UpdateLabels()
        {
            if (this.serialPortIn.IsOpen)
            {
                this.label2.Text = "Control Port (open)";
            }
            else
            {
                this.label2.Text = "Control Port";
            }

            if (this.serialPortOut.IsOpen)
            {
                this.label3.Text = "Radio CAT Port (open)";
            }
            else
            {
                this.label3.Text = "Radio CAT Port";
            }
        }

        private void CreateMessages()
        {
            //Transmit command string;
            bytesxmit = new byte[10];
            bytesxmit[0] = 254;
            bytesxmit[1] = 254;
            bytesxmit[2] = Convert.ToByte(comboBox4.SelectedItem.ToString(), 16); //0x88;
            bytesxmit[3] = 224;
            bytesxmit[4] = 28;
            bytesxmit[5] = 0;
            bytesxmit[6] = 1;
            bytesxmit[7] = 253;

            //Recieve command string;
            bytesrecv = new byte[10];
            bytesrecv[0] = 254;
            bytesrecv[1] = 254;
            bytesrecv[2] = Convert.ToByte(comboBox4.SelectedItem.ToString(), 16); //0x88; 
            bytesrecv[3] = 224;
            bytesrecv[4] = 28;
            bytesrecv[5] = 0;
            bytesrecv[6] = 0;
            bytesrecv[7] = 253;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            xmit = true;
            this.CheckPtt();
        }

        private void button2_KeyUp(object sender, KeyEventArgs e)
        {
            xmit = false;
            this.CheckPtt();
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            xmit = true;
            this.CheckPtt();
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            xmit = false;
            this.CheckPtt();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close input port to stop / prevent more triggering
            serialPortIn.Close();

            //If radio COM port is open, transmit the sequence to stop transmitting (just in case), then close it too.
            if (serialPortOut.IsOpen)
            {
                serialPortOut.Write(this.bytesrecv, 0, 8);
                serialPortOut.Close();
            }

            //Now normal closing of form proceeds.
        }
    }
}
