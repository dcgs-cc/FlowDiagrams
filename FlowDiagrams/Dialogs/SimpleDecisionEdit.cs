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
    public partial class SimpleDecisionEdit : Form
    {
        public string pin;
        public string[] asm_code = new string[6];
        public int n = 1;
        public SimpleDecisionEdit()
        {
            InitializeComponent();
        }
        public void Setup()
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(pin);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateASm()
        {
            int i=(int)Math.Pow(2, comboBox1.SelectedIndex);
            asm_code[0] = "linexx:  movi S9," + i.ToString("X");
            pin = comboBox1.SelectedItem.ToString();
            asm_code[1] = "         in   S8,I";
            asm_code[2] = "         and  S8,S9";
            asm_code[3] = "         jz   lineyy";
            asm_code[4] = "         jp   linezz";
            n = 5;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateASm();
        }
    }
}
