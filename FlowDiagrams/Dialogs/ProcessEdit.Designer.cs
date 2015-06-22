namespace FlowDiagrams.Dialogs
{
    partial class ProcessEdit
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_sub = new System.Windows.Forms.RadioButton();
            this.radioButton_add = new System.Windows.Forms.RadioButton();
            this.radioButton_orr = new System.Windows.Forms.RadioButton();
            this.radioButton_xor = new System.Windows.Forms.RadioButton();
            this.radioButton_and = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_wait = new System.Windows.Forms.RadioButton();
            this.radioButton_mov = new System.Windows.Forms.RadioButton();
            this.radioButton_addi = new System.Windows.Forms.RadioButton();
            this.radioButton_subi = new System.Windows.Forms.RadioButton();
            this.radioButton_movi = new System.Windows.Forms.RadioButton();
            this.comboBox_Sn = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_Sm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_byte = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_done = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_comment = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_sub);
            this.groupBox1.Controls.Add(this.radioButton_add);
            this.groupBox1.Controls.Add(this.radioButton_orr);
            this.groupBox1.Controls.Add(this.radioButton_xor);
            this.groupBox1.Controls.Add(this.radioButton_and);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_wait);
            this.groupBox1.Controls.Add(this.radioButton_mov);
            this.groupBox1.Controls.Add(this.radioButton_addi);
            this.groupBox1.Controls.Add(this.radioButton_subi);
            this.groupBox1.Controls.Add(this.radioButton_movi);
            this.groupBox1.Location = new System.Drawing.Point(35, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 380);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Type";
            // 
            // radioButton_sub
            // 
            this.radioButton_sub.AutoSize = true;
            this.radioButton_sub.Location = new System.Drawing.Point(9, 320);
            this.radioButton_sub.Name = "radioButton_sub";
            this.radioButton_sub.Size = new System.Drawing.Size(116, 21);
            this.radioButton_sub.TabIndex = 10;
            this.radioButton_sub.Text = " Sn = Sn - Sm";
            this.radioButton_sub.UseVisualStyleBackColor = true;
            this.radioButton_sub.CheckedChanged += new System.EventHandler(this.radioButton_sub_CheckedChanged);
            // 
            // radioButton_add
            // 
            this.radioButton_add.AutoSize = true;
            this.radioButton_add.Location = new System.Drawing.Point(9, 293);
            this.radioButton_add.Name = "radioButton_add";
            this.radioButton_add.Size = new System.Drawing.Size(119, 21);
            this.radioButton_add.TabIndex = 9;
            this.radioButton_add.Text = " Sn = Sn + Sm";
            this.radioButton_add.UseVisualStyleBackColor = true;
            this.radioButton_add.CheckedChanged += new System.EventHandler(this.radioButton_add_CheckedChanged);
            // 
            // radioButton_orr
            // 
            this.radioButton_orr.AutoSize = true;
            this.radioButton_orr.Location = new System.Drawing.Point(9, 266);
            this.radioButton_orr.Name = "radioButton_orr";
            this.radioButton_orr.Size = new System.Drawing.Size(143, 21);
            this.radioButton_orr.TabIndex = 8;
            this.radioButton_orr.Text = " Sn = Sn  OR byte";
            this.radioButton_orr.UseVisualStyleBackColor = true;
            this.radioButton_orr.CheckedChanged += new System.EventHandler(this.radioButton_orr_CheckedChanged);
            // 
            // radioButton_xor
            // 
            this.radioButton_xor.AutoSize = true;
            this.radioButton_xor.Location = new System.Drawing.Point(9, 239);
            this.radioButton_xor.Name = "radioButton_xor";
            this.radioButton_xor.Size = new System.Drawing.Size(148, 21);
            this.radioButton_xor.TabIndex = 7;
            this.radioButton_xor.Text = " Sn = Sn XOR byte";
            this.radioButton_xor.UseVisualStyleBackColor = true;
            this.radioButton_xor.CheckedChanged += new System.EventHandler(this.radioButton_xor_CheckedChanged);
            // 
            // radioButton_and
            // 
            this.radioButton_and.AutoSize = true;
            this.radioButton_and.Location = new System.Drawing.Point(9, 212);
            this.radioButton_and.Name = "radioButton_and";
            this.radioButton_and.Size = new System.Drawing.Size(147, 21);
            this.radioButton_and.TabIndex = 6;
            this.radioButton_and.Text = " Sn = Sn AND byte";
            this.radioButton_and.UseVisualStyleBackColor = true;
            this.radioButton_and.CheckedChanged += new System.EventHandler(this.radioButton_and_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "...Below here not on AS spec...";
            // 
            // radioButton_wait
            // 
            this.radioButton_wait.AutoSize = true;
            this.radioButton_wait.Location = new System.Drawing.Point(6, 143);
            this.radioButton_wait.Name = "radioButton_wait";
            this.radioButton_wait.Size = new System.Drawing.Size(116, 21);
            this.radioButton_wait.TabIndex = 4;
            this.radioButton_wait.Text = "Wait n (msec)";
            this.radioButton_wait.UseVisualStyleBackColor = true;
            this.radioButton_wait.CheckedChanged += new System.EventHandler(this.radioButton_wait_CheckedChanged);
            // 
            // radioButton_mov
            // 
            this.radioButton_mov.AutoSize = true;
            this.radioButton_mov.Location = new System.Drawing.Point(6, 62);
            this.radioButton_mov.Name = "radioButton_mov";
            this.radioButton_mov.Size = new System.Drawing.Size(86, 21);
            this.radioButton_mov.TabIndex = 3;
            this.radioButton_mov.Text = " Sn = Sm";
            this.radioButton_mov.UseVisualStyleBackColor = true;
            this.radioButton_mov.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton_addi
            // 
            this.radioButton_addi.AutoSize = true;
            this.radioButton_addi.Location = new System.Drawing.Point(6, 89);
            this.radioButton_addi.Name = "radioButton_addi";
            this.radioButton_addi.Size = new System.Drawing.Size(126, 21);
            this.radioButton_addi.TabIndex = 2;
            this.radioButton_addi.Text = " Sn = Sn + byte";
            this.radioButton_addi.UseVisualStyleBackColor = true;
            this.radioButton_addi.CheckedChanged += new System.EventHandler(this.radioButton_addi_CheckedChanged);
            // 
            // radioButton_subi
            // 
            this.radioButton_subi.AutoSize = true;
            this.radioButton_subi.Location = new System.Drawing.Point(6, 116);
            this.radioButton_subi.Name = "radioButton_subi";
            this.radioButton_subi.Size = new System.Drawing.Size(123, 21);
            this.radioButton_subi.TabIndex = 1;
            this.radioButton_subi.Text = " Sn = Sn - byte";
            this.radioButton_subi.UseVisualStyleBackColor = true;
            this.radioButton_subi.CheckedChanged += new System.EventHandler(this.radioButton_subi_CheckedChanged);
            // 
            // radioButton_movi
            // 
            this.radioButton_movi.AutoSize = true;
            this.radioButton_movi.Checked = true;
            this.radioButton_movi.Location = new System.Drawing.Point(6, 35);
            this.radioButton_movi.Name = "radioButton_movi";
            this.radioButton_movi.Size = new System.Drawing.Size(93, 21);
            this.radioButton_movi.TabIndex = 0;
            this.radioButton_movi.TabStop = true;
            this.radioButton_movi.Text = " Sn = byte";
            this.radioButton_movi.UseVisualStyleBackColor = true;
            this.radioButton_movi.CheckedChanged += new System.EventHandler(this.radioButton_movi_CheckedChanged);
            // 
            // comboBox_Sn
            // 
            this.comboBox_Sn.FormattingEnabled = true;
            this.comboBox_Sn.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3",
            "S4",
            "S5",
            "S6",
            "S7"});
            this.comboBox_Sn.Location = new System.Drawing.Point(124, 27);
            this.comboBox_Sn.Name = "comboBox_Sn";
            this.comboBox_Sn.Size = new System.Drawing.Size(59, 24);
            this.comboBox_Sn.TabIndex = 24;
            this.comboBox_Sn.SelectedIndexChanged += new System.EventHandler(this.comboBox_Sn_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 23;
            this.label3.Text = "Register Sn:";
            // 
            // comboBox_Sm
            // 
            this.comboBox_Sm.FormattingEnabled = true;
            this.comboBox_Sm.Items.AddRange(new object[] {
            "S0",
            "S1",
            "S2",
            "S3",
            "S4",
            "S5",
            "S6",
            "S7"});
            this.comboBox_Sm.Location = new System.Drawing.Point(124, 71);
            this.comboBox_Sm.Name = "comboBox_Sm";
            this.comboBox_Sm.Size = new System.Drawing.Size(59, 24);
            this.comboBox_Sm.TabIndex = 26;
            this.comboBox_Sm.SelectedIndexChanged += new System.EventHandler(this.comboBox_Sm_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Register Sm:";
            // 
            // textBox_byte
            // 
            this.textBox_byte.Location = new System.Drawing.Point(124, 112);
            this.textBox_byte.Name = "textBox_byte";
            this.textBox_byte.Size = new System.Drawing.Size(59, 22);
            this.textBox_byte.TabIndex = 34;
            this.textBox_byte.Text = "00";
            this.textBox_byte.TextChanged += new System.EventHandler(this.textBox_byte_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 33;
            this.label4.Text = "Byte:";
            // 
            // button_done
            // 
            this.button_done.Location = new System.Drawing.Point(300, 379);
            this.button_done.Name = "button_done";
            this.button_done.Size = new System.Drawing.Size(75, 27);
            this.button_done.TabIndex = 36;
            this.button_done.Text = "OK";
            this.button_done.UseVisualStyleBackColor = true;
            this.button_done.Click += new System.EventHandler(this.button_done_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Location = new System.Drawing.Point(35, 21);
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.Size = new System.Drawing.Size(133, 22);
            this.textBox_result.TabIndex = 35;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(402, 379);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(79, 27);
            this.button_Cancel.TabIndex = 39;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_comment
            // 
            this.textBox_comment.Location = new System.Drawing.Point(6, 21);
            this.textBox_comment.Multiline = true;
            this.textBox_comment.Name = "textBox_comment";
            this.textBox_comment.Size = new System.Drawing.Size(205, 79);
            this.textBox_comment.TabIndex = 38;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_Sm);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBox_Sn);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_byte);
            this.groupBox2.Location = new System.Drawing.Point(278, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 147);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registers and Values";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_comment);
            this.groupBox3.Location = new System.Drawing.Point(278, 186);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 117);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Comment";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_result);
            this.groupBox4.Location = new System.Drawing.Point(278, 310);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 57);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Result:";
            // 
            // ProcessEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 432);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_done);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProcessEdit";
            this.Text = "ProcessEdit";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_movi;
        private System.Windows.Forms.RadioButton radioButton_sub;
        private System.Windows.Forms.RadioButton radioButton_add;
        private System.Windows.Forms.RadioButton radioButton_orr;
        private System.Windows.Forms.RadioButton radioButton_xor;
        private System.Windows.Forms.RadioButton radioButton_and;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_wait;
        private System.Windows.Forms.RadioButton radioButton_mov;
        private System.Windows.Forms.RadioButton radioButton_addi;
        private System.Windows.Forms.RadioButton radioButton_subi;
        private System.Windows.Forms.ComboBox comboBox_Sn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_Sm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_byte;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_done;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_comment;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}