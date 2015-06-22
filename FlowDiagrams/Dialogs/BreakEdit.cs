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
    public partial class BreakEdit : Form
    {
        public string text;
        public FlowDiagrams.BreakBox box;
        public BreakEdit()
        {
            InitializeComponent();

        }

        public void Setup()
        {
            textBox_Text.Text = box.text;
            textBox_number.Text = box.break_number.ToString("X");
        }

        private void textBox_number_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            string s = textBox_number.Text;
            if (s.Length > 0)
            {
                try
                {
                    i = System.Convert.ToInt32(s, 16);
                    if (i < 0) throw new System.Exception();
                    if (i > 0xff) throw new System.Exception();

                    box.text = "Break " + s;
                    box.Asmcode[0] = "linexx:   movi  S8," + s;
                    box.Asmcode[1] = "          RCALL  BREAK"; box.n_asm = 2;
                    box.break_number = i;
                    textBox_Text.Text = box.text;
                }
                catch
                {
                    MessageBox.Show("Byte " + s + " is not a valid Hexdecimal value between 0 and FF");
                }
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
