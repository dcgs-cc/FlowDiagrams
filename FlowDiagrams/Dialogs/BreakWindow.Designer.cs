namespace FlowDiagrams.Dialogs
{
    partial class BreakWindow
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
            this.textBox_BreakNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Status = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_S0 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_S1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_S2 = new System.Windows.Forms.TextBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_Continue = new System.Windows.Forms.Button();
            this.button_Run = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_S3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_S7 = new System.Windows.Forms.TextBox();
            this.textBox_S6 = new System.Windows.Forms.TextBox();
            this.textBox_S5 = new System.Windows.Forms.TextBox();
            this.textBox_S4 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_Z = new System.Windows.Forms.TextBox();
            this.textBox_OutputPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_InputPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_BreakNo
            // 
            this.textBox_BreakNo.Location = new System.Drawing.Point(113, 112);
            this.textBox_BreakNo.Name = "textBox_BreakNo";
            this.textBox_BreakNo.ReadOnly = true;
            this.textBox_BreakNo.Size = new System.Drawing.Size(40, 22);
            this.textBox_BreakNo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Break Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Status:";
            // 
            // textBox_Status
            // 
            this.textBox_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Status.Location = new System.Drawing.Point(56, 73);
            this.textBox_Status.Name = "textBox_Status";
            this.textBox_Status.ReadOnly = true;
            this.textBox_Status.Size = new System.Drawing.Size(321, 22);
            this.textBox_Status.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "S0";
            // 
            // textBox_S0
            // 
            this.textBox_S0.Location = new System.Drawing.Point(40, 158);
            this.textBox_S0.Name = "textBox_S0";
            this.textBox_S0.ReadOnly = true;
            this.textBox_S0.Size = new System.Drawing.Size(40, 22);
            this.textBox_S0.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "S1";
            // 
            // textBox_S1
            // 
            this.textBox_S1.Location = new System.Drawing.Point(39, 196);
            this.textBox_S1.Name = "textBox_S1";
            this.textBox_S1.ReadOnly = true;
            this.textBox_S1.Size = new System.Drawing.Size(40, 22);
            this.textBox_S1.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "S2";
            // 
            // textBox_S2
            // 
            this.textBox_S2.Location = new System.Drawing.Point(39, 231);
            this.textBox_S2.Name = "textBox_S2";
            this.textBox_S2.ReadOnly = true;
            this.textBox_S2.Size = new System.Drawing.Size(40, 22);
            this.textBox_S2.TabIndex = 9;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(124, 22);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(82, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "Reset/Exit";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_Continue
            // 
            this.button_Continue.Enabled = false;
            this.button_Continue.Location = new System.Drawing.Point(233, 22);
            this.button_Continue.Name = "button_Continue";
            this.button_Continue.Size = new System.Drawing.Size(82, 23);
            this.button_Continue.TabIndex = 12;
            this.button_Continue.Text = "Continue";
            this.button_Continue.UseVisualStyleBackColor = true;
            this.button_Continue.Click += new System.EventHandler(this.button_Continue_Click);
            // 
            // button_Run
            // 
            this.button_Run.Location = new System.Drawing.Point(24, 22);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(82, 23);
            this.button_Run.TabIndex = 13;
            this.button_Run.Text = "Run Code";
            this.button_Run.UseVisualStyleBackColor = true;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(146, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "S5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(146, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "S4";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 17);
            this.label9.TabIndex = 14;
            this.label9.Text = "S3";
            // 
            // textBox_S3
            // 
            this.textBox_S3.Location = new System.Drawing.Point(39, 266);
            this.textBox_S3.Name = "textBox_S3";
            this.textBox_S3.ReadOnly = true;
            this.textBox_S3.Size = new System.Drawing.Size(40, 22);
            this.textBox_S3.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(146, 274);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "S7";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(146, 234);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 17);
            this.label11.TabIndex = 18;
            this.label11.Text = "S6";
            // 
            // textBox_S7
            // 
            this.textBox_S7.Location = new System.Drawing.Point(176, 268);
            this.textBox_S7.Name = "textBox_S7";
            this.textBox_S7.ReadOnly = true;
            this.textBox_S7.Size = new System.Drawing.Size(40, 22);
            this.textBox_S7.TabIndex = 23;
            // 
            // textBox_S6
            // 
            this.textBox_S6.Location = new System.Drawing.Point(176, 234);
            this.textBox_S6.Name = "textBox_S6";
            this.textBox_S6.ReadOnly = true;
            this.textBox_S6.Size = new System.Drawing.Size(40, 22);
            this.textBox_S6.TabIndex = 22;
            // 
            // textBox_S5
            // 
            this.textBox_S5.Location = new System.Drawing.Point(176, 196);
            this.textBox_S5.Name = "textBox_S5";
            this.textBox_S5.ReadOnly = true;
            this.textBox_S5.Size = new System.Drawing.Size(40, 22);
            this.textBox_S5.TabIndex = 21;
            // 
            // textBox_S4
            // 
            this.textBox_S4.Location = new System.Drawing.Point(177, 160);
            this.textBox_S4.Name = "textBox_S4";
            this.textBox_S4.ReadOnly = true;
            this.textBox_S4.Size = new System.Drawing.Size(40, 22);
            this.textBox_S4.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(205, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 17);
            this.label12.TabIndex = 24;
            this.label12.Text = "Z flag";
            // 
            // textBox_Z
            // 
            this.textBox_Z.Location = new System.Drawing.Point(255, 112);
            this.textBox_Z.Name = "textBox_Z";
            this.textBox_Z.ReadOnly = true;
            this.textBox_Z.Size = new System.Drawing.Size(40, 22);
            this.textBox_Z.TabIndex = 25;
            // 
            // textBox_OutputPort
            // 
            this.textBox_OutputPort.Location = new System.Drawing.Point(113, 339);
            this.textBox_OutputPort.Name = "textBox_OutputPort";
            this.textBox_OutputPort.ReadOnly = true;
            this.textBox_OutputPort.Size = new System.Drawing.Size(40, 22);
            this.textBox_OutputPort.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 28;
            this.label3.Text = "Output Port";
            // 
            // textBox_InputPort
            // 
            this.textBox_InputPort.Location = new System.Drawing.Point(113, 310);
            this.textBox_InputPort.Name = "textBox_InputPort";
            this.textBox_InputPort.ReadOnly = true;
            this.textBox_InputPort.Size = new System.Drawing.Size(40, 22);
            this.textBox_InputPort.TabIndex = 27;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 313);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 17);
            this.label13.TabIndex = 26;
            this.label13.Text = "Input Port";
            // 
            // BreakWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 413);
            this.Controls.Add(this.textBox_OutputPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_InputPort);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_Z);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox_S7);
            this.Controls.Add(this.textBox_S6);
            this.Controls.Add(this.textBox_S5);
            this.Controls.Add(this.textBox_S4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox_S3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_Run);
            this.Controls.Add(this.button_Continue);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_S2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_S1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_S0);
            this.Controls.Add(this.textBox_Status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_BreakNo);
            this.Name = "BreakWindow";
            this.Text = "BreakWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_BreakNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Status;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_S0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_S1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_S2;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_Continue;
        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_S3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_S7;
        private System.Windows.Forms.TextBox textBox_S6;
        private System.Windows.Forms.TextBox textBox_S5;
        private System.Windows.Forms.TextBox textBox_S4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_Z;
        private System.Windows.Forms.TextBox textBox_OutputPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_InputPort;
        private System.Windows.Forms.Label label13;
    }
}