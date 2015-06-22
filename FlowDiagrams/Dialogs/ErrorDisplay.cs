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
    public partial class ErrorDisplay : Form
    {
        public string ErrorText;
        public ErrorDisplay()
        {
            InitializeComponent();
        }
        public void Initialise()
        {
            label_text.Text = ErrorText;
        }
    }
}
