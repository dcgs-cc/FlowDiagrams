namespace FlowDiagrams.Dialogs
{
    partial class DecisionEdit
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
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBit_Des = new System.Windows.Forms.ComboBox();
            this.comboBoxBIT_bit = new System.Windows.Forms.ComboBox();
            this.comboBoxBit_Ins = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_Register = new System.Windows.Forms.ComboBox();
            this.button_done = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_YesSide = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radio_TestEquals = new System.Windows.Forms.RadioButton();
            this.radio_TestGT = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_byte = new System.Windows.Forms.TextBox();
            this.radio_BitTests = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_comment = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Test";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Bit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Register";
            // 
            // comboBoxBit_Des
            // 
            this.comboBoxBit_Des.FormattingEnabled = true;
            this.comboBoxBit_Des.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3",
            "S4",
            "S5",
            "S6",
            "S7"});
            this.comboBoxBit_Des.Location = new System.Drawing.Point(91, 32);
            this.comboBoxBit_Des.Name = "comboBoxBit_Des";
            this.comboBoxBit_Des.Size = new System.Drawing.Size(59, 24);
            this.comboBoxBit_Des.TabIndex = 19;
            this.comboBoxBit_Des.SelectedIndexChanged += new System.EventHandler(this.comboBoxBit_Des_SelectedIndexChanged);
            // 
            // comboBoxBIT_bit
            // 
            this.comboBoxBIT_bit.FormattingEnabled = true;
            this.comboBoxBIT_bit.Items.AddRange(new object[] {
            ":0",
            ":1",
            ":2",
            ":3",
            ":4",
            ":5",
            ":6",
            ":7"});
            this.comboBoxBIT_bit.Location = new System.Drawing.Point(91, 67);
            this.comboBoxBIT_bit.Name = "comboBoxBIT_bit";
            this.comboBoxBIT_bit.Size = new System.Drawing.Size(59, 24);
            this.comboBoxBIT_bit.TabIndex = 18;
            this.comboBoxBIT_bit.SelectedIndexChanged += new System.EventHandler(this.comboBoxBIT_bit_SelectedIndexChanged);
            // 
            // comboBoxBit_Ins
            // 
            this.comboBoxBit_Ins.FormattingEnabled = true;
            this.comboBoxBit_Ins.Items.AddRange(new object[] {
            "=1",
            "=0"});
            this.comboBoxBit_Ins.Location = new System.Drawing.Point(91, 106);
            this.comboBoxBit_Ins.Name = "comboBoxBit_Ins";
            this.comboBoxBit_Ins.Size = new System.Drawing.Size(59, 24);
            this.comboBoxBit_Ins.TabIndex = 17;
            this.comboBoxBit_Ins.SelectedIndexChanged += new System.EventHandler(this.comboBoxBit_Ins_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 17);
            this.label4.TabIndex = 21;
            this.label4.Text = "Byte";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "Register:";
            // 
            // comboBox_Register
            // 
            this.comboBox_Register.FormattingEnabled = true;
            this.comboBox_Register.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3",
            "S4",
            "S5",
            "S6",
            "S7"});
            this.comboBox_Register.Location = new System.Drawing.Point(94, 115);
            this.comboBox_Register.Name = "comboBox_Register";
            this.comboBox_Register.Size = new System.Drawing.Size(77, 24);
            this.comboBox_Register.TabIndex = 22;
            this.comboBox_Register.SelectedIndexChanged += new System.EventHandler(this.comboBox_Register_SelectedIndexChanged);
            // 
            // button_done
            // 
            this.button_done.Location = new System.Drawing.Point(18, 59);
            this.button_done.Name = "button_done";
            this.button_done.Size = new System.Drawing.Size(79, 27);
            this.button_done.TabIndex = 26;
            this.button_done.Text = "OK";
            this.button_done.UseVisualStyleBackColor = true;
            this.button_done.Click += new System.EventHandler(this.button_done_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Location = new System.Drawing.Point(9, 21);
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.Size = new System.Drawing.Size(190, 22);
            this.textBox_result.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_YesSide);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(254, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 91);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View";
            // 
            // radioButton_YesSide
            // 
            this.radioButton_YesSide.AutoSize = true;
            this.radioButton_YesSide.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton_YesSide.Location = new System.Drawing.Point(64, 55);
            this.radioButton_YesSide.Name = "radioButton_YesSide";
            this.radioButton_YesSide.Size = new System.Drawing.Size(102, 21);
            this.radioButton_YesSide.TabIndex = 3;
            this.radioButton_YesSide.Text = "YES to side";
            this.radioButton_YesSide.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(70, 28);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(96, 21);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "NO to side";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxBit_Des);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.comboBoxBit_Ins);
            this.groupBox2.Controls.Add(this.comboBoxBIT_bit);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(32, 231);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(198, 158);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bit Test Opions";
            // 
            // radio_TestEquals
            // 
            this.radio_TestEquals.AutoSize = true;
            this.radio_TestEquals.Checked = true;
            this.radio_TestEquals.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radio_TestEquals.Location = new System.Drawing.Point(19, 21);
            this.radio_TestEquals.Name = "radio_TestEquals";
            this.radio_TestEquals.Size = new System.Drawing.Size(137, 21);
            this.radio_TestEquals.TabIndex = 29;
            this.radio_TestEquals.TabStop = true;
            this.radio_TestEquals.Text = "Test: Sn = byte ?";
            this.radio_TestEquals.UseVisualStyleBackColor = true;
            this.radio_TestEquals.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radio_TestGT
            // 
            this.radio_TestGT.AutoSize = true;
            this.radio_TestGT.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radio_TestGT.Location = new System.Drawing.Point(19, 48);
            this.radio_TestGT.Name = "radio_TestGT";
            this.radio_TestGT.Size = new System.Drawing.Size(129, 21);
            this.radio_TestGT.TabIndex = 30;
            this.radio_TestGT.Text = "Test: Sn>byte ?";
            this.radio_TestGT.UseVisualStyleBackColor = true;
            this.radio_TestGT.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_byte);
            this.groupBox3.Controls.Add(this.radio_BitTests);
            this.groupBox3.Controls.Add(this.radio_TestEquals);
            this.groupBox3.Controls.Add(this.radio_TestGT);
            this.groupBox3.Controls.Add(this.comboBox_Register);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(30, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 198);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Test Type";
            // 
            // textBox_byte
            // 
            this.textBox_byte.Location = new System.Drawing.Point(94, 148);
            this.textBox_byte.Name = "textBox_byte";
            this.textBox_byte.Size = new System.Drawing.Size(77, 22);
            this.textBox_byte.TabIndex = 32;
            this.textBox_byte.Text = "00";
            this.textBox_byte.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // radio_BitTests
            // 
            this.radio_BitTests.AutoSize = true;
            this.radio_BitTests.Enabled = false;
            this.radio_BitTests.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radio_BitTests.Location = new System.Drawing.Point(19, 75);
            this.radio_BitTests.Name = "radio_BitTests";
            this.radio_BitTests.Size = new System.Drawing.Size(145, 21);
            this.radio_BitTests.TabIndex = 31;
            this.radio_BitTests.Text = "Bit Tests (non std)";
            this.radio_BitTests.UseVisualStyleBackColor = true;
            this.radio_BitTests.CheckedChanged += new System.EventHandler(this.radio_BitTests_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_result);
            this.groupBox4.Controls.Add(this.button_Cancel);
            this.groupBox4.Controls.Add(this.button_done);
            this.groupBox4.Location = new System.Drawing.Point(254, 149);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 105);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Result:";
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(120, 59);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(79, 27);
            this.button_Cancel.TabIndex = 35;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_comment
            // 
            this.textBox_comment.Location = new System.Drawing.Point(9, 21);
            this.textBox_comment.Multiline = true;
            this.textBox_comment.Name = "textBox_comment";
            this.textBox_comment.Size = new System.Drawing.Size(190, 68);
            this.textBox_comment.TabIndex = 34;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox_comment);
            this.groupBox5.Location = new System.Drawing.Point(254, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(211, 100);
            this.groupBox5.TabIndex = 36;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Comment:";
            // 
            // DecisionEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 412);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DecisionEdit";
            this.Text = "DecisionEdit";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBit_Des;
        private System.Windows.Forms.ComboBox comboBoxBIT_bit;
        private System.Windows.Forms.ComboBox comboBoxBit_Ins;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_Register;
        private System.Windows.Forms.Button button_done;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_YesSide;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radio_TestEquals;
        private System.Windows.Forms.RadioButton radio_TestGT;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radio_BitTests;
        private System.Windows.Forms.TextBox textBox_byte;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_comment;
        private System.Windows.Forms.GroupBox groupBox5;

    }
}