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
    public partial class InputEdit : Form
    {
        public string result="";
        public string[] asm_code = new string[5];
        public int n = 0;
        public string comment = "";
        public InputEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            textBox_comment.Text = comment;
            comboBox1.SelectedIndex=comboBox1.Items.IndexOf(result);
        }
        private void button_done_Click(object sender, EventArgs e)
        {
            result = comboBox1.SelectedItem.ToString();
            asm_code[0] = "linexx:      IN " + comboBox1.SelectedItem.ToString().Substring(6) + ",I"; 
            n = 1;
            comment = textBox_comment.Text;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
