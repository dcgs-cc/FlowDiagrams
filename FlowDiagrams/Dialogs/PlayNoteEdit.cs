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
    public partial class PlayNoteEdit : Form

    {

        public int frequency;
        public int Length;
        public PlayNoteEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            numericUpDown1.Value = frequency;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // so have a frequency
            frequency = (int)numericUpDown1.Value;
            Length = (int)(numericUpDown2.Value * 10);
            Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
