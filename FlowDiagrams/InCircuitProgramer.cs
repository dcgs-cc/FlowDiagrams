using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialProgrammer;

namespace FlowDiagrams
{
    public partial class InCircuitProgramer : Form
    {
        public SerialProgrammer.SerialProgrammer program1 = new SerialProgrammer.SerialProgrammer();
        public InCircuitProgramer()
        {
            InitializeComponent();
            string[] SerialPortEnums = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string s in SerialPortEnums) comboBox_ComPorts.Items.Add(s);
            try
            {
                comboBox_ComPorts.SelectedIndex = 0;
            }
            catch
            {
            }
            //serialPort1.ReceivedBytesThreshold = 17;//this is length of response from brak///
        }
        public void Setup()
        {
            string s = "";
            program1.serialPort1 = serialPort1;
            //todo  check folowing
            if (!program1.GenerateMC(ref s,false))
            {
                textBox1.Text = s;
            }
            s=program1.IntelCode();
            s = s;
        }
        private void button_send_Click(object sender, EventArgs e)
        {
            textBox1.Text = program1.SendMessage("@@@@" + textBox_send.Text);
        }
        private void button_uploadHex_Click(object sender, EventArgs e)
        {
            textBox_ProgramStatus.Text = "Starting Program sequence";
            if (program1.ProgramPIC(UpdateProgress)) textBox_ProgramStatus.Text = "Programming successful";
            else textBox_ProgramStatus.Text = "Programming Failed";
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = program1.SendMessage("@@@@Z") + Environment.NewLine;
            if (!textBox1.Text.Contains("Ready"))
            {//try twice
                textBox1.Text = program1.SendMessage("@@@@Z") + Environment.NewLine;
                if (!textBox1.Text.Contains("Ready"))
                {
                    MessageBox.Show("Unable to communicate with PIC. Check the serial connection is in place and reset the PIC and try again."); return;
                }
            }

            button_uploadHex.Enabled = true;
            button_send.Enabled = true;
            button_Start.Enabled = true;

        }
        public void UpdateProgress(int max, int val, string text)
        {
            progressBar1.Maximum = max; progressBar1.Value = val;
            textBox_ProgramStatus.Text = text;
            Application.DoEvents();
        }
        private void InCircuitProgramer_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }
        private void button_Start_Click(object sender, EventArgs e)
        {
            //decide if we have to wait for breaks..... and if the PIC version is high enough
            if (program1.CodeHasBreaks())
            {
                string s = program1.SendMessage("@@@@V");  //response is Version 1.x
                //PIC version needs to be >1.3
                s = s.Substring(8);
                double v = System.Convert.ToDouble(s);
                if (v < 1.3)
                {
                    MessageBox.Show("Code has Breaks, but PIC firmware version is <1.3 and needs to be upgraded to run break code","Error");
                    return;
                }
                Dialogs.BreakWindow b1 = new Dialogs.BreakWindow();
                b1.program1 = program1;
                b1.ShowDialog();
            }
            else
            {
                string s = program1.SendMessage("@@@@J0500");
            }
            
        }
        private void comboBox_ComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort1.PortName = comboBox_ComPorts.SelectedItem.ToString();
            serialPort1.Open();
        }
    }
}