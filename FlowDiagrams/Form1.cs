using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Runtime.Serialization.Formatters.Binary;
using SerialProgrammer;

namespace FlowDiagrams
{
    public partial class Form1 : Form
    {

        static bool simpleModel = true;
        #region Variables

        static int MAX_LINES_ASM = 500;
        static int MAX_LINES_ASM_SUBS = 100;

        public FlowDiagram d = new FlowDiagram();
        public string[] OCRCode = new string[MAX_LINES_ASM];
        public string[] OCRcode_subs = new string[MAX_LINES_ASM_SUBS];

        public List<string> OCRcode_subs_names= new List<string>();
        public int max_n = 0;
        public int max_n_subs = 0;
        int text_count = 0;
        string filename = "";
        float scale = 100f;
        #endregion

        #region Initialization and Paint
        public Form1()
        {
            InitializeComponent();
            Label label_End = new Label();
            Controls.Add(label_End);
            label_End.AutoSize = true;
            label_End.Location = new System.Drawing.Point(834, 424);
            label_End.Name = "label_End";
            label_End.Size = new System.Drawing.Size(40, 17);
            label_End.TabIndex = 1;
            label_End.Location = new Point(2000, 3000);//to set max size....  
            label_End.Text = "END!";
            Controls.Add(d.start);

            this.KeyPreview = true;
            //find the insert menu and disable it
            ToolStripItem[] fred = this.menuStrip1.Items.Find("newToolStripMenuItem", true);
            foreach (ToolStripItem i in fred)
            {
                //i.Visible = !simpleModel;
                i.Enabled = !simpleModel;
            }
            //find the simple insert menu and enable it...
            fred = this.menuStrip1.Items.Find("simpleInsertToolStripMenuItem", true);
            foreach (ToolStripItem i in fred)
            {
                //i.Visible = simpleModel;
                i.Enabled = simpleModel;
            }
            
            label_End.Location = new Point(2000, 3000);//to set max size....  

            AllowDrop = true;
            DragDrop += new DragEventHandler(Form1_DragDrop);
            DragEnter += new DragEventHandler(Form1_DragEnter);
            MouseClick += new MouseEventHandler(Form1_MouseClick);

            //Dialogs.DataTable dd = new Dialogs.DataTable();
            //dd.ShowDialog();
        }
        public Form1(FlowDiagram d1)
        {
            InitializeComponent();
            AllowDrop = true;
            DragDrop += new DragEventHandler(Form1_DragDrop);
            DragEnter += new DragEventHandler(Form1_DragEnter);
            MouseClick += new MouseEventHandler(Form1_MouseClick);
            d = d1;
            foreach (FlowObject f in d.BoxesList)
            {
                Controls.Add(f);
                f.Box_Clicked += new FlowObject.Click_Select(Box_Clicked);
                f.Box_RightClicked += new FlowObject.Click_Right(Box_RightClicked);
            }
            this.Invalidate();
        }




        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);   
            d.Draw(e.Graphics, this.AutoScrollPosition);
        }

        private void Form1_Scroll(object sender, ScrollEventArgs e)
        {

            int x = this.AutoScrollPosition.X;
            int y= this.AutoScrollPosition.Y;

        }


        #endregion
        
        #region Events and delegates
        void Box_RightClicked(FlowObject control, MouseEventArgs e)
        {
            //here on rt click... select...
            if (control.GetType() == typeof(StartBox))
            {
                MessageBox.Show("You can't edit or delete the start box!"); return;
            }
            control.BackColor = this.BackColor;
            control.Selected = !control.Selected;
            control.Invalidate();
        }
        void Box_Clicked(FlowObject control)
        {
            if (control.GetType() == typeof(StartBox))
            {
                MessageBox.Show("You can't edit or delete the start box!"); return;
            }



            if (control.GetType() == typeof(InputBox))
            {
                Dialogs.InputEdit d1 = new FlowDiagrams.Dialogs.InputEdit();
                d1.result = control.text;
                d1.comment = control.Comment;
                d1.Setup();
                d1.ShowDialog();
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[0] = d1.asm_code[k];
                control.text = d1.result;
                control.Comment = d1.comment;
                control.Invalidate(); return;
            }
            if (control.GetType() == typeof(OutputBox))
            {
                Dialogs.OutputEdit d1 = new FlowDiagrams.Dialogs.OutputEdit();
                d1.result = control.text;
                d1.comment = control.Comment;
                d1.Setup();
                d1.ShowDialog();
                control.text = d1.result;
                control.Comment = d1.comment;
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[0] = d1.asm_code[k];
                control.Invalidate(); return;
            }
            if (control.GetType() == typeof(ProcessBox))
            {
                ProcessBox ppr1 = (ProcessBox)control;
                Dialogs.ProcessEdit d1 = new FlowDiagrams.Dialogs.ProcessEdit();
                d1.result = control.text;
                d1.comment = control.Comment;
                d1.op_type = ppr1.op_type;
                d1.b1 = ppr1.b;
                d1.sn = ppr1.Sn;
                d1.sm=ppr1.Sm;
                d1.Setup();
                d1.ShowDialog();
                control.text = d1.result;
                control.Comment = d1.comment;
                ppr1.op_type = d1.op_type;
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[k] = d1.asm_code[k];
                ppr1.b = d1.b1;
                ppr1.Sn = d1.sn;
                ppr1.Sm = d1.sm;

                control.Invalidate(); return;
            }
            if (control.GetType() == typeof(DecisionBox))
            {
                Dialogs.DecisionEdit d1 = new FlowDiagrams.Dialogs.DecisionEdit();
                DecisionBox c1 = (DecisionBox)control;
                d1.result = control.text;
                d1.YesSide = c1.YesSide;
                d1.comment = c1.Comment;
                d1.Setup();
                d1.ShowDialog();
                c1.text = d1.result;
                c1.YesSide = d1.YesSide;
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[k] = d1.asm_code[k];
                c1.Comment = d1.comment;
                c1.Invalidate(); return;
            }


            if (control.GetType() == typeof(SubProcessBox))
            {
                Dialogs.SubProcessEdit d1 = new FlowDiagrams.Dialogs.SubProcessEdit();
                SubProcessBox c1 = (SubProcessBox)control;
                d1.result = control.text;//this is the name of the process and the name appended to lines
                d1.comment = c1.Comment;
                d1.Filename = c1.FileName;
                if (c1.diagram1 != null)
                {
                    d1.d = c1.diagram1;
                }
                for (int k = 0; k < control.n_asm; k++)
                {
                    d1.asm_code[k]= control.Asmcode[k].Replace(control.text + "line", "line");
                }
                d1.n = control.n_asm;
                d1.Setup();
                d1.ShowDialog();
                c1.text = d1.result;
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[k] = d1.asm_code[k];
                c1.Comment = d1.comment;
                c1.text = d1.result;
                c1.FileName = d1.Filename;
                c1.diagram1 = d1.d;
                c1.Invalidate(); return;
            }


            if (control.GetType() == typeof(BreakBox))
            {
                Dialogs.BreakEdit d1 = new Dialogs.BreakEdit();
                d1.box = (BreakBox)control;
                d1.Setup();
                d1.ShowDialog();
                //control.n_asm = d1.n;
                //for (int k = 0; k < d1.n; k++) control.Asmcode[0] = d1.asm_code[k];
                //control.text = d1.result;
                //control.Comment = d1.comment;
                control.Invalidate(); return;
            }

            if (control.GetType() == typeof(OutputPin))
            {
                Dialogs.OutputPinEdit d1 = new Dialogs.OutputPinEdit();
                d1.result = control.text.Substring(0,1);//ist char is A,B et
                OutputPin o1 = (OutputPin)control;
                d1.state = o1.state;
                d1.comment = control.Comment;
                d1.Setup();
                d1.ShowDialog();
                o1.state = d1.state;
                control.text = d1.result + " =OFF";
                if (d1.state) control.text = d1.result + " =ON";
                control.Comment = d1.comment;
                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[0] = d1.asm_code[k];
                control.Invalidate(); return;
            }

            if (control.GetType() == typeof(WaitSec))
            {
                Dialogs.WaitSecEdit d1 = new Dialogs.WaitSecEdit();
                WaitSec o1 = (WaitSec)control;
                d1.wait = o1.wait;
                d1.Setup();
                d1.ShowDialog();
                o1.wait= d1.wait;//max value is going to be 20...

                control.text = "Wait " + d1.wait.ToString() + "s";
                //if more than 1 sec we need to call wait1Sec
                //fractions call wait1ms
                int wait1 = (int)o1.wait;
                decimal d2 = 100*(o1.wait - wait1);
                int wait0 = (int)d2;
                if (wait1 > 0)
                {
                    control.Asmcode[0] = "linexx:     MOVI   S9," + (wait1).ToString("X");
                    control.Asmcode[1] = "D1linexx:   RCALL wait1Sec";
                    control.Asmcode[2] = "            DEC   S9";
                    control.Asmcode[3] = "            JNZ D1linexx";
                }
                else
                {
                    control.Asmcode[0] = "linexx:     NOP";
                    control.Asmcode[1] = "            NOP";
                    control.Asmcode[2] = "            NOP";
                    control.Asmcode[3] = "            NOP";
                }
                if (wait0 > 0)
                {
                    control.Asmcode[4] = "            MOVI   S9," + (wait0).ToString("X");
                    control.Asmcode[5] = "D3linexx:   MOVI   S8,0A";
                    control.Asmcode[6] = "D2linexx:   RCALL  wait1ms";
                    control.Asmcode[7] = "            DEC    S8";
                    control.Asmcode[8] = "            JNZ    D2linexx";
                    control.Asmcode[9] = "            DEC    S9";
                    control.Asmcode[10] = "           JNZ    D3linexx";
                    control.n_asm = 11;
                }
                else
                {
                    control.n_asm = 4;
                }

                control.Invalidate(); return;
                /*
                 *                     s = "Wait " + b1; CheckHexByte(b1);
                     asm_code[0] = "linexx:      MOVI S8," + b1;
                     asm_code[1] = "Dellinexx:         RCALL wait1ms";
                     asm_code[2] = "            DEC S8";
                     asm_code[3] = "            JNZ Dellinexx"; n = 4;
                 */



            }
            if (control.GetType() == typeof(SimpleDecisionBox))
            {
                Dialogs.SimpleDecisionEdit d1 = new FlowDiagrams.Dialogs.SimpleDecisionEdit();
                SimpleDecisionBox c1 = (SimpleDecisionBox)control;
                for (int k = 0; k < c1.n_asm; k++) d1.asm_code[k] = c1.Asmcode[k];
                d1.pin = c1.Pinnumber.ToString();
                d1.Setup();
                d1.ShowDialog();
                c1.text = "I" + d1.pin + " = 0??";

                control.n_asm = d1.n;
                for (int k = 0; k < d1.n; k++) control.Asmcode[k] = d1.asm_code[k];

                c1.Invalidate(); return;
            }


            if (control.GetType() == typeof(PlayNote))
            {
                PlayNote c1 = (PlayNote)control;
                Dialogs.PlayNoteEdit d1 = new FlowDiagrams.Dialogs.PlayNoteEdit();
                d1.frequency = c1.frequency;
                d1.Length = c1.Length;
                d1.Setup();
                d1.ShowDialog();
                c1.frequency = d1.frequency;
                c1.Length = d1.Length;
                decimal f1 = c1.frequency;
                int TimeValue = (int)(1000000 / (f1 * 16));
                if (TimeValue < 15) { TimeValue = 15; c1.frequency = (1000000 / (16 * TimeValue)); }
                if (TimeValue > 255) { TimeValue = 255; c1.frequency = (1000000 / (16 * TimeValue)); }
                TimeValue = 0x100 - TimeValue;
                c1.Asmcode[0] = "linexx:   movi   S0," + TimeValue.ToString("X");
                c1.Asmcode[1] = "          movi   S1,"+  c1.Length.ToString("X");
                c1.text = "Play " + d1.frequency.ToString();
                c1.Invalidate(); return;
            } 

        }
        void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string item = (string)e.Data.GetData(typeof(System.String));
                FlowObject c = (FlowObject)Controls[Controls.IndexOfKey(item)];
                if (e.AllowedEffect.ToString().Contains("Link"))
                {
                    return;
                }

                if (e.Effect == DragDropEffects.Move)
                {
                    Point p0 = new Point(e.X, e.Y);
                    Point p = PointToClient(new Point(e.X, e.Y));
                    Point p1 = c.start_drag;

                    int x = (p.X / 10); x = 10 * x; p.X = x;//grid to 10??
                    c.x_position += (int)((100/scale) * (p.X - c.Location.X));
                    c.y_position += (int)((100/scale)*(p.Y - c.Location.Y));
                    c.Location = p;

                }
                if (e.Effect == DragDropEffects.Link)
                {
                    Point p = PointToClient(new Point(e.X, e.Y));
                }

            }
            catch
            {
                e.Effect = DragDropEffects.None;
            }
            Invalidate();
        }
        #endregion

        #region ToolStripHandlers

        private void programPICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InterpretDiagram();// forms the OCRcode 
            if (!simpleModel)
            {
                InCircuitProgramer f = new InCircuitProgramer();
                //copy the current OCR code to the programmer...
                for (int i = 0; i < max_n; i++) f.program1.OCRCode[i] = OCRCode[i];
                f.program1.max_OCRlines = max_n;
                f.Setup();
                f.ShowDialog();
            }
            else
            {
                string s = "";
                SerialProgrammer.SerialProgrammer program1 = new SerialProgrammer.SerialProgrammer();
                for (int i = 0; i < max_n; i++) program1.OCRCode[i] = OCRCode[i];
                program1.max_OCRlines = max_n;
                program1.Code_start = 0x3E;
                program1.SimpleModel = true;// flag we are using simple model and 16F84A
                if (!program1.GenerateMC(ref s,false))
                {
                    MessageBox.Show(s, "Error in code"); return;
                }
                s = program1.IntelCode();//get the code
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                try
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string filename = saveFileDialog1.FileName;

                        Stream TestFileStream = saveFileDialog1.OpenFile();
                        StreamWriter sr1 = new StreamWriter(TestFileStream);
                        sr1.Write(s);
                        sr1.Close();
                        TestFileStream.Close();
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Save Error" + e1.Message);
                }
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<FlowObject> BoxesList1 = new List<FlowObject>();
            foreach (FlowObject f in d.BoxesList)
            {
                if (f.Selected)
                {
                    //need to go through the list and remove any references...
                    foreach (FlowObject f1 in d.BoxesList)
                    {
                        if (f1.FlowOut == f) f1.FlowOut = null;
                        if (f1.FlowoutSide == f) f1.FlowoutSide = null;
                    }
                    Controls.Remove(f);
                }
                else
                {
                    BoxesList1.Add(f);
                }
            }
            d.BoxesList.Clear();
            foreach (FlowObject f in BoxesList1)
            {
                d.BoxesList.Add(f);
            }
            if (d.BoxesList.Count == 1) ReflectModel(false);else ReflectModel(true);
        }
        private void inputBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new InputBox(40, 120));
        }
        private void processBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new ProcessBox(40, 120));
        }
        private void outputBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new OutputBox(40, 120));
        }
        private void decisionBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new DecisionBox(40, 120));
        }
        private void deleteLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FlowObject f in d.BoxesList)
            {
                if (f.Selected)
                {
                    f.FlowOut = null; f.FlowoutSide = null;
                }
            }
            Invalidate();
        }
        private void clearSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (FlowObject f in d.BoxesList)
            {
                f.Selected = false; f.BackColor = this.BackColor;
            }
            Invalidate();
        }
        private void endBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new EndBox(40, 120));
        }
        private void readADCBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new ADCBox(40, 120));
        }
        private void subProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Warning...this is still in beta! ie still needs testing...");
            AddFlowObject(new SubProcessBox(40, 120));
        }
        private void breakBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new BreakBox(40, 120));
        }
        private void returnBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new ReturnBox(100, 120));
        }

        private void decisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new SimpleDecisionBox(40, 120));
        }

        //simple ones...
        #region SimpleBoxes
        private void outputPinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new OutputPin(40, 120));
        }

        private void waitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new WaitSec(40,120));
        }

        private void playNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFlowObject(new PlayNote(40, 120));
        }
        #endregion
        #endregion

        #region Methods
        private void AddFlowObject(FlowObject o)
        {
            o.scale = scale;
            o.Scale(new SizeF(scale / 100, scale / 100));
            d.BoxesList.Add(o);
            Controls.Add(o);
            o.Box_Clicked += new FlowObject.Click_Select(Box_Clicked);
            o.Box_RightClicked += new FlowObject.Click_Right(Box_RightClicked);
            //o.Location = Cursor.Position;
            ReflectModel(true);
            this.Invalidate();
        }

        public string InterpretDiagram()
        {
            string s = "";//for output
            int i = 0;
            foreach (FlowObject o in d.BoxesList)
            {
                o.Line_Number = i; i++; o.Parsed = false;
            }
            max_n = 0; max_n_subs = 0;
            for (int j = 0; j < MAX_LINES_ASM; j++) OCRCode[j] = "";
            for (int j=0;j<MAX_LINES_ASM_SUBS;j++)OCRcode_subs[j]="";
            OCRcode_subs_names.Clear();
            s += ParseSection(d.start);
            //now need to copy the sub code to top of the code space....
            //first a comment o two....
            //OCRCode[max_n] = ";=============SUBROUTINES==========="; max_n++;
            for (int k = 0; k < max_n_subs; k++)
            {
                try
                {
                    OCRCode[max_n] = OCRcode_subs[k];max_n++;
                }
                catch
                {
                    MessageBox.Show("Code too long!!!  Write neater code and come back later", "Error"); return "";
                }
            }
            return s;
        }

        private string ParseSection(FlowObject o1)
        {
            string s = ""; int j; FlowObject F1;
            char c = (char)0x09; string sc = ""; sc += c; bool done = false;
            while (o1 != null)
            {
                if (!o1.Parsed)
                {
                    done = false;
                    if (o1.GetType() == typeof(DecisionBox)) { s += ParseDecisionBox(o1); done = true; }
                    if (o1.GetType() == typeof(SimpleDecisionBox)) { s += ParseSimpleDecisionBox(o1); done = true; }
                    if (o1.GetType() == typeof(SubProcessBox)) { s += ParseSub(o1); done = true; }
                    o1.Parsed = true;
                    if (!done)
                    {
                        j = o1.Line_Number;
                        s += "line" + j.ToString() + sc + o1.text;
                        for (int k = 0; k < o1.n_asm; k++)
                        {
                            try
                            {
                                OCRCode[max_n] = o1.Asmcode[k];
                                OCRCode[max_n] = OCRCode[max_n].Replace("linexx", "line" + j.ToString());
                                max_n++;
                            }
                            catch
                            {
                                MessageBox.Show("Code too long!!!  Write neater code and come back later", "Error"); return "";
                            }
                        }

                        if (o1.FlowOut != null)
                        {
                            F1 = o1.FlowOut; j = F1.Line_Number;
                            if (F1.Parsed)
                            {// so won't output this so need jp...
                                s += sc + "GOTO line" + j.ToString();
                                OCRCode[max_n] = "      jp   line" + j.ToString(); max_n++;
                            }
                        }
                        s += Environment.NewLine;
                    }
                    text_count++;
                }
                else
                {
                    break;
                }
                o1 = o1.FlowOut;
            }
            return s;
        }
        private string ParseDecisionBox(FlowObject o1)
        {
            string s = ""; int j, j1, j2; FlowObject F1; FlowObject F2; string s2 = ""; string s1 = "";
            char c = (char)0x09; string sc = ""; sc += c;
            bool HasSide = false; j1 = 0; j2 = 0; j = o1.Line_Number;
            DecisionBox d2 = (DecisionBox)o1;
            j = o1.Line_Number; F1 = d2.FlowOut; F2 = d2.FlowoutSide; HasSide = d2.YesSide;
            if ((F1 == null) || (F2 == null))
            {
                MessageBox.Show("The Decision Box marked has a link missing.", "Parse Error", MessageBoxButtons.OK);
                foreach (FlowObject f in d.BoxesList) { f.Selected = false; } d2.Selected = true; d2.Invalidate(); return s;
            }
            j1 = F1.Line_Number;
            j2 = F2.Line_Number;
            if (HasSide) { s1 = "line" + j2.ToString(); s2 = "line" + j1.ToString(); }
            else { s1 = "line" + j1.ToString(); s2 = "line" + j2.ToString(); }

            s += "line" + j.ToString() + sc + "IF (" + o1.text + ") THEN GOTO " + s1 + " ELSE GOTO " + s2;
            s += Environment.NewLine;
            for (int k = 0; k < o1.n_asm; k++)
            {
                OCRCode[max_n] = o1.Asmcode[k];
                OCRCode[max_n] = OCRCode[max_n].Replace("linexx", "line" + j.ToString());
                OCRCode[max_n] = OCRCode[max_n].Replace("lineyy", s1);
                OCRCode[max_n] = OCRCode[max_n].Replace("linezz", s2);
                max_n++;
            }
            s += ParseSection(o1.FlowoutSide);
            return s;
        }
        private string ParseSimpleDecisionBox(FlowObject o1)
        {
            string s = ""; int j,j1, j2; FlowObject F1; FlowObject F2; string s2 = ""; string s1 = "";
            char c = (char)0x09; string sc = ""; sc += c;
            bool HasSide = false; j1 = 0; j2 = 0; j = o1.Line_Number;
            SimpleDecisionBox d3 = (SimpleDecisionBox)o1;
            F1 = d3.FlowOut; F2 = d3.FlowoutSide; HasSide = d3.YesSide;
            if ((F1 == null) || (F2 == null))
            {
                MessageBox.Show("The Decision Box marked has a link missing.", "Parse Error", MessageBoxButtons.OK);
                foreach (FlowObject f in d.BoxesList) { f.Selected = false; } d3.Selected = true; d3.Invalidate(); return s;
            }
            j1 = F1.Line_Number;
            j2 = F2.Line_Number;

            if (HasSide) { s1 = "line" + j2.ToString(); s2 = "line" + j1.ToString(); }
            else { s1 = "line" + j1.ToString(); s2 = "line" + j2.ToString(); }

            s += "line" + j.ToString() + sc + "IF (" + o1.text + ") THEN GOTO " + s1 + " ELSE GOTO " + s2;
            s += Environment.NewLine;
            for (int k = 0; k < o1.n_asm; k++)
            {
                OCRCode[max_n] = o1.Asmcode[k];
                OCRCode[max_n] = OCRCode[max_n].Replace("linexx", "line" + j.ToString());
                OCRCode[max_n] = OCRCode[max_n].Replace("lineyy", s1);
                OCRCode[max_n] = OCRCode[max_n].Replace("linezz", s2);
                max_n++;
            }
            s += ParseSection(o1.FlowoutSide);
            return s;
        }
        private string ParseSub(FlowObject o1)
        {
            string s = ""; int j; FlowObject F1;char c = (char)0x09; string sc = ""; sc += c;
            SubProcessBox b1 = (SubProcessBox)o1;

            j = o1.Line_Number;
            s += "line" + j.ToString() + sc + "CALL " + o1.text + Environment.NewLine; ;

            try
            {
                OCRCode[max_n] = "linexx" + sc + "RCALL" + sc + b1.text;//ie call it!
                OCRCode[max_n] = OCRCode[max_n].Replace("linexx", "line" + j.ToString());
                max_n++;
            }
            catch
            {
                MessageBox.Show("Code too long!!!  Write neater code and come back later", "Error"); return "";
            }


            if (o1.FlowOut != null)
            {
                F1 = o1.FlowOut; j = F1.Line_Number;
                if (F1.Parsed)
                {// so won't output this so need jp...
                    s += sc + "GOTO line" + j.ToString();
                    OCRCode[max_n] = "      jp   line" + j.ToString(); max_n++;
                }
            }
            s += Environment.NewLine;

            bool found = false;
            //do we already have this in code space...???
            foreach (string s1 in OCRcode_subs_names)
            {
                if (s1 == b1.text) { found = true; break; }
            }
            if (!found)
            {
                OCRcode_subs_names.Add(b1.text);// add the name now copy the code....
                // need to insert text name...
                OCRcode_subs[max_n_subs] = b1.text + sc + "NOP"+sc+";subroutine "+b1.text; max_n_subs++;
                for (int k = 1; k < o1.n_asm; k++)
                {
                    try
                    {
                        OCRcode_subs[max_n_subs] = o1.Asmcode[k];
                        OCRcode_subs[max_n_subs] = OCRcode_subs[max_n_subs].Replace("linexx", "line" + j.ToString());
                        max_n_subs++;
                    }
                    catch
                    {
                        MessageBox.Show("Subroutine code too long!!!  Write neater code and come back later", "Error"); return "";
                    }
                }
                //now need to insert the RET.. last line will be NOP so change....
                OCRcode_subs[max_n_subs-1] = OCRcode_subs[max_n_subs-1].Replace("NOP", "RET");
            }

            return s;
        }

        #endregion

        #region DisplayCodeMenu
        

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.ParseOutput d1 = new FlowDiagrams.Dialogs.ParseOutput();
            d1.Setup(InterpretDiagram());
            d1.ShowDialog();
        }
        private void convertToOCRAsmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //going to parse the file and then convert...
            InterpretDiagram();
            string s = "";
            for (int i = 0; i < max_n; i++)
            {
                s += OCRCode[i] + Environment.NewLine;
            }
            Dialogs.ParseOutput d1 = new FlowDiagrams.Dialogs.ParseOutput();
            d1.Setup(s);
            d1.ShowDialog();
        }
        #endregion

        #region FileMenu

        private string SaveAs(string suggest)
        {
            float oldscale = scale;
            SetScale(100, scale); Invalidate();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "fld files (*.fld)|*.fld|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = suggest;
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    Stream TestFileStream = saveFileDialog1.OpenFile();
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(TestFileStream, d);
                    TestFileStream.Close();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Save Error" + e1.Message);
            }
            SetScale(oldscale, scale); Invalidate();
            saveFileDialog1.Dispose();
            return filename;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float oldscale = scale;
            SetScale(100, scale); Invalidate();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "fld files (*.fld)|*.fld|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    Stream TestFileStream = saveFileDialog1.OpenFile();
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(TestFileStream, d);
                    TestFileStream.Close();
                }
            }
            catch(Exception e1)
            {
                MessageBox.Show("Save Error" + e1.Message);
            }
            SetScale(oldscale, scale); Invalidate();
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "fld files (*.fld)|*.fld|All files (*.*)|*.*";
            loadFileDialog.FilterIndex = 0;
            loadFileDialog.RestoreDirectory = true;
            try
            {
                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //going to delete controls....
                    foreach (FlowObject f in d.BoxesList) 
                    {
                        Controls.Remove(f);
                        f.Box_Clicked -= Box_Clicked;
                        f.Box_RightClicked -= Box_RightClicked;
                    }
                    d = new FlowDiagram(); scale = 100; d.scale = 100;
                    Stream TestFileStream = loadFileDialog.OpenFile();
                    BinaryFormatter serializer = new BinaryFormatter();
                    d = (FlowDiagram)serializer.Deserialize(TestFileStream);
                    simpleModel=false;
                    foreach (FlowObject f in d.BoxesList) 
                    { 
                        Controls.Add(f);
                        f.Box_Clicked += new FlowObject.Click_Select(Box_Clicked);
                        f.Box_RightClicked += new FlowObject.Click_Right(Box_RightClicked);
                        System.Type t1 = typeof(SimpleDecisionBox);
                        if ((f is SimpleDecisionBox)||(f is WaitSec)||(f is OutputPin)||(f is PlayNote))
                        {
                            simpleModel=true;
                        }
                    }
                    filename = loadFileDialog.FileName;
                    ReflectModel(true);
                    TestFileStream.Close();
                }
                
                this.Invalidate();
            }
            catch(Exception e1)
            {
                MessageBox.Show("Load Error" + e1.Message);
            }

        }
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (filename == "") saveAsToolStripMenuItem_Click(sender, e);

            try
            {
                    Stream TestFileStream = File.Open(filename, FileMode.Create);
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(TestFileStream, d);
                    TestFileStream.Close();

            }
            catch (Exception e1)
            {
                MessageBox.Show("Save Error" + e1.Message);
            }
        }
        #endregion

        #region PrinterSupport
        private void printDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            PrintDialog pd1 = new PrintDialog();
            pd1.Document = pd;
            pd1.ShowDialog();
            pd.PrintPage += new PrintPageEventHandler(PrintImage);
            pd.Print(); 
        }
        void PrintImage(object o, PrintPageEventArgs e)
        {
            int x = SystemInformation.WorkingArea.X;
            int y = SystemInformation.WorkingArea.Y;
            int width = this.Width;
            int height = this.Height;
            d.Draw(e.Graphics, new Point(0, 0));

            foreach (FlowObject f in d.BoxesList)
            {
                f.Draw(e.Graphics, new Rectangle(f.x_position,f.y_position,f.width,f.height));
            }
        }
        #endregion 



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData ==Keys.F1)
            {
                string s = keyData.ToString();
                s = s;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Scaling
        private void SetScale(float newscale, float oldscale)
        {
            if (newscale == oldscale) return;
            scale = newscale;
            d.scale = scale;
            foreach (FlowObject o in d.BoxesList)
            {
                o.scale = scale; o.Scale(new SizeF(newscale/oldscale, newscale/oldscale));
                o.Invalidate();
            }
            this.Invalidate();
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.ScalingDialog d1 = new Dialogs.ScalingDialog();
            d1.setup(scale);
            float scale1 = scale;
            d1.ShowDialog();
            SetScale(d1.scale,scale1);
        }

        private void toolStripMenu_Scale100_Click(object sender, EventArgs e)
        {
            SetScale(100, scale);
        }

        private void toolStripMenu_Scale80_Click(object sender, EventArgs e)
        {
            SetScale(80, scale);
        }

        private void toolStripMenu_Scale60_Click(object sender, EventArgs e)
        {
            SetScale(60, scale);
        }

        private void toolStripMenu_Scale50_Click(object sender, EventArgs e)
        {
            SetScale(50, scale);
        }

        private void toolStripMenu_Scale40_Click(object sender, EventArgs e)
        {
            SetScale(40, scale);
        }

        private void toolStripMenu_Scale25_Click(object sender, EventArgs e)
        {
            SetScale(25, scale);
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float s1 = scale;
            SetScale(100, scale);
            foreach (FlowObject o in d.BoxesList)
            {
                int x = o.x_position;
                if ((x % 200) < 100) x = 200 * (x / 200); else x = 200 * (x / 200) + 200;
                if (x < 200) x = 100;
                o.x_position =x;

                int y = o.y_position;
                if ((y % 100) < 50) y = 100 * (y / 100); else y = 100 * (y / 100) + 100;
                if (y < 100) y = 50;
                o.y_position = y;

                o.Location = new Point(o.x_position - o.width / 2, o.y_position);
            }
            SetScale(s1, 100);
            Invalidate();
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialogs.About d1 = new Dialogs.About();
            d1.Setup(simpleModel);
            d1.ShowDialog();
        }


        public void CheckHexByte(string s)
        {
            if (s.Length > 0)
            {
                try
                {
                    int i = System.Convert.ToInt32(s, 16);
                    if (i < 0) throw new System.Exception();
                    if (i > 0xff) throw new System.Exception();
                }
                catch
                {
                    MessageBox.Show("Byte " + s + " is not a valid Hexdecimal value between 0 and FF");
                }
            }
        }
        private bool ProcessXMLBox(FlowDiagram d, XmlTextReader r1)
        {
            string s_type, s_cmd, s_id, s_next, sd, ss,s_no,s_yes;
            Guid id = new Guid() ;
            r1.MoveToFirstAttribute();
            r1.MoveToAttribute("type"); s_type = r1.Value;
            r1.MoveToAttribute("command"); s_cmd = r1.Value;
            r1.MoveToAttribute("id"); s_id = r1.Value;
            r1.MoveToAttribute("nextBox"); s_next = r1.Value;
            r1.MoveToAttribute("Sd"); sd = r1.Value;
            r1.MoveToAttribute("Ss"); ss = r1.Value;
            r1.MoveToAttribute("noBox");s_no=r1.Value;
            r1.MoveToAttribute("yesBox");s_yes=r1.Value;
            

            switch (s_type)
            {
                case "input":  
                    InputBox zin = new InputBox(40, 40,sd); 
                    AddFlowObject(zin);
                    id = zin.id;
                    break;
                case "output": 
                    OutputBox zout = new OutputBox(40, 40, ss); 
                    AddFlowObject(zout);
                    id = zout.id;
                    break;
                case "process": ProcessBox zproc = new ProcessBox(40, 60); zproc.Sn = sd;
                    switch (s_cmd.ToUpper())
                    {
                        case "MOVI": zproc.op_type = ProcessType.movi; zproc.b=ss;    break;
                        case "MOV":  zproc.op_type = ProcessType.mov;  zproc.Sm=ss;   break;
                        case "ADDI": zproc.op_type = ProcessType.addi; zproc.b = ss;  break;
                        case "SUBI": zproc.op_type = ProcessType.subi; zproc.b = ss;  break;
                        case "ADD":  zproc.op_type = ProcessType.add;  zproc.Sm = ss; break;
                        case "SUB":  zproc.op_type = ProcessType.sub;  zproc.Sm = ss; break;
                        case "AND":  zproc.op_type = ProcessType.and;  zproc.b = ss;  break;
                        case "ORR":  zproc.op_type = ProcessType.orr;  zproc.b = ss;  break;
                        case "XOR":  zproc.op_type = ProcessType.xor;  zproc.b = ss;  break;
                        case "WAIT": zproc.op_type = ProcessType.wait; zproc.b = ss;  break;
                    }
                    zproc.Asm();
                    AddFlowObject(zproc);
                    id=zproc.id;
                    break;
                case "return":
                    ReturnBox zret = new ReturnBox(50, 50);
                    AddFlowObject(zret);
                    id = zret.id;
                    break;
                case "decision":
                    DecisionBox zdec = new DecisionBox(40, 40);
                    zdec.Asm(sd, ss, s_cmd);
                    AddFlowObject(zdec);
                    id = zdec.id;
                    break;
                case "subroutine":
                    SubProcessBox zsub = new SubProcessBox(40, 40);
                    AddFlowObject(zsub);
                    id = zsub.id;
                    zsub.text = s_cmd.Substring(0, s_cmd.Length - 2);
                    foreach (sub_process_list p in list_subs)
                    {
                        if (zsub.text == p.name) 
                        { 
                            zsub.FileName = p.filename; 
                        }
                    }

                    FileStream f1 = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                     
                    BinaryFormatter serializer = new BinaryFormatter();
                    d = (FlowDiagram)serializer.Deserialize(f1);
                    Form1 ff = new Form1(d);
                    ff.Text = filename;      
                    d = ff.d;
                    ff.InterpretDiagram();int n=0;
                    for (int i = 0; i < ff.max_n; i++)
                    {
                        zsub.Asmcode[i] = ff.OCRCode[i];
                    }
                    zsub.n_asm = ff.max_n;
                    break;


            }
            box_links n1 = new box_links(id,s_id, s_next);
            if (s_type == "decision"){n1.next_id = s_yes; n1.no_id = s_no;}
            list_boxes.Add(n1);
            return true;
        }

        class sub_process_list
        {
            public sub_process_list(string Name, string Filename)
            {
                name = Name; filename = Filename;
            }
            public string name;
            public string filename;
        }
        List<sub_process_list> list_subs = new List<sub_process_list>();

        class box_links
        {
            public Guid this_guid;
            public string this_id;
            public string next_id;
            public Guid next_guid;  /// wont be known when made...
            public string no_id;//for decision boxes
            public Guid no_guid;

            public box_links(Guid GUID,string ID, string next)
            {
                this_guid = GUID; this_id = ID; next_id = next; no_id = null;
            }
            public box_links(Guid GUID, string ID, string next, string NoId)
            {
                this_id = ID; next_id = next; no_id = NoId; this_guid = GUID;
            }
        }
        List<box_links> list_boxes = new List<box_links>();

        private bool ProcessXMLNode(FlowDiagram d,   XmlTextReader r1, int depth,bool sub_only,string start_id)
        {
            string id = "0"; string s = ""; string s1 = ""; 
            list_boxes.Clear();

            //going to delete controls....
            foreach (FlowObject f in d.BoxesList)
            {
                Controls.Remove(f);
                f.Box_Clicked -= Box_Clicked;
                f.Box_RightClicked -= Box_RightClicked;
            }
            d.BoxesList.Clear();
            Controls.Add(d.start); d.BoxesList.Add(d.start);


            box_links startb = new box_links(d.start.id,"0", start_id); 
            list_boxes.Add(startb);
            while (!r1.EOF)
            {
                r1.Read(); r1.MoveToContent(); s = r1.Name;
                    if (r1.Depth == depth)
                    {
                        switch (r1.Name)
                        {
                            case "sub":
                                if (sub_only)
                                {
                                    r1.MoveToAttribute("start");
                                    id = r1.Value;
                                    r1.MoveToAttribute("name"); s = r1.Value;//name
                                    ProcessXMLNode(d, r1, r1.Depth, false, id);
                                    s1 = SaveAs(s);//filename
                                    //need to remember this.... 
                                    sub_process_list s3 = new sub_process_list(s, s1);
                                    list_subs.Add(s3);
                                }
                                break;
                            case "box":
                                if (!sub_only)
                                {
                                    ProcessXMLBox(d, r1);
                                }
                                break;
                            default   : break;
                        }
                    }
            }
            if (!sub_only)
            {
                foreach (box_links b in list_boxes)
                {
                    foreach (box_links b1 in list_boxes)
                    {
                        if (b1.this_id == b.next_id) b.next_guid = b1.this_guid;
                    }

                    foreach (box_links b1 in list_boxes)
                    {
                        if (b1.this_id == b.no_id) b.no_guid = b1.this_guid;
                    }
                }
                // so list is complete
                foreach (box_links b in list_boxes)
                {
                    foreach (FlowObject f in d.BoxesList)
                    {
                        if (f.id == b.this_guid)
                        {
                            foreach (FlowObject f1 in d.BoxesList)
                            {
                                if (f1.id == b.next_guid)
                                {
                                    f.FlowOut = f1;
                                }
                            }
                            foreach (FlowObject f1 in d.BoxesList)
                            {
                                if (f1.id == b.no_guid)
                                {
                                    f.FlowoutSide = f1;
                                }
                            }                        
                        }
                    }
                    //now going to try to space out...
                    foreach (FlowObject o in d.BoxesList)
                    {
                        o.Parsed = false;
                    }//dodgy use of bool parsed to remember if we have positioned this one... to avoid changing objects!
                    FlowObject f2 = d.start; f2 = f2.FlowOut;

                    int x = 200; int y = 180; 
                    Allocate_position(f2, 200, 100, ref x, ref y);
                }
            }
            return true;
        }

        private void Allocate_position(FlowObject f2, int xinc, int yinc, ref int x, ref int y)
        {
            while ((f2.FlowOut != null) && (!f2.Parsed))
            {
                f2.x_position = x;
                f2.y_position = y;
                f2.Location = new Point(x - f2.width / 2, y);
                y += yinc;
                if (y > 800) { y = 180; x += xinc; }
                f2.Parsed = true;
                if (f2.FlowoutSide != null) Allocate_position(f2.FlowoutSide, xinc, yinc, ref x, ref y);
                f2 = f2.FlowOut;
            }

        }
        private void loadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadFileDialog = new OpenFileDialog();
            loadFileDialog.Filter = "fld files (*.xml)|*.xml|All files (*.*)|*.*";
            loadFileDialog.FilterIndex = 0;
            loadFileDialog.RestoreDirectory = true;
            //try
            {
                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //going to delete controls....
                    foreach (FlowObject f in d.BoxesList)
                    {
                        Controls.Remove(f);
                        f.Box_Clicked -= Box_Clicked;
                        f.Box_RightClicked -= Box_RightClicked;
                    }
                    d = new FlowDiagram(); scale = 100; d.scale = 100; Controls.Add(d.start);
                    Stream TestFileStream = loadFileDialog.OpenFile();
                    //parse once to resolve subs...?
                    string s;
                    XmlTextReader r1 = new XmlTextReader(TestFileStream);
                    //first pass to do subs....

                    while (!r1.EOF)
                    {
                        r1.Read(); r1.MoveToContent();s = r1.Name;
                        if ((s == "main")&&(r1.NodeType==XmlNodeType.Element))
                        {
                            r1.MoveToAttribute("start"); 
                            ProcessXMLNode(d, r1, r1.Depth, true, r1.Value);//process sub s only
                        }
                    }
                    r1.Close();
                    TestFileStream = loadFileDialog.OpenFile();
                    r1 = new XmlTextReader(TestFileStream);
                    while (!r1.EOF)//to do main prog
                    {
                        r1.Read(); r1.MoveToContent(); s = r1.Name;
                        if ((s == "main") && (r1.NodeType == XmlNodeType.Element))
                        {
                            r1.MoveToAttribute("start");
                            ProcessXMLNode(d, r1, r1.Depth, false, r1.Value);//process boxes
                        }
                    }
                }

                this.Invalidate();
            }
            //catch (Exception e1)
            //{
             //   MessageBox.Show("Load Error:  " + e1.Message);
            //}

/*
                    BinaryFormatter serializer = new BinaryFormatter();
                    d = (FlowDiagram)serializer.Deserialize(TestFileStream);
                    foreach (FlowObject f in d.BoxesList)
                    {
                        Controls.Add(f);
                        f.Box_Clicked += new FlowObject.Click_Select(Box_Clicked);
                        f.Box_RightClicked += new FlowObject.Click_Right(Box_RightClicked);
                    }
                    filename = loadFileDialog.FileName;
                    TestFileStream.Close();
                }

                this.Invalidate();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Load Error" + e1.Message);
            }
*/
        }

        private void modelTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleModel = !simpleModel;
            ReflectModel(false);
        }

        private void ReflectModel(bool locked)
        {
            ToolStripItem[] fred = this.menuStrip1.Items.Find("newToolStripMenuItem", true);
            foreach (ToolStripItem i in fred)
            {
                i.Enabled = !simpleModel;
            }
            //find the simple insert menu and enable it...
            fred = this.menuStrip1.Items.Find("simpleInsertToolStripMenuItem", true);
            foreach (ToolStripItem i in fred)
            {
                i.Enabled = simpleModel;
            }

            fred = this.menuStrip1.Items.Find("modelTypeToolStripMenuItem", true);
            foreach (ToolStripItem i in fred)
            {
                i.Enabled = !locked;
                i.Text = (simpleModel ? "Switch to Full Model" : "Switch to SimpleModel");
            }
            this.Text = "Using " + (simpleModel ? "SIMPLE Model and 16F84" : "AS Model and 16F886");
        }

        private void simpleInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }





    }
}
