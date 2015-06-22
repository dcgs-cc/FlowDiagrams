using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlowDiagrams.Dialogs
{
    public partial class OutputPinEdit : Form
    {
        public string result;//ie which pin
        public bool state; // ie on or off
        public string comment = "";
        public string[] asm_code = new string[5];
        public int n = 1;

        public OutputPinEdit()
        {
            InitializeComponent();
        }
        public void Setup()
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(result);
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf("OFF");
            if (state) comboBox2.SelectedIndex = comboBox2.Items.IndexOf("ON");
            UpdateASM();
        }

        private void UpdateASM()
        {
            int i = comboBox1.SelectedIndex;
            if (state)
            {
                asm_code[0] = "linexx:     BTS  Q," + i.ToString();
            }
            else 
            {
                asm_code[0] = "linexx:     BTC  Q," + i.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            result = comboBox1.SelectedItem.ToString(); UpdateASM();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "ON") 
                state = true; 
            else 
                state = false;
            UpdateASM();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
