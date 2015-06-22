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
    public partial class DecisionEdit : Form
    {
        public string result;
        public bool YesSide = true;
        public string[] asm_code = new string[15];
        public int n = 0;
        public string comment;
        public DecisionEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            textBox_result.Text = result;
            radioButton_YesSide.Checked = YesSide;
            comboBox_Register.SelectedIndex = 0;
            textBox_comment.Text = comment;
            comboBox_Register.SelectedIndex=comboBox_Register.Items.IndexOf(result.Substring(0,2));
            if (result.Contains(">")) radio_TestGT.Checked = true;
            textBox_byte.Text = result.Substring(5);
        }



        private void button_done_Click(object sender, EventArgs e)
        {
            result = textBox_result.Text; Close();
            YesSide = false;
            if (radioButton_YesSide.Checked) YesSide = true;
            comment = textBox_comment.Text;
        }

        private void UpdateResult()
        {
            if (comboBox_Register.SelectedIndex < 0)return;
            string s = ""; string b = textBox_byte.Text;
            string sn =comboBox_Register.SelectedItem.ToString();
            if ((radio_TestEquals.Checked)||(radio_TestGT.Checked))
            {
                string s1 = " = "; if (radio_TestGT.Checked) s1 = " > ";
                s += sn + s1 + b;
                if (textBox_byte.Text.Length > 0)
                {
                    try
                    {
                        int i = System.Convert.ToInt32(textBox_byte.Text, 16);
                        if (i < 0) throw new System.Exception();
                        if (i > 0xff) throw new System.Exception();
                    }
                    catch
                    {
                        MessageBox.Show("Byte " + textBox_byte.Text + " is not a valid Hexdecimal value between 0 and FF");
                    }
                }
                asm_code[0] = "linexx:    movi  S8," + b;
                asm_code[1] = "           sub   S8," + sn;
                asm_code[2] = "           jz    lineyy";
                asm_code[3] = "           jp    linezz"; n = 4;

                if (radio_TestGT.Checked)
                {
                    //oh dear .. not easy to write in OCR speak....
                    //will invent temp new jgt operation...
                    asm_code[0] = "linexx:    movi  S8," + b;             
                    asm_code[1] = "           sub   S8," + sn;//test if eq...
                    asm_code[2] = "           jgt    lineyy";//test true... 
                    asm_code[3] = "           jp     linezz";//test false
                    n = 4;
                    //!!!!!!!!!!!!!

                }


            }
            if (radio_BitTests.Checked)
            {
                if (comboBoxBit_Des.SelectedIndex >= 0) s += comboBoxBit_Des.SelectedItem;
                if (comboBoxBIT_bit.SelectedIndex >= 0) s += comboBoxBIT_bit.SelectedItem;
                if (comboBoxBit_Ins.SelectedIndex >= 0) s += comboBoxBit_Ins.SelectedItem;

            }
            textBox_result.Text = s;
        }

        private void comboBoxBit_Des_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void comboBoxBIT_bit_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void comboBoxBit_Ins_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void comboBox_Register_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }


        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void radio_BitTests_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
