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
    public partial class ScalingDialog : Form
    {
        public float scale;
        public ScalingDialog()
        {
            InitializeComponent();
        }

        public void setup(float Scale)
        {
            numericUpDown1.Value = (decimal)Scale;
            scale = Scale;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            scale = (float)numericUpDown1.Value;
            base.OnClosing(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
