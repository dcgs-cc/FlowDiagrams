namespace FlowDiagrams
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDiagramToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.seToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decisionBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readADCBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.returnBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToOCRAsmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programPICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleInsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputPinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale80 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale60 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale40 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Scale25 = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.seToolStripMenuItem,
            this.newToolStripMenuItem,
            this.simpleInsertToolStripMenuItem,
            this.viewToolStripMenuItem1,
            this.outputToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1036, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.printDiagramToolStripMenuItem1,
            this.loadXMLToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.saveToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // printDiagramToolStripMenuItem1
            // 
            this.printDiagramToolStripMenuItem1.Enabled = false;
            this.printDiagramToolStripMenuItem1.Name = "printDiagramToolStripMenuItem1";
            this.printDiagramToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
            this.printDiagramToolStripMenuItem1.Text = "Print Diagram";
            this.printDiagramToolStripMenuItem1.Click += new System.EventHandler(this.printDiagramToolStripMenuItem_Click);
            // 
            // seToolStripMenuItem
            // 
            this.seToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearSelectToolStripMenuItem,
            this.deleteLinksToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.seToolStripMenuItem.Name = "seToolStripMenuItem";
            this.seToolStripMenuItem.Size = new System.Drawing.Size(74, 22);
            this.seToolStripMenuItem.Text = "Selected";
            // 
            // clearSelectToolStripMenuItem
            // 
            this.clearSelectToolStripMenuItem.Name = "clearSelectToolStripMenuItem";
            this.clearSelectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.clearSelectToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.clearSelectToolStripMenuItem.Text = "Clear Select";
            this.clearSelectToolStripMenuItem.Click += new System.EventHandler(this.clearSelectToolStripMenuItem_Click);
            // 
            // deleteLinksToolStripMenuItem
            // 
            this.deleteLinksToolStripMenuItem.Name = "deleteLinksToolStripMenuItem";
            this.deleteLinksToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.deleteLinksToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.deleteLinksToolStripMenuItem.Text = "Delete Selected Links";
            this.deleteLinksToolStripMenuItem.Click += new System.EventHandler(this.deleteLinksToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.deleteToolStripMenuItem.Text = "Delete Selected Boxes";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputBoxToolStripMenuItem,
            this.processBoxToolStripMenuItem,
            this.outputBoxToolStripMenuItem,
            this.decisionBoxToolStripMenuItem,
            this.endBoxToolStripMenuItem,
            this.readADCBoxToolStripMenuItem,
            this.subProcessToolStripMenuItem,
            this.breakBoxToolStripMenuItem,
            this.returnBoxToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(59, 22);
            this.newToolStripMenuItem.Text = "Insert";
            // 
            // inputBoxToolStripMenuItem
            // 
            this.inputBoxToolStripMenuItem.Name = "inputBoxToolStripMenuItem";
            this.inputBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.inputBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.inputBoxToolStripMenuItem.Text = "Input Box";
            this.inputBoxToolStripMenuItem.Click += new System.EventHandler(this.inputBoxToolStripMenuItem_Click);
            // 
            // processBoxToolStripMenuItem
            // 
            this.processBoxToolStripMenuItem.Name = "processBoxToolStripMenuItem";
            this.processBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.processBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.processBoxToolStripMenuItem.Text = "Process Box";
            this.processBoxToolStripMenuItem.Click += new System.EventHandler(this.processBoxToolStripMenuItem_Click);
            // 
            // outputBoxToolStripMenuItem
            // 
            this.outputBoxToolStripMenuItem.Name = "outputBoxToolStripMenuItem";
            this.outputBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.outputBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.outputBoxToolStripMenuItem.Text = "Output Box";
            this.outputBoxToolStripMenuItem.Click += new System.EventHandler(this.outputBoxToolStripMenuItem_Click);
            // 
            // decisionBoxToolStripMenuItem
            // 
            this.decisionBoxToolStripMenuItem.Name = "decisionBoxToolStripMenuItem";
            this.decisionBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.decisionBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.decisionBoxToolStripMenuItem.Text = "Decision Box";
            this.decisionBoxToolStripMenuItem.Click += new System.EventHandler(this.decisionBoxToolStripMenuItem_Click);
            // 
            // endBoxToolStripMenuItem
            // 
            this.endBoxToolStripMenuItem.Name = "endBoxToolStripMenuItem";
            this.endBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.endBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.endBoxToolStripMenuItem.Text = "End Box";
            this.endBoxToolStripMenuItem.Click += new System.EventHandler(this.endBoxToolStripMenuItem_Click);
            // 
            // readADCBoxToolStripMenuItem
            // 
            this.readADCBoxToolStripMenuItem.Name = "readADCBoxToolStripMenuItem";
            this.readADCBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.readADCBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.readADCBoxToolStripMenuItem.Text = "Read ADC Box";
            this.readADCBoxToolStripMenuItem.Click += new System.EventHandler(this.readADCBoxToolStripMenuItem_Click);
            // 
            // subProcessToolStripMenuItem
            // 
            this.subProcessToolStripMenuItem.Name = "subProcessToolStripMenuItem";
            this.subProcessToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.subProcessToolStripMenuItem.Text = "Sub_Process";
            this.subProcessToolStripMenuItem.Click += new System.EventHandler(this.subProcessToolStripMenuItem_Click);
            // 
            // breakBoxToolStripMenuItem
            // 
            this.breakBoxToolStripMenuItem.Name = "breakBoxToolStripMenuItem";
            this.breakBoxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.breakBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.breakBoxToolStripMenuItem.Text = "BreakBox";
            this.breakBoxToolStripMenuItem.Click += new System.EventHandler(this.breakBoxToolStripMenuItem_Click);
            // 
            // returnBoxToolStripMenuItem
            // 
            this.returnBoxToolStripMenuItem.Name = "returnBoxToolStripMenuItem";
            this.returnBoxToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.returnBoxToolStripMenuItem.Text = "Return Box";
            this.returnBoxToolStripMenuItem.Click += new System.EventHandler(this.returnBoxToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.convertToOCRAsmToolStripMenuItem,
            this.programPICToolStripMenuItem});
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(65, 22);
            this.outputToolStripMenuItem.Text = "Output";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.viewToolStripMenuItem.Text = "View as Psuedo Code";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // convertToOCRAsmToolStripMenuItem
            // 
            this.convertToOCRAsmToolStripMenuItem.Name = "convertToOCRAsmToolStripMenuItem";
            this.convertToOCRAsmToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.convertToOCRAsmToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.convertToOCRAsmToolStripMenuItem.Text = "View as OCR Asembler";
            this.convertToOCRAsmToolStripMenuItem.Click += new System.EventHandler(this.convertToOCRAsmToolStripMenuItem_Click);
            // 
            // programPICToolStripMenuItem
            // 
            this.programPICToolStripMenuItem.Name = "programPICToolStripMenuItem";
            this.programPICToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.programPICToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.programPICToolStripMenuItem.Text = "Program PIC";
            this.programPICToolStripMenuItem.Click += new System.EventHandler(this.programPICToolStripMenuItem_Click);
            // 
            // simpleInsertToolStripMenuItem
            // 
            this.simpleInsertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputPinToolStripMenuItem,
            this.waitToolStripMenuItem,
            this.playNoteToolStripMenuItem,
            this.decisionToolStripMenuItem});
            this.simpleInsertToolStripMenuItem.Name = "simpleInsertToolStripMenuItem";
            this.simpleInsertToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.simpleInsertToolStripMenuItem.Text = "Simple Insert";
            // 
            // outputPinToolStripMenuItem
            // 
            this.outputPinToolStripMenuItem.Name = "outputPinToolStripMenuItem";
            this.outputPinToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.outputPinToolStripMenuItem.Text = "Output pin";
            this.outputPinToolStripMenuItem.Click += new System.EventHandler(this.outputPinToolStripMenuItem_Click);
            // 
            // waitToolStripMenuItem
            // 
            this.waitToolStripMenuItem.Name = "waitToolStripMenuItem";
            this.waitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.waitToolStripMenuItem.Text = "Wait";
            this.waitToolStripMenuItem.Click += new System.EventHandler(this.waitToolStripMenuItem_Click);
            // 
            // playNoteToolStripMenuItem
            // 
            this.playNoteToolStripMenuItem.Name = "playNoteToolStripMenuItem";
            this.playNoteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.playNoteToolStripMenuItem.Text = "Play Note";
            this.playNoteToolStripMenuItem.Click += new System.EventHandler(this.playNoteToolStripMenuItem_Click);
            // 
            // decisionToolStripMenuItem
            // 
            this.decisionToolStripMenuItem.Name = "decisionToolStripMenuItem";
            this.decisionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.decisionToolStripMenuItem.Text = "Decision";
            this.decisionToolStripMenuItem.Click += new System.EventHandler(this.decisionToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenu_Scale100,
            this.toolStripMenu_Scale80,
            this.toolStripMenu_Scale60,
            this.toolStripMenu_Scale50,
            this.toolStripMenu_Scale40,
            this.toolStripMenu_Scale25,
            this.scaleToolStripMenuItem,
            this.snapToGridToolStripMenuItem});
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(49, 22);
            this.viewToolStripMenuItem1.Text = "View";
            // 
            // toolStripMenu_Scale100
            // 
            this.toolStripMenu_Scale100.Name = "toolStripMenu_Scale100";
            this.toolStripMenu_Scale100.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.toolStripMenu_Scale100.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale100.Text = "100%";
            this.toolStripMenu_Scale100.Click += new System.EventHandler(this.toolStripMenu_Scale100_Click);
            // 
            // toolStripMenu_Scale80
            // 
            this.toolStripMenu_Scale80.Name = "toolStripMenu_Scale80";
            this.toolStripMenu_Scale80.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
            this.toolStripMenu_Scale80.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale80.Text = "80%";
            this.toolStripMenu_Scale80.Click += new System.EventHandler(this.toolStripMenu_Scale80_Click);
            // 
            // toolStripMenu_Scale60
            // 
            this.toolStripMenu_Scale60.Name = "toolStripMenu_Scale60";
            this.toolStripMenu_Scale60.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
            this.toolStripMenu_Scale60.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale60.Text = "60%";
            this.toolStripMenu_Scale60.Click += new System.EventHandler(this.toolStripMenu_Scale60_Click);
            // 
            // toolStripMenu_Scale50
            // 
            this.toolStripMenu_Scale50.Name = "toolStripMenu_Scale50";
            this.toolStripMenu_Scale50.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
            this.toolStripMenu_Scale50.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale50.Text = "50%";
            this.toolStripMenu_Scale50.Click += new System.EventHandler(this.toolStripMenu_Scale50_Click);
            // 
            // toolStripMenu_Scale40
            // 
            this.toolStripMenu_Scale40.Name = "toolStripMenu_Scale40";
            this.toolStripMenu_Scale40.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.toolStripMenu_Scale40.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale40.Text = "40%";
            this.toolStripMenu_Scale40.Click += new System.EventHandler(this.toolStripMenu_Scale40_Click);
            // 
            // toolStripMenu_Scale25
            // 
            this.toolStripMenu_Scale25.Name = "toolStripMenu_Scale25";
            this.toolStripMenu_Scale25.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.toolStripMenu_Scale25.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenu_Scale25.Text = "25%";
            this.toolStripMenu_Scale25.Click += new System.EventHandler(this.toolStripMenu_Scale25_Click);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.scaleToolStripMenuItem.Text = "Scale";
            this.scaleToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // snapToGridToolStripMenuItem
            // 
            this.snapToGridToolStripMenuItem.Name = "snapToGridToolStripMenuItem";
            this.snapToGridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.snapToGridToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.snapToGridToolStripMenuItem.Text = "Snap to grid";
            this.snapToGridToolStripMenuItem.Click += new System.EventHandler(this.snapToGridToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(58, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // loadXMLToolStripMenuItem
            // 
            this.loadXMLToolStripMenuItem.Name = "loadXMLToolStripMenuItem";
            this.loadXMLToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.loadXMLToolStripMenuItem.Text = "Load XML";
            this.loadXMLToolStripMenuItem.Click += new System.EventHandler(this.loadXMLToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1036, 526);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flow Diagrams";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem seToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decisionBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readADCBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToOCRAsmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programPICToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printDiagramToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem breakBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleInsertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputPinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decisionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale100;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale80;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale50;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale25;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale40;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Scale60;
        private System.Windows.Forms.ToolStripMenuItem snapToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem returnBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadXMLToolStripMenuItem;
    }
}

