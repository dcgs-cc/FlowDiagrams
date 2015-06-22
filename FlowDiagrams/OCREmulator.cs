using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SerialProgrammer;

namespace FlowDiagrams
{
    public partial class OCREmulator : Form
    {
        private InCircuitProgramer form1 = new InCircuitProgramer();
        private string FileName = "";
        public OCREmulator()
        {
            InitializeComponent();
        }

        #region File Handling
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            loadFileDialog.FilterIndex = 0;
            loadFileDialog.RestoreDirectory = true;
            try
            {
                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.Stream TestFileStream = loadFileDialog.OpenFile();           
                    System.IO.StreamReader sr = new System.IO.StreamReader(TestFileStream);
                    richTextBox_OCR.Text = sr.ReadToEnd();
                    TestFileStream.Close();
                    this.Invalidate();
                    FileName = loadFileDialog.FileName;

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Load Error" + e1.Message);
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileName == "") { saveAsToolStripMenuItem_Click(sender, e); return; }
            try
            {
                StreamWriter strw1 = new StreamWriter(FileName);
                strw1.Write(richTextBox_OCR.Text);
                strw1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Save Error" + e1.Message);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf1 = new SaveFileDialog();
            sf1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sf1.FilterIndex = 0;
            sf1.RestoreDirectory = true;
            try
            {
                if (sf1.ShowDialog() == DialogResult.OK)
                {
                    FileName = sf1.FileName;
                    Stream TestFileStream = sf1.OpenFile();
                    StreamWriter strw1 = new StreamWriter(TestFileStream);
                    strw1.Write(richTextBox_OCR.Text);
                    strw1.Close();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Save Error" + e1.Message);
            }
        }
        #endregion

        private void inCircuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //enable the serial programmer....
            GenerateAssembler();
            form1.ShowDialog();
            
        }
        private bool GenerateAssembler()
        {
            string[] output_split = new string[20];
            char[] c1 = new char[1]; c1[0] = (char)0x0a;
            output_split = richTextBox_OCR.Text.Split(c1);
            int i = 0;
            foreach (string s5 in output_split)
            {
                form1.program1.OCRCode[i] = s5; i++;
            }
            string s1 = "";
            form1.program1.max_OCRlines = i;
            if (!form1.program1.GenerateMC(ref s1,false))
            {
                //error in code.....
                FlowDiagrams.Dialogs.ErrorDisplay err1 = new Dialogs.ErrorDisplay();
                err1.ErrorText = s1; err1.Initialise();
                err1.ShowDialog(); return false;
            }
            else
            {
                //for (int j = 0; j < form1.program1.max_ASMlines; j++) AssemblerTextBox.AppendText(form1.program1.AssemblerCode[j]);
            }
            return true;
        }

    }
}
