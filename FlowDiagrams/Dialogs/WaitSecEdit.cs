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
    public partial class WaitSecEdit : Form
    {
        public decimal wait;
        public WaitSecEdit()
        {
            InitializeComponent();
        }
        public void Setup()
        {
            textBox1.Text = wait.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal d = System.Convert.ToDecimal(textBox1.Text);
                //want to round to 10 ms...
                int i = (int)(d * 100);
                wait=(decimal)i/100;
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
