using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace FlowDiagrams.Dialogs
{
    public partial class SubProcessEdit : Form
    {
        public string result;
        public string comment;
        public string Filename;
        public FlowDiagram d;
        private Form1 ff;
        public string[] asm_code = new string[255];
        public int n = 0;

        public SubProcessEdit()
        {
            InitializeComponent();
        }

        public void Setup()
        {
            textBox_CodeLabel.Text = result;
            textBox_FileName.Text = Filename;
            if (d != null) ff = new Form1(d); else ff = new Form1();
            Label label_End = new Label();
            ff.Controls.Add(label_End);
            label_End.AutoSize = true;
            label_End.Location = new System.Drawing.Point(834, 424);
            label_End.Name = "label_End";
            label_End.Size = new System.Drawing.Size(40, 17);
            label_End.TabIndex = 1;
            label_End.Location = new Point(2000, 3000);//to set max size....  
            label_End.Text = "END!";
            ff.Text = Filename;
            ff.InterpretDiagram();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "fld files (*.fld)|*.fld|All files (*.*)|*.*";
            loadFileDialog.FilterIndex = 0;
            loadFileDialog.RestoreDirectory = true;
            try
            {
                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox_FileName.Text = loadFileDialog.FileName;
                    
                    Stream TestFileStream = loadFileDialog.OpenFile();
                    BinaryFormatter serializer = new BinaryFormatter();
                    d = (FlowDiagram)serializer.Deserialize(TestFileStream);
                    ff = new Form1(d);
                    ff.Text = loadFileDialog.FileName;
                    Filename = loadFileDialog.FileName;
                    //ff.ShowDialog();          
                    d = ff.d;
                    ff.InterpretDiagram();
                    for (int i = 0; i < ff.max_n; i++)
                    {
                        asm_code[n] = ff.OCRCode[i]; n++;
                    }
 
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Load Error" + e1.Message);
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            //need to check that the code label exists...
            if (textBox_CodeLabel.Text == null || textBox_CodeLabel.Text.Length == 0)
            {
                MessageBox.Show("You must give the process a unique Code label");
                e.Cancel = true;
                base.OnClosing(e);
                return;
            }
            result = textBox_CodeLabel.Text;
            for (int i = 1; i < n; i++)
            {
                asm_code[i] = asm_code[i].Replace("line", textBox_CodeLabel.Text + "line");
            }

            bool found = false;
            // the flow diagram has to have a return box...
            foreach (FlowObject f in d.BoxesList)
            {
                if (f is ReturnBox)
                {
                    found = true;
                }
            }

            if (!found) MessageBox.Show("Warning... Sub process doesn't have a return box...");
            
            base.OnClosing(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //show diagram..
            ff.ShowDialog();
        }
    }
}
