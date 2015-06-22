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
    public partial class ParseOutput : Form
    {
        
        public ParseOutput()
        {
            InitializeComponent();
        }
        public void Setup(string text)
        {
            richTextBox1.Text = text;
        }
    }
}
