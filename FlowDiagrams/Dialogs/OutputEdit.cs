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
   
    public partial class OutputEdit : Form
    {
        public string result;
        public string comment = "";
        public string[] asm_code = new string[5];
        public int n = 0;
        public OutputEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            textBox_comment.Text = comment;
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(result);
        }

        private void button_done_Click(object sender, EventArgs e)
        {
            result = comboBox1.SelectedItem.ToString();
            asm_code[0] = "linexx:     OUT Q," + comboBox1.SelectedItem.ToString().Substring(4);
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
