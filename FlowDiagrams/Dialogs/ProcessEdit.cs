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
    public partial class ProcessEdit : Form
    {
        public string result = "";
        public string comment = "";
        
        public ProcessType op_type;
        public string[] asm_code = new string[5]; 
        public int n = 0;

        public string sn = "";
        public string sm = "";
        public string b1 = "";

        public ProcessEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            textBox_byte.Text = b1;
            comboBox_Sn.SelectedIndex = comboBox_Sn.Items.IndexOf(sn);
            comboBox_Sm.SelectedIndex = comboBox_Sm.Items.IndexOf(sm);
            switch (op_type)
            {
                case ProcessType.unknown:
                    break;
                case ProcessType.movi: radioButton_movi.Checked = true;
                    break;
                case ProcessType.mov: radioButton_mov.Checked = true;
                    break;
                case ProcessType.addi: radioButton_addi.Checked = true;
                    break;
                case ProcessType.subi: radioButton_subi.Checked = true;
                    break;
                case ProcessType.wait: radioButton_wait.Checked = true;
                    break;
                case ProcessType.and: radioButton_and.Checked = true;
                    break;
                case ProcessType.orr: radioButton_orr.Checked = true;
                    break;
                case ProcessType.xor: radioButton_xor.Checked = true;
                    break;
                case ProcessType.add: radioButton_add.Checked = true;
                    break;
                case ProcessType.sub: radioButton_sub.Checked = true;
                    break;
                default:
                    break;
            }
            textBox_comment.Text = comment;
            textBox_result.Text = result;
            UpdateResult();

        }

        public void CheckHexByte(string s)
        {
            if (s.Length > 0)
            {
                try
                {
                    int i = System.Convert.ToInt32(s, 16);
                    if (i < 0) throw new System.Exception();
                    if (i > 0xff) throw new System.Exception();
                }
                catch
                {
                    MessageBox.Show("Byte " +s + " is not a valid Hexdecimal value between 0 and FF");
                }
            }
        }

        public void UpdateResult()
        {
            if (comboBox_Sm.SelectedIndex < 0) return;
            string s = "";
            sn = comboBox_Sn.SelectedItem.ToString();
            sm = comboBox_Sm.SelectedItem.ToString();
            b1 = textBox_byte.Text;
            switch (op_type)
            {
                case ProcessType.movi: 
                    s = sn + " = " + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:    movi   "+sn+","+b1; n = 1;
                    break;
                case ProcessType.mov: 
                    s = sn + " = " + sm;
                    asm_code[0] = "linexx:     mov  " + sn + "," + sm; n = 1;   
                    break;
                case ProcessType.addi: 
                    s = sn + " = " + sn + "+" + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:    movi   s8," + b1; n = 1;
                    asm_code[1] = "          add    "+sn+",s8"; n = 2;
                    break;
                case ProcessType.subi: 
                    s = sn + " = " + sn + "-" + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:    movi   s8," + b1; n = 1;
                    asm_code[1] = "          sub    " + sn + ",s8"; n = 2;
                    break;
                case ProcessType.wait: 
                    s = "Wait " + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:      MOVI S8," + b1;
                    asm_code[1] = "Dellinexx:         RCALL wait1ms";
                    asm_code[2] = "            DEC S8";
                    asm_code[3] = "            JNZ Dellinexx"; n = 4;
                    break;
                case ProcessType.and: 
                    s = sn + "=" + sn + " AND " + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:      MOVI S8," + b1;
                    asm_code[1] = "    and     " + sn + ",S8"; n = 2;
                    break;
                case ProcessType.orr: 
                    s = sn + "=" + sn + " OR " + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:      MOVI S8," + b1;
                    asm_code[1] = "    orr     " + sn + ",S8"; n = 2;
                    break;     
                case ProcessType.xor: 
                    s = sn + "=" + sn + " XOR " + b1; CheckHexByte(b1);
                    asm_code[0] = "linexx:      MOVI S8," + b1;
                    asm_code[1] = "    eor     " + sn + ",S8"; n = 2;
                    break;
                case ProcessType.add: 
                    s = sn + "=" + sn + " + " + sm;
                    asm_code[0] = "linexx:     add     " + sn + "," + sm; n = 1;
                    break;
                case ProcessType.sub: 
                    s = sn + "=" + sn + " - " + sm;
                    asm_code[0] = "linexx:     sub     " + sn + "," + sm; n = 1;
                    break;            
                default: break;
            }

            textBox_result.Text = s;
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.mov; UpdateResult();
        }
        private void radioButton_movi_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.movi; UpdateResult();
        }
        private void radioButton_addi_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.addi; UpdateResult();
        }
        private void radioButton_subi_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.subi; UpdateResult();
        }
        private void radioButton_wait_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.wait; UpdateResult();
        }
        private void radioButton_and_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.and; UpdateResult();
        }
        private void radioButton_xor_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.xor; UpdateResult();
        }
        private void radioButton_orr_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.orr; UpdateResult();
        }
        private void radioButton_add_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.add; UpdateResult();
        }
        private void radioButton_sub_CheckedChanged(object sender, EventArgs e)
        {
            op_type = ProcessType.sub; UpdateResult();
        }
        private void textBox_byte_TextChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }
        private void comboBox_Sm_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }
        private void comboBox_Sn_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateResult();
        }

        private void button_done_Click(object sender, EventArgs e)
        {
            result = textBox_result.Text;
            comment = textBox_comment.Text;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
    }
}
