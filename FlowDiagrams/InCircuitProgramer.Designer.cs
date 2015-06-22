namespace FlowDiagrams
{
    partial class InCircuitProgramer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.button_uploadHex = new System.Windows.Forms.Button();
            this.button_initialise = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_Start = new System.Windows.Forms.Button();
            this.comboBox_ComPorts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ProgramStatus = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.DiscardNull = true;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Location = new System.Drawing.Point(22, 88);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(281, 41);
            this.textBox1.TabIndex = 0;
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(228, 163);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 5;
            this.button_send.Text = "Send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBox_send
            // 
            this.textBox_send.Location = new System.Drawing.Point(22, 164);
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.Size = new System.Drawing.Size(200, 22);
            this.textBox_send.TabIndex = 6;
            // 
            // button_uploadHex
            // 
            this.button_uploadHex.Enabled = false;
            this.button_uploadHex.Location = new System.Drawing.Point(32, 33);
            this.button_uploadHex.Name = "button_uploadHex";
            this.button_uploadHex.Size = new System.Drawing.Size(84, 58);
            this.button_uploadHex.TabIndex = 7;
            this.button_uploadHex.Text = "Program Micro";
            this.button_uploadHex.UseVisualStyleBackColor = true;
            this.button_uploadHex.Click += new System.EventHandler(this.button_uploadHex_Click);
            // 
            // button_initialise
            // 
            this.button_initialise.Location = new System.Drawing.Point(231, 33);
            this.button_initialise.Name = "button_initialise";
            this.button_initialise.Size = new System.Drawing.Size(72, 25);
            this.button_initialise.TabIndex = 49;
            this.button_initialise.Text = "Initialize";
            this.button_initialise.UseVisualStyleBackColor = true;
            this.button_initialise.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(22, 110);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(281, 23);
            this.progressBar1.TabIndex = 50;
            // 
            // button_Start
            // 
            this.button_Start.Enabled = false;
            this.button_Start.Location = new System.Drawing.Point(219, 32);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(84, 59);
            this.button_Start.TabIndex = 52;
            this.button_Start.Text = "Run from Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // comboBox_ComPorts
            // 
            this.comboBox_ComPorts.FormattingEnabled = true;
            this.comboBox_ComPorts.Location = new System.Drawing.Point(103, 33);
            this.comboBox_ComPorts.Name = "comboBox_ComPorts";
            this.comboBox_ComPorts.Size = new System.Drawing.Size(92, 24);
            this.comboBox_ComPorts.TabIndex = 54;
            this.comboBox_ComPorts.SelectedIndexChanged += new System.EventHandler(this.comboBox_ComPorts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 55;
            this.label1.Text = "Serial Port";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox_ComPorts);
            this.panel1.Controls.Add(this.button_initialise);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button_send);
            this.panel1.Controls.Add(this.textBox_send);
            this.panel1.Location = new System.Drawing.Point(22, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 213);
            this.panel1.TabIndex = 56;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 58;
            this.label4.Text = "Test Message";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 57;
            this.label3.Text = "Serial Interface";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 56;
            this.label2.Text = "Status Reply";
            // 
            // textBox_ProgramStatus
            // 
            this.textBox_ProgramStatus.Location = new System.Drawing.Point(22, 150);
            this.textBox_ProgramStatus.Name = "textBox_ProgramStatus";
            this.textBox_ProgramStatus.Size = new System.Drawing.Size(281, 22);
            this.textBox_ProgramStatus.TabIndex = 60;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.textBox_ProgramStatus);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.button_uploadHex);
            this.panel2.Controls.Add(this.button_Start);
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Location = new System.Drawing.Point(22, 228);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(373, 194);
            this.panel2.TabIndex = 57;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, -2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 17);
            this.label5.TabIndex = 58;
            this.label5.Text = "Program";
            // 
            // InCircuitProgramer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 432);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "InCircuitProgramer";
            this.Text = "In Circuit Programer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InCircuitProgramer_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBox_send;
        private System.Windows.Forms.Button button_uploadHex;
        private System.Windows.Forms.Button button_initialise;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.ComboBox comboBox_ComPorts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ProgramStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
    }
}