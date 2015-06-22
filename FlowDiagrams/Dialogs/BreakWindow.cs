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
    public partial class BreakWindow : Form
    {
        public SerialProgrammer.SerialProgrammer program1;
        public SerialProgrammer.SerialProgrammer.OCRmodel ocr_model = new SerialProgrammer.SerialProgrammer.OCRmodel();
        bool waitbreak = false;
        bool inbreak = false;
        public BreakWindow()
        {
            InitializeComponent();
            textBox_Status.Text = "Ready to run.";
        }

        private void button_Run_Click(object sender, EventArgs e)
        {
            string s = program1.SendMessage("@@@@J0500");
            textBox_Status.Text = "Running: waiting for break";
            waitbreak = true; s = "";
            button_cancel.Text = "Exit";
            button_Run.Enabled = false;
            inbreak = false;
            do
            {
                if (program1.WaitBreakMessage(ref ocr_model))
                {
                    textBox_S0.Text = ocr_model.S[0];
                    textBox_S1.Text = ocr_model.S[1];
                    textBox_S2.Text = ocr_model.S[2]; 
                    textBox_S3.Text = ocr_model.S[3];
                    textBox_S4.Text = ocr_model.S[4];
                    textBox_S5.Text = ocr_model.S[5];
                    textBox_S6.Text = ocr_model.S[6];
                    textBox_S7.Text = ocr_model.S[7];
                    textBox_InputPort.Text = ocr_model.InputPort;
                    textBox_OutputPort.Text = ocr_model.OutputPort;
                    inbreak = true;
                    button_Continue.Enabled = true;
                    textBox_BreakNo.Text = ocr_model.W;
                    textBox_Z.Text = ocr_model.Z;
                    textBox_Status.Text = "At break point " + ocr_model.W;
                    button_cancel.Text = "Exit/Reset";
                }
                if (!inbreak) button_cancel.Text = "Exit";
                Application.DoEvents();
            } 
           while (waitbreak);
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            string s="";
            waitbreak = false;
            if(inbreak) s = program1.SendMessage("@@@@U");//reset
            if (button_Run.Enabled) this.Close();//not yet running
        }

        private void button_Continue_Click(object sender, EventArgs e)
        {
            program1.SendMessage_NoReply("@@@@C");
            inbreak = false;
            button_Continue.Enabled = false;
            textBox_Status.Text = "Running: waiting for break";
        }
    }
}
