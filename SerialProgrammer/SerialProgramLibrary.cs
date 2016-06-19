using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialProgrammer
{
    public class SerialProgrammer
    {
        #region Variables
        static int MAX_LINES_ASM = 4000;
        public string[] OCRCode = new string[MAX_LINES_ASM];// has the OCR code
        public int max_OCRlines = 0;
        public string[] AssemblerCode = new string[MAX_LINES_ASM];// has the microchip assembler code
        public int max_ASMlines = 0;
        public string[] hexcode = new string[MAX_LINES_ASM];//has 16F876 opcodes for OCR instructions
        public int max_hexlines = 0;
        public string[] PicCode = new string[MAX_LINES_ASM];//has 16F876 data read back
        public string[] labels = new string[MAX_LINES_ASM];
        public int[] labelvalues_code = new int[MAX_LINES_ASM];
        public int maxlabels = 0;
        public int Code_start = 0x500;
        public int Table_start = 0x400;
        public int Code_break = 0x4f0;
        public System.IO.Ports.SerialPort serialPort1;
        public bool SimpleModel = false;// used to flag if we are using the simple 16F84A model....
        public bool InLineAssembler = false;
        #endregion

        //delegate for call backs to forms...
        public delegate void UpdateDisplay(int progress_max,int progress_value,string text);

        #region SerialStuff

        private bool ProgramQuadWord(int address, string data)
        {
            //this is really for 16F876A which has to program 4 words (8 bytes) at a time..
            bool success = false; string s = "";
            s = SendMessage("@@@@w0" + address.ToString("X") + data);
            s = SendMessage("@@@@Z");//little delay...
            s = SendMessage("@@@@r0" + address.ToString("X"));
            if (s.Length > 15) if (s.Substring(0, 16) == data) success = true;
            return success;
        }
        private bool Program16Word(int address, string data)
        {
            //this is really for 16F886 which has to program 16 words (32 bytes) at a time..
            bool success = false; string s = "";
            s = SendMessage("@@@@w0" + address.ToString("X") + data);
            s = SendMessage("@@@@r0" + address.ToString("X"));
            if (s.Length > 63) if (s.Substring(0, 64) == data) success = true;
            return success;
        }
        private bool ProgramWord(int address, string data)
        {
            bool success = false; int tries = 0; string s = "";
            while ((!success) && (tries < 4))
            {
                s = SendMessage("@@@@R0" + address.ToString("X"));
                if (s == data) return true;
                s = SendMessage("@@@@W0" + address.ToString("X") + data);
                s = SendMessage("@@@@R0" + address.ToString("X"));
                if (s == data) success = true;
                tries++;
            }
            return success;
        }

        private string ReadBlock(UpdateDisplay update1, int start_address, int No_Words)
        {
            string s = ""; string s1 = "";
            int progress_value = 0;
            update1(1 + No_Words/16, 0, "Reading Code");
            int add = start_address;
            int n = No_Words;

            while (n > 0)
            {
                s1="";
                while(s1.Length!=64)s1 = SendMessage("@@@@r0" + add.ToString("X"));
                s += s1;
                n -= 16; add += 16;progress_value++;
                update1(1 + No_Words / 16, progress_value, "Reading Code");
            }
            while (s.Length < 4 * No_Words) 
                s += "F";
            return s.Substring(0, 4 * No_Words);//trim to right length...
        }
        private void ReadCode(UpdateDisplay update1)
        {
            string s = "";
            int n_words = CountWordsinCode(); int progress_value = 0;
            update1(1 + n_words / 16, 0, "Reading Code");
            int add = Code_start; int n = n_words; s = "";
            //going to use block read (r) for speed 
            while (n > 0)
            {
                s += SendMessage("@@@@r0" + add.ToString("X"));
                n -= 16; add += 16;
                progress_value++;
                update1(1 + n_words / 16, progress_value, "Reading Code");
            }
            for (int i = 0; i < max_hexlines; i++)
            {
                PicCode[i] = s.Substring(0, hexcode[i].Length);
                s = s.Substring(hexcode[i].Length, s.Length - hexcode[i].Length);
            }
        }
        public bool Initialize_Comms(UpdateDisplay update1, System.IO.Ports.SerialPort serialPort) 
        {
            serialPort1 = serialPort;
            string s = SendMessage("@@@@Z");
            if (!s.Contains("Ready"))
            {
                update1(10, 0, "Unable to communicate with PIC. Check the serial connection is in place and reset the PIC and try again."); return false;
            }
            update1(10, 0, s + Environment.NewLine); return true;
        }
        public string SendMessage(string s)
        {
            byte[] byte1 = new byte[2];
            byte1[0] = 0x0d; byte1[1] = 0;
            serialPort1.ReadExisting();
            serialPort1.Write(s);
            serialPort1.Write(byte1, 0, 1);
            return WaitMessage();
        }
        public void SendMessage_NoReply(string s)
        {
            byte[] byte1 = new byte[2];
            byte1[0] = 0x0d; byte1[1] = 0;
            serialPort1.ReadExisting();
            serialPort1.Write(s); serialPort1.Write(byte1, 0, 1);
            return;
        }
        private string WaitMessage()
        {
            int x = 0;
            string r = "";
            serialPort1.ReadTimeout = 100; x = 0;
            //running at 9k6 baud, so about 1k bytes/sec, so about 1byte/ms
            //so should be OK if timeout is 20 ms. allow x5 twice
            for (int n = 0; n < 5; n++)
            {
                x = 0;
                try
                {
                    x = serialPort1.ReadByte();
                }
                catch { }
                if (x == 0x3f) x = 0;//ignore these
                if (x < 0x20) x = 0;
                if (x > 0)
                {
                    if (x == 0x40)//terminator..
                    {
                        return r;
                    }
                    else
                    {
                        r += (char)x; n = 0;//more bytes to get...
                    }
                }
            }
            return r;
        }
        public bool WaitBreakMessage(ref OCRmodel o1)
        {
            if (serialPort1.BytesToRead < 10) return false;
            string s= WaitMessage(); //s has status reg
            if (s == "") return false;
            o1.Z = "0"; o1.C = "0";
            int z1 = System.Convert.ToInt16(s.Substring(1,1),16);//bit 2 is Z flag
            if ((z1 & 0x04) == 0x04) o1.Z = "1";  //bit 2 is Z flag
            if ((z1 & 0x01) == 0x01) o1.C = "1";  //bit 0 is C flag
            o1.W = s.Substring(2, 2);
            for (int i = 0; i < 8; i++)
            {
                o1.S[i] = s.Substring(4 + 2 * i, 2);
            }
            o1.InputPort = s.Substring(20, 2);
            o1.OutputPort = s.Substring(22, 2);
            return true;
        }

        private bool ProgramAny(UpdateDisplay update1, int ProgramWordLength, int address_start, string data)
        {
            int add = address_start;
            int n2 = (4 * ProgramWordLength);
            string s2 = data; int n1 = s2.Length;
            string s3 = ReadBlock(update1, address_start, n1 / 4);
            string s = ""; string s1 = "";

            //need ProgramWordLength words at a time so 4*ProgramWordLength chars
            //need s2 and s3 to be a multiple of 4*ProgramWordLength chars
            while (n1 % n2 != 0) { s2 += "0"; s3 += "0"; n1++; }
            int progress = 0; int max_progress = s2.Length + n2;

            while (s2.Length > n2-1)
            {
                update1(max_progress, progress, "Programming " + add.ToString("X"));
                s = s2.Substring(0, n2); s1 = s3.Substring(0, n2);
                if (s != s1)//need to program
                {
                    switch (ProgramWordLength)
                    {
                        case 16: if (!Program16Word(add, s)) return false; break;
                        case 4: if (!ProgramQuadWord(add, s)) return false; break;
                        case 1: if (!ProgramWord(add, s)) return false; break;
                        default: update1(max_progress, progress, "Unknown WordLength"); return false;
                    }
                }
                add += ProgramWordLength;progress += n2;
                if (progress > max_progress) max_progress += n2;
                s3 = s3.Substring(n2); s2 = s2.Substring(n2);
            }
            return true;
        }
        private bool Program876(UpdateDisplay update1)
        {
            int add = Code_start; string s = ""; string s1 = "";
            int progress = 0;
            update1(max_hexlines + 1, progress, "Programming ");
            for (int i = 0; i < max_hexlines; i++)
            {
                if (PicCode[i] != hexcode[i])//need to program...
                {
                    s = hexcode[i];
                    while (s.Length > 3)
                    {
                        s1 = s.Substring(0, 4);
                        s = s.Substring(4, s.Length - 4);
                        if (!ProgramWord(add, s1)) { return false; }
                        add++;
                    }
                }
                else
                {
                    add += hexcode[i].Length / 4;
                }

                if (progress < max_hexlines) progress++;
                update1(max_hexlines + 1, progress, "Programming "+s1);
            }    
            return true;
        }
        private bool Program876A(UpdateDisplay update1)
        {
            int add = Code_start; string s, s1, s2, s3;
            
            int n1 = 0; s2 = ""; s3 = "";
            for (int i = 0; i < max_hexlines; i++)
            {
                s2 += hexcode[i]; s3 += PicCode[i]; n1 += hexcode[i].Length;
            }
            int progress = 0; int max_progress = s2.Length + 4;
            update1(max_progress, progress, "Programming ");
            //need 8 bytes at a time...so 16 chars
            //need s2 and s3 to be a multiple of 16.....
            while (n1 % 16 != 0) { s2 += "0"; s3 += "0"; n1++; }
            while (s2.Length > 15)
            {
                s = s2.Substring(0, 16); s1 = s3.Substring(0, 16);
                if (s != s1)
                {
                    if (!ProgramQuadWord(add, s)) { return false; }
                }
                add += 4;progress+=16;
                if (progress > max_progress) max_progress += 16;

                update1(max_progress, progress, "Programming "+s);
                s3 = s3.Substring(16); s2 = s2.Substring(16);

            }
            return true;
        }
        private bool Program886(UpdateDisplay update1)
        {
            int add = Code_start; string s, s1, s2, s3;

            int n1 = 0; s2 = ""; s3 = "";
            for (int i = 0; i < max_hexlines; i++)
            {
                s2 += hexcode[i]; s3 += PicCode[i]; n1 += hexcode[i].Length;
            }

            //need 16 words at a time...so 64 chars
            //need s2 and s3 to be a multiple of 64.....
            while (n1 % 64 != 0) { s2 += "0"; s3 += "0"; n1++; }
            int progress = 0; int max_progress = s2.Length + 2;
            update1(max_progress, progress, "Programming ");

            while (s2.Length > 63)
            {
                s = s2.Substring(0, 64); s1 = s3.Substring(0, 64);
                if (s != s1)
                {
                    if (!Program16Word(add, s)) { return false; }
                }
                add += 16;
                progress += 64;
                if (progress > max_progress) max_progress += 64;
                s3 = s3.Substring(64); s2 = s2.Substring(64);
                update1(max_progress, progress, "Programming "+s);
            }
            return true;
        }
        public bool ProgramPIC(UpdateDisplay update1)
        {
            //check proc type....
            string pic_type = SendMessage("@@@@X");
            string s = ""; int No_Words = 0; int n1 = 0;
            switch (pic_type)
            {
                case "16F876": No_Words = 1; break;
                case "16F876A": No_Words = 4; break;
                case "16F886": No_Words = 16; break;
                default: update1(1, 1, "unknown PIC type :" + pic_type); return false;
            }
            //check firmware version 

            string version_s = SendMessage("@@@@V");
            //response should be Version m.nn
            if (version_s.Length < 11) { update1(1, 1, "Failed to read firmware version  :" + version_s); return false; }
            version_s = version_s.Substring(8);
            double version = System.Convert.ToDouble(version_s);
            //has already produced MC for version 1.nn or less....
            string s1 = "";
            max_hexlines = 0;
            if (version >= 2) // has output as port B....
            {
                if (!GenerateMC(ref s1, true))
                {
                    update1(1, 1, "Assembly failure  :" + s1); return false;
                }
            }
            else
            {
                if (!GenerateMC(ref s1, false))
                {
                    update1(1, 1, "Assembly failure  :" + s1); return false;
                }
            }
            for (int i = 0; i < max_hexlines; i++) s += hexcode[i];
            if (!ProgramAny(update1, No_Words, Code_start, s)) return false;
            if (FindLabel_asm("TABLE", ref n1))
            {
                //n1 has the word address..... 4* for chars...
                if (s.Length <= n1 * 4) { update1(1, 0, "TABLE exists but no data in the TABLE"); return false; }
                s = s.Substring(n1 * 4);
                if (s.Length > 4 * 255) { update1(1, 0, "TABLE too long"); return false; }
                if (!ProgramAny(update1, No_Words, Table_start, s)) return false;
            }
            return true;
        }

        #endregion

        #region Assembly Routines
        private int CountWordsinCode()
        {
            int no_words = 0;
            for (int i = 0; i < max_hexlines; i++)
            {
                no_words += hexcode[i].Length / 4;
            }
            return no_words;
        }
        public bool CodeHasBreaks()
        {
            bool found = false;
            for (int i = 0; i < max_OCRlines; i++)
            {
                if (OCRCode[i].Contains("BREAK")) found = true;
            }
            return found;

        }
        public bool GenerateMC(ref string ErrorMessage, bool out_PortB)   // has OCR code on input and generates Assembler and MC
        {
            //out_PortB = true;
            //out_PortB is true if we are swopping ports so that Port B is output....
            string s2 = ""; int asm_no = 0; string s3 = ""; 
            string line = ""; string s1 = ""; int n = 0;
            string label = "";string op = ""; int k=0;
            bool hastable = false;
            
            maxlabels = 0;
            labels[maxlabels] = "WAIT1MS";
            labelvalues_code[maxlabels] = 0x300 - Code_start;
            if (SimpleModel) labelvalues_code[maxlabels] = 0x01 - Code_start;// does  wait in msecs
            maxlabels++;
            if (SimpleModel)
            {
                labels[maxlabels] = "WAIT1SEC";
                labelvalues_code[maxlabels] = 0x02 - Code_start;// does  wait in secs
                maxlabels++;
            }
            if (SimpleModel)
            {
                labels[maxlabels] = "PLAYS0";
                labelvalues_code[maxlabels] = 0x03 - Code_start;// Plays note in R0 for 1 sec
                maxlabels++;
            }
            labels[maxlabels] = "READADC";//not used in simple code...
            labelvalues_code[maxlabels] = 0x301 - Code_start;
            maxlabels++;
            labels[maxlabels] = "READTABLE";//not used in simple code...
            labelvalues_code[maxlabels] = 0x302 - Code_start;
            maxlabels++;

            labels[maxlabels] = "BREAK";//not used in simple code...
            labelvalues_code[maxlabels] = 0x303 - Code_start;
            maxlabels++;

            labels[maxlabels] = "READEEPROM";//not used in simple code...
            labelvalues_code[maxlabels] = 0x307 - Code_start;
            maxlabels++;

            labels[maxlabels] = "WRITEEEPROM";//not used in simple code...
            labelvalues_code[maxlabels] = 0x308 - Code_start;
            maxlabels++;
            //following used for Winstar WG12864A display (Tristam Newmann)
            labels[maxlabels] = "TRIS_CLEAR";//not used in simple code...
            labelvalues_code[maxlabels] = 0x309 - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_RESET";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30A - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SETX";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30B - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SETY";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30C - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SETZ";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30D - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_CMD";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30E - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_WRDATA";//not used in simple code...
            labelvalues_code[maxlabels] = 0x30F - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_RDDATA";//not used in simple code...
            labelvalues_code[maxlabels] = 0x310 - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SELCS1";//not used in simple code...
            labelvalues_code[maxlabels] = 0x311 - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SELCS2";//not used in simple code...
            labelvalues_code[maxlabels] = 0x312 - Code_start;
            maxlabels++;
            labels[maxlabels] = "TRIS_SETPIXEL";//not used in simple code...
            labelvalues_code[maxlabels] = 0x313 - Code_start;
            maxlabels++;


            //first run through to get the labels...
            n = 0;//here n counts the number of OCR code lines for the labels.
            for (int i = 0; i < max_OCRlines; i++)
            {
                line = OCRCode[i];
                s1 = line;
                if (!DecodeLine(ref s1, ref label, ref op, ref k, ref  k, ref s2)) //only need labels here
                {//we have an error...
                    ErrorMessage = s1 + "   :" + line; return false;
                }
                s1 = line; asm_no = 0;
                ProcessLine_ToAssembler(s1, ref s2, ref s3, ref asm_no, false, ref s1, out_PortB);
                if (label == "TABLE")
                {
                    //handled in serial programmer by separate program run for TABLE to 0x400
                    //in cross assembler we insert an ORG directive
                    if (hastable) { ErrorMessage = "Only 1 TABLE is allowed in the OCR emulator"; return false; }
                    hastable = true;
                }
                if (label != "")
                {
                    labels[maxlabels] = label;
                    labelvalues_code[maxlabels] = n;
                    maxlabels++;
                }
                if (op != "")n+= asm_no;
            }

            int no_linescode = 0;
            string outputASM  = "";
            string outputHEX = "";
            string[] outputASM_split = new string[20];
            char[] c1 = new char[1]; c1[0] = (char)0x0a;
            n = 0;max_ASMlines= 0;

            for (int i = 0; i < max_OCRlines; i++)
            {
                line = OCRCode[i];
                outputHEX = ""; outputASM = "";
                if (!ProcessLine_ToAssembler(line, ref outputASM, ref outputHEX, ref no_linescode, true,ref s1, out_PortB))
                {//we have an error...
                    ErrorMessage = s1 + "   :" + line; return false;
                }
                //so outputASM if the microchip code
                //outputHEX is the MC  and no_linescode is the number of lines of mircochipcode
                hexcode[n] = outputHEX; n++;//one line of MC per OCR line...
                outputASM_split = outputASM.Split(c1);
                foreach (string s5 in outputASM_split) { if (s5.Length > 0) { AssemblerCode[max_ASMlines] = s5; max_ASMlines++; } }          
            }
            max_hexlines = n;


            return true;
        }
        protected bool ProcessLine_ToAssembler(string input, ref string line, ref string hexline, ref int no_lines, bool ResolveLabels,ref string ErrorString, bool out_Option)
        {

            //out_Option is true if the output is to port B
            string nl = Environment.NewLine;
            string s1 = "";
            if (ResolveLabels)
            {
                s1 = "k";
            }
            char t1 = (char)0x09; string t = ""; t += t1;
            //line = line.ToUpper();
            string label = ""; int i = 0;
            int bit1 = 0; int bit2 = 0;
            string op = "";
            string data = ""; int Rs = 0; int Rd = 0;
            if (!DecodeLine(ref input, ref label, ref op, ref Rs, ref Rd, ref data))
            {
                ErrorString = input;//returns any errorcode...
                return false;
            }
            line = "";
            if (op == "BLANK") return true;
            if (op == "") return true;
            if (label == "TABLE"){ line += t + "ORG 0x400" + t + nl + label + nl + t; }
            else { line += label + (char)0x09; }

            if (!InLineAssembler)
            {   //else see below
                switch (op)  //all OCR codes...
                {
                    case "}": line += "}" + nl; no_lines += 1; break;
                    case "":
                        line += "nop" + nl;
                        hexline += "0000"; no_lines += 1;
                        break;
                    case "NOP":
                        line += "nop" + nl;
                        hexline += "0000"; no_lines += 1;
                        break;
                    case "MOVI":
                        if (Rd < 10)
                        {
                            line += "movlw " + t + "0x" + data + nl;
                            line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                            if (data.Length == 1) data = "0" + data;
                            hexline += "30" + data;// movlw
                            hexline += "00" + (Rd + 0xA0).ToString("X");//movwf  (S0 is stored at 0x020 in PIC memory
                            no_lines += 2;
                        }
                        else
                        {//need to use bank 3....  
                            line += "movlw " + t + "0x" + data + nl;
                            line += "bsf STATUS,5" + nl;
                            line += "bsf STATUS,6" + nl;
                            line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                            line += "bcf STATUS,5" + nl;
                            line += "bcf STATUS,6" + nl;
                            if (data.Length == 1) data = "0" + data;
                            hexline += "30" + data;// movlw
                            hexline += "1613"; hexline += "1603";
                            hexline += "00" + (Rd + 0xA0).ToString("X");//movwf  (S0 is stored at 0x020 in PIC memory
                            hexline += "1213"; hexline += "1303";
                            no_lines += 6;
                        }
                        break;
                    case "MOV":
                        line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;//ie to W
                        line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                        if (data.Length == 1) data = "0" + data;
                        hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                        hexline += "00" + (Rd + 0xA0).ToString("X");//movwf
                        no_lines += 2;
                        break;
                    case "ADD":
                        line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;
                        line += t + "addwf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                        hexline += "07" + (Rd + 0xA0).ToString("X");//addwf
                        no_lines += 2;
                        break;
                    case "SUB":
                        line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;
                        line += t + "subwf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                        hexline += "02" + (Rd + 0xA0).ToString("X");//subwf to F
                        no_lines += 2;
                        break;
                    case "AND":
                        line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;
                        line += t + "andwf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                        hexline += "05" + (Rd + 0xA0).ToString("X");//andwf to F                   
                        no_lines += 2;
                        break;
                    case "EOR":
                        line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;
                        line += t + "xorwf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                        hexline += "06" + (Rd + 0xA0).ToString("X");//xorwf to F
                        no_lines += 2;
                        break;
                    case "INC":
                        line += "incf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "0A" + (Rd + 0xA0).ToString("X");//incf to F
                        no_lines += 1;
                        break;
                    case "DEC":
                        line += "decf" + t + "R" + Rd.ToString() + ",1 " + nl;
                        hexline += "03" + (Rd + 0xA0).ToString("X");//decf to F
                        no_lines += 1;
                        break;

                    case "IN":
                        if (!out_Option)
                        {
                            //read from portB
                            line += "movf" + t + "PORTB,0" + nl;
                            line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                            hexline += "0806";//movf to W
                            hexline += "00" + (Rd + 0xA0).ToString("X");//movwf
                            no_lines += 2;
                        }
                        if (out_Option)
                        {
                            //read from PORT C

                            //movf PORTC,W
                            line += t + "movf" + t + "PORTC,0" + nl;
                            hexline += "0807";//movf to W

                            //andlw  0x3F
                            line += t + "andlw" + t + " 0x3F" + nl;
                            hexline += "393F";

                            //movwf Rd
                            line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                            hexline += "00" + (Rd + 0xA0).ToString("X");//movwf

                            // btfsc   porta,4
                            line += t + "btfsc PORTA,4" + nl;
                            hexline += "1A05";

                            // BSF  rd,6
                            line += t + "bsf" + t + "R" + Rd.ToString() + ",6" + nl;
                            hexline += "17" + (Rd + 0x20).ToString("X");

                            // btfsc   porta,5
                            line += t + "btfsc PORTA,5" + nl;
                            hexline += "1A85";

                            //BSF  RD,7
                            line += t + "bsf" + t + "R" + Rd.ToString() + ",7" + nl;
                            hexline += "17" + (Rd + 0xA0).ToString("X");

                            no_lines += 7;

                        }
                        break;

                    case "OUT":
                        if (!out_Option)
                        {
                            //tricky here cos bits 0-5 can go to port C, but b6/7 to Port A bits 4/5
                            //  movf    Rs,W
                            line += "movf" + t + "R" + Rs.ToString() + ",0" + nl;
                            hexline += "08" + (Rs + 0x20).ToString("X");//movf to W

                            //  movwf   PORTC   dont think this messes up serial...
                            line += t + "movwf" + t + "PORTC" + nl;
                            hexline += "0087";

                            //  movwf   temp1
                            line += t + "movwf" + t + "temp1" + nl;
                            hexline += "00AA";//temp1 is at 2A

                            //  rrf     temp1,1
                            line += t + "rrf" + t + "temp1" + nl;
                            hexline += "0CAA";

                            //  rrf     temp1,1    bit 7 - 5
                            line += t + "rrf" + t + "temp1" + nl;
                            hexline += "0CAA";

                            //  movlw   0x30
                            line += t + "movlw" + t + "0x30" + nl;
                            hexline += "3030";

                            //  andwf   temp1,0
                            line += t + "andwf" + t + "temp1,0" + nl;
                            hexline += "052A";

                            //  movwf   PORTA
                            line += t + "movwf" + t + "PORTA" + nl;
                            hexline += "0085";
                            no_lines += 8;
                        }

                        if (out_Option)
                        {

                            //re-program to move out to PORT B 
                            //  movf    Rs,W
                            line += t + "movf" + t + "R" + Rs.ToString() + ",0" + nl;
                            hexline += "08" + (Rs + 0x20).ToString("X");//movf to W

                            //  movwf   PORTB  
                            line += t + "movwf" + t + "PORTB" + nl;
                            hexline += "0086";
                            no_lines += 2;
                        }
                        break;

                    case "TRISQ":
                        if (!out_Option)
                        {
                            //tricky here cos bits 0-5 can go to port C, but b6/7 to Port A bits 4/5

                            line += "movf" + t + "R" + Rs.ToString() + ",0" + nl;
                            hexline += "08" + (Rs + 0x20).ToString("X");//movf to W

                            //  movwf   PORTC   dont think this messes up serial...
                            line += t + "movwf" + t + "PORTC" + nl;
                            hexline += "0087";

                            //  movwf   temp1
                            line += t + "movwf" + t + "temp1" + nl;
                            hexline += "00AA";//temp1 is at 2A

                            //  rrf     temp1,1
                            line += t + "rrf" + t + "temp1" + nl;
                            hexline += "0CAA";

                            //  rrf     temp1,1    bit 7 - 5
                            line += t + "rrf" + t + "temp1" + nl;
                            hexline += "0CAA";

                            //  movlw   0x30
                            line += t + "movlw" + t + "0x30" + nl;
                            hexline += "3030";

                            //  andwf   temp1,0
                            line += t + "andwf" + t + "temp1,0" + nl;
                            hexline += "052A";

                            //  movwf   PORTA
                            line += t + "movwf" + t + "PORTA" + nl;
                            hexline += "0085";
                            no_lines += 8;
                        }

                        if (out_Option)
                        {

                            // output on PORT B so only need to TRIS B   S0 has mask
                            //  movf    R0,W
                            line += t + "movf" + t + "R0,0" + nl;
                            hexline += "08" + (0x20).ToString("X");//movf to W
                            //bsf STATUS,RP0
                            line += t + "bsf" + t + "STATUS, RP0" + nl;//bank 1
                            hexline += "1683";
                            //movwf TRISB
                            line += t + "movwf" + t + "TRISB" + nl;//
                            hexline += "0086";
                            //bcf STATUS,RP0
                            line += t + "bcf" + t + "STATUS, RP0" + nl;//bank 1
                            hexline += "1283";

                            no_lines += 4;
                        }
                        break;

                    case "JP":
                        line += "goto" + t + data + nl;
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start + 0x800;
                            else
                            {
                                ErrorString = "Label " + data + " not found "; return false;
                            }
                        }


                        hexline += "2" + i.ToString("X");//goto... 
                        no_lines += 1;
                        break;
                    case "JZ":
                        line += "btfsc" + t + "STATUS,2" + nl;
                        line += t + "goto" + t + data + nl;
                        hexline += "1903";// status = 3
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start + 0x800;
                            else { ErrorString = "Label " + data + " not found "; return false; }
                        }
                        hexline += "2" + i.ToString("X");//goto... 
                        no_lines += 2;
                        break;
                    case "JNZ":
                        line += "btfss" + t + "STATUS,2" + nl;
                        line += t + "goto" + t + data + nl;
                        hexline += "1D03";// status = 3
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start + 0x800;
                            else { ErrorString = "Label " + data + " not found "; return false; }
                        }
                        hexline += "2" + i.ToString("X");//goto... 
                        no_lines += 2;
                        break;
                    case "JGT": //not really in the OCR set but included for the problem of flow diagrams gt
                        //after subwf Y,W   sub w from F
                        //  if W>Y then C=0 and jump is jump if  W>Y
                        //if w < Y: Status,Z = 0. Status,C = 1. wnew = Y-w. 
                        //if w == Y: Status,Z = 1. Status,C = 1. wnew = 0. 
                        //if Y < w: Status,Z = 0. Status,C = 0. wnew = Y-w + 0x100.

                        line += "btfss" + t + "STATUS,0" + nl;//carry flag
                        line += t + "goto" + t + data + nl;
                        hexline += "1C03";// status = 3 and bit =0
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start + 0x800;
                            else { ErrorString = "Label " + data + " not found "; return false; }
                        }
                        hexline += "2" + i.ToString("X");//goto... 
                        no_lines += 2;
                        break;


                    case "RCALL":
                        line += "call" + t + data + nl; i = 0;
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start;
                            else { ErrorString = "Label " + data + " not found "; return false; }
                        }
                        s1 = i.ToString("X");
                        while (s1.Length < 3) s1 = "0" + s1;
                        hexline += "2" + s1;//call... 
                        no_lines += 1;
                        break;
                    case "RET":
                        line += "return" + nl;
                        hexline += "0008";//ret. 
                        no_lines += 1;
                        break;
                    case "SHL":
                        //need to clear C first   and rlf/rrf  dont set zero flag!!
                        line += "bcf" + t + "STATUS,0" + nl;
                        line += t + "rlf" + t + "R" + Rd.ToString() + ",1" + nl;
                        line += t + "movf" + t + "R" + Rd.ToString() + ",1" + nl;   //move to itself to set Z
                        hexline += "1003";
                        hexline += "0D" + (Rd + 0xA0).ToString("X");
                        hexline += "08" + (Rd + 0xA0).ToString("X");
                        no_lines += 3;
                        break;
                    case "SHR":
                        //need to clear C first
                        line += "bcf" + t + "STATUS,0" + nl;
                        line += t + "rrf" + t + "R" + Rd.ToString() + ",1" + nl;
                        line += t + "movf" + t + "R" + Rd.ToString() + ",1" + nl;   //move to itself to set Z
                        hexline += "1003";
                        hexline += "0C" + (Rd + 0xA0).ToString("X");
                        hexline += "08" + (Rd + 0xA0).ToString("X");
                        no_lines += 3;
                        break;
                    case "BTS": // included for simple flow diagrams.... syntax is BTS   Q,n
                        //this uses 16F84A so PORTA for bits 0...3 and port B for bits 4...7
                        int bit_field = System.Convert.ToInt16(data);
                        if (bit_field < 4)
                        {
                            line += "bsf" + t + "PORTA," + data + nl;
                            bit1 = Convert.ToInt16(data);
                            bit2 = bit1 / 2;
                            if (bit1 % 2 == 0)
                            {
                                hexline += (bit2 + 0x14).ToString("X") + "05";//even
                            }
                            else
                            {
                                hexline += (bit2 + 0x14).ToString("X") + "85";
                            }
                        }
                        else 
                        {
                            line += "bsf" + t + "PORTB," + data + nl;
                            bit1 = Convert.ToInt16(data);
                            bit2 = bit1 / 2;
                            if (bit1 % 2 == 0)
                            {
                                hexline += (bit2 + 0x14).ToString("X") + "06";//even
                            }
                            else
                            {
                                hexline += (bit2 + 0x14).ToString("X") + "86";
                            }
                        }

                        no_lines += 1;
                        break;
                    case "BTC":// included for simple flow diagrams.... syntax is BTC   Q,n
                        //this uses 16F84A so PORTA
                        int bit_field1 = System.Convert.ToInt16(data);
                        if (bit_field1 < 4)
                        {
                            line += "bcf" + t + "PORTA," + data.ToString() + nl;
                            bit1 = Convert.ToInt16(data);
                            bit2 = bit1 / 2;
                            if (bit1 % 2 == 0)
                            {
                                hexline += (bit2 + 0x10).ToString("X") + "05";//even
                            }
                            else
                            {
                                hexline += (bit2 + 0x10).ToString("X") + "85";
                            }
                        }
                        else
                        {
                            line += "bcf" + t + "PORTB," + data.ToString() + nl;
                            bit1 = Convert.ToInt16(data);
                            bit2 = bit1 / 2;
                            if (bit1 % 2 == 0)
                            {
                                hexline += (bit2 + 0x10).ToString("X") + "06";//even
                            }
                            else
                            {
                                hexline += (bit2 + 0x10).ToString("X") + "86";
                            }
                        }
                        no_lines += 1;
                        break;

                    case "RLF": // added for Will 2105 as extension
                        //rlf/rrf  dont set zero flag!!
                        line += t + "rlf" + t + "R" + Rd.ToString() + ",1" + nl;
                        hexline += "0D" + (Rd + 0xA0).ToString("X");
                        no_lines += 1;
                        break;

                    case "RRF":
                        //rlf/rrf  dont set zero flag!!
                        line += t + "rrf" + t + "R" + Rd.ToString() + ",1" + nl;
                        hexline += "0C" + (Rd + 0xA0).ToString("X");
                        no_lines += 1;
                        break;

                    case "EQUB":
                        line += "db" + t + "0x00, 0x" + data + nl;
                        if (data.Length == 1) data = "0" + data;
                        hexline += "00" + data;
                        no_lines += 1;
                        break;
                    case "BYTE":
                        line += "db" + t + "0x00, 0x" + data + nl;
                        if (data.Length == 1) data = "0" + data;
                        hexline += "00" + data;
                        no_lines += 1;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                line += op + "\t" + input + nl;
                //so inline assembler..
                switch (op)
                {
                    //following have 8 bit data field
                    case "MOVLW":case "ADDLW":case "ANDLW":case "IORLW":case "RETLW":case "SUBLW":case "XORLW":
                        if (!CheckAsmData(ref data, 0xFF, ref Rd, ref Rs)) { line = data; return false; }
                        //cheeky   Rd has data field (ie litteral value or file reg or address and Rs has 1,0 value of d
                        break;
                    //following have 7 bit data  and d 
                    case "MOVF":case "ADDWF":case "ANDWF": case"CLRF":case "COMF":case "DECF":case "DECFSZ":case "INCF":
                    case "INCFSZ":case "MOVWF":case "RLF":case "RRF":case "SUBWF":case "SWAPF":case "XORWF":case "IORWF":
                        if (!CheckAsmData(ref data, 0x7F, ref Rd, ref Rs)) { line = data; return false; }
                        break;
                    //following have 11 bit data
                    case "CALL":case "GOTO":
                        break;
                    //followinf have 7 bits of data and a 3 bit bit field
                    case "BCF":case "BSF":case "BTFSS":case "BTFSC":
                        if (!CheckAsmData(ref data, 0x7F, ref Rd, ref Rs)) { line = data; return false; }
                        //Rs needs to have the bit field....
                        i = data.IndexOf(",");
                        if (i > 0)
                        {
                            try { Rs = System.Convert.ToInt16(data.Substring(i + 1)); }
                            catch { line = "Bit field wrong format "; return false; }
                            if ((Rs > 7) | (Rs < 0)) { line = "Bit field out of range "; return false; }
                        }
                        break;
                    //following have no data field!
                    case "CLRWDT":
                    case "RETURN":
                    case "SLEEP":
                    case "RETFIE":
                    case "CLRW":
                    case "NOP":
                    case "{":
                    case "}": 
                        break;
                    case "BANKSEL":
                        if (!CheckAsmData(ref data, 0x03, ref Rd, ref Rs)) { line = data; return false; }
                        break;

                    default:
                        ErrorString= "Unkown ASm code" + op; return false;
                }
                if (data.Length == 1) data = "0" + data;
                switch (op)
                {
                    //following have 8 bit data field
                    case "MOVLW":   hexline += "30" + data;no_lines++;break;
                    case "ADDLW":   hexline += "3E" + data;no_lines++;break;
                    case "ANDLW":   hexline += "39" + data;no_lines++;break;
                    case "IORLW":   hexline += "38" + data;no_lines++;break;
                    case "RETLW":   hexline += "34" + data;no_lines++;break;
                    case "SUBLW":   hexline += "3C" + data;no_lines++;break;
                    case "XORLW":   hexline += "3A" + data;no_lines++;break;
                    //following have 7 bit data  and d 
                    case "MOVF":    
                        Rd += Rs << 7; hexline += "08" + Rd.ToString("X");
                        no_lines++;
                        break;
                    case "ADDWF":   Rd += Rs << 7; hexline += "07" + Rd.ToString("X"); no_lines++; break;
                    case "ANDWF":   Rd += Rs << 7; hexline += "05" + Rd.ToString("X"); no_lines++; break;
                    case "CLRF":    Rd += 0x3 << 7; hexline+= "0"+ Rd.ToString("X"); no_lines++; break;//no d
                    case "COMF":    Rd += Rs << 7; hexline += "09" + Rd.ToString("X"); no_lines++; break;
                    case "DECF":    Rd += Rs << 7; hexline += "03" + Rd.ToString("X"); no_lines++; break;
                    case "DECFSZ":  Rd += Rs << 7; hexline += "0B" + Rd.ToString("X"); no_lines++; break;
                    case "INCF":    Rd += Rs << 7; hexline += "0A" + Rd.ToString("X"); no_lines++; break;
                    case "INCFSZ":  Rd += Rs << 7; hexline += "0F" + Rd.ToString("X"); no_lines++; break;
                    case "IORWF":   Rd += Rs << 7; hexline += "04" + Rd.ToString("X"); no_lines++; break;
                    case "MOVWF":   Rd += 0x01 << 7; hexline += "00" + Rd.ToString("X"); no_lines++; break;//no d
                    case "RLF":     Rd += Rs << 7; hexline += "0D" + Rd.ToString("X"); no_lines++; break;
                    case "RRF":     Rd += Rs << 7; hexline += "0C" + Rd.ToString("X"); no_lines++; break;
                    case "SUBWF":   Rd += Rs << 7; hexline += "02" + Rd.ToString("X"); no_lines++; break;
                    case "SWAPF":   Rd += Rs << 7; hexline += "0E" + Rd.ToString("X"); no_lines++; break;
                    case "XORWF":   Rd += Rs << 7; hexline += "06" + Rd.ToString("X"); no_lines++; break;
                    //following have 11 bit data
                    case "CALL":
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start;
                            else
                            {
                                //allow to be a value?
                                if (!CheckAsmData(ref data, 0x7ff, ref Rd, ref Rs))
                                {
                                    ErrorString = "Label " + data + " not found ";
                                    return false;
                                }
                                i = Rd;
                            }
                        }
                        s1 = i.ToString("X");
                        while (s1.Length < 3) s1 = "0" + s1;
                        hexline += "2" + s1;//call... 
                        no_lines += 1;
                        break;
                    case "GOTO": 
                        if (ResolveLabels)
                        {
                            if (FindLabel_asm(data, ref i)) i += Code_start;
                            else 
                            {
                                //allow to be a value?
                                if (!CheckAsmData(ref data, 0x7ff, ref Rd, ref Rs))
                                {
                                    ErrorString = "Label " + data + " not found ";
                                    return false;
                                }
                                i = Rd;
                            }
                        }
                        i += 5 << 11;
                        s1 = i.ToString("X");
                        hexline += s1;
                        no_lines++;
                        break;
                    //followinf have 7 bits of data and a 3 bit bit field
                        //b is in Rs
                    case "BCF": 
                        Rd += Rs << 7;data=Rd.ToString("X");
                        while (data.Length < 3) data="0"+data;
                        hexline += "1" + data; no_lines++; 
                        break;
                    case "BSF":
                        Rs = Rs + 8;
                        Rd += Rs << 7;data=Rd.ToString("X");
                        hexline += "1" + data; no_lines++; 
                        break;
                    case "BTFSS": 
                        Rs = Rs + 24;
                        Rd += Rs << 7;data=Rd.ToString("X");
                        hexline += "1" + data; no_lines++; 
                        break;

                    case "BTFSC":
                        Rs = Rs + 16;
                        Rd += Rs << 7;data=Rd.ToString("X");
                        hexline += "1" + data; no_lines++; 
                        break;
                    //following have no data field!
                    case "CLRWDT":  hexline += "0064"; no_lines++; break;
                    case "RETURN":  hexline += "0008"; no_lines++; break;
                    case "SLEEP":   hexline += "0063"; no_lines++; break;
                    case "RETFIE":  hexline += "0009"; no_lines++; break;
                    case "CLRW":    hexline += "0100"; no_lines++; break;
                    case "NOP":     hexline += "0000"; no_lines++; break;
                    case "{":case "}":
                        break;
                    case "BANKSEL":
                        switch (Rd)
                        {
                            case 0:
                                //bcf STATUS,5  bcf STATUS,6 status=3
                                //bcf 03,5   bcf 03,6
                                hexline += "1213"; no_lines++;
                                hexline += "1303"; no_lines++;
                                break;
                            case 1:
                                //bsf 03,5   bcf 03,6
                                hexline += "1613"; no_lines++;
                                hexline += "1303"; no_lines++;
                                break;
                            case 2:
                                //bcf 03,5   bsf 03,6
                                hexline += "1213"; no_lines++;
                                hexline += "1713"; no_lines++;
                                break;
                            case 3:
                                //bsf 03,5   bsf 03,6
                                hexline += "1613"; no_lines++;
                                hexline += "1713"; no_lines++;
                                break;
                        }
                        break;
                    default:
                        ErrorString = "Unkown ASm code" + op; return false; break;
                }
            }
            return true;
        }
        protected bool FindLabel_asm(string s,ref int value)
        {
            if (InLineAssembler)
            {
                if (s == "STATUS") {value = 3; return true; }
                if ((s == "PORTA")|(s=="TRISA")) { value = 5; return true; }
                if ((s == "PORTB") |(s=="TRISB")){ value = 6; return true; }
                if ((s == "PORTC") | (s == "TRISC")) { value = 7; return true; }
            }
            for (int i = 0; i < maxlabels; i++)
            {
                if (labels[i] == s)
                { value = labelvalues_code[i]; return true; }
            }
            value = -1;
            return false;
        }
        protected bool GetRegister(ref string s, ref int r)
        {
            if (!s.StartsWith("S"))
            {
                s = "Register must be in format Sn";
                return false;
            }
            try
            {
                //fro tris try to parse 2 digits
                r = System.Convert.ToInt32(s.Substring(1, 1));
                try
                {
                    int temp = System.Convert.ToInt32(s.Substring(2, 1));
                    r = r * 10 + temp;
                }
                catch
                {
                }
            }
            catch (Exception e)
            {
                s = "Invalid Register number " + s[1]; return false;
            }
            if ((r < 0) || (r > 200))
            {
                s = "Invalid Register number " + s[1]; return false;
            }
            return true;
        }
        protected bool CheckAsmData(ref string s, int MaxValue, ref int value,ref int d)
        {
            //so s has data field...  need to check range...
            // data field might contain ,1... eg MOVF A0,1
            int i = s.IndexOf(",");
            string s1 = s; d = 1;//ie d=1 store in F reg
            if (i > 0)
            {
                s1 = s.Substring(i + 1).Trim();
                
                try
                {
                    d = System.Convert.ToInt16(s1);//might be 0,1  ..,2...7  for bcf?
                }
                catch
                {
                }
                if (s1 == "W") d = 0;//ie store in W
                if (s1 == "F") d = 1;//ie store in F

                if(s1=="0")d=0;//ie store in W
                if (s1== "W") d = 0;//ie store in W
                //not sure about bcf etc....
                s1 = s.Substring(0, i);
            }
            
            try
            {
                value = System.Convert.ToInt32(s1,16);
            }
            catch
            {
                s = "Data value wrong format";return false;
            }
            if (value > MaxValue) { s = "Data value too large"; return false; }

            return true;
        }
        public bool DecodeLine(ref string line, ref string label, ref string op, ref int Rs, ref int Rd, ref string data)
        {
            int i = 0;
            line = line.ToUpper(); label = "";
            if (line == "{")
            {
                InLineAssembler = true;
                op = "{"; return true;//done!
            }
            if (line == "}")
            {
                InLineAssembler = false;
                op = "}"; return true;
            }
            //if (InLineAssembler) { op = line; return true; }//done!
            int n = 0, k = 0;
            char[] c1 = new char[2]; c1[0] = (char)0x09; c1[1] = ' ';
            //strip comments
            n = line.IndexOf(";");
            if (n != -1) { line = line.Substring(0, n); }
            n = line.IndexOfAny(c1);
            if (line.Length == 0)
            {
                //comment only
                op = ""; return true;//done!
            }
            if (n != 0)
            {
                //we have a label
                k = line.IndexOf(":");
                if (k < 1)
                {
                    //no colon so..
                    line = "Syntax Error:  labels must end with :"; return false;
                }
                label = line.Substring(0, k);
                line = line.Substring(k + 1);
            }
            line = line.Trim(c1);// strip the separators

            //now have opcode.....
            n = line.IndexOfAny(c1);//end of opcode
            if (n == -1) op = line;
            else
            {
                op = line.Substring(0, n);
                line = line.Substring(n + 1);
                line = line.Trim(c1);
                data = line;
            }
            if (InLineAssembler)
            {
                //Asm codes...
                switch (op)
                {
                    //followinf have 8 bit data field
                    case "MOVLW":case "ADDLW":case "ANDLW":case "IORLW":case "RETLW":case "SUBLW":case "XORLW":
                        if (!CheckAsmData(ref data, 0xFF, ref Rd, ref Rs)) { line = data; return false; }
                        //cheeky   Rd has data field (ie litteral value or file reg or address and Rs has 1,0 value of d
                        break;
                    //following have 7 bit data  and d 
                    case "MOVF":case "ADDWF":case "ANDWF": case "COMF": case "DECF": case "DECFSZ": case "INCF":
                    case "INCFSZ": case "IORWF": case "RLF": case "RRF": case "SUBWF":
                    case "SWAPF":case "XORWF":
                    case "MOVWF":
                    case "CLRF":
                        if (!CheckAsmData(ref data, 0x7F, ref Rd, ref Rs)) { line = data; return false; }
                        break;
                    //following have 11 bit data
                    case "CALL":case "GOTO":
                        break;
                        //followinf have 7 bits of data and a 3 bit bit field
                    case "BCF": case "BSF": case "BTFSS": case "BTFSC":
                        if (!CheckAsmData(ref data, 0x7F, ref Rd, ref Rs)) { line = data; return false; }
                        //Rs needs to have the bit field....
                    i = data.IndexOf(",");
                    if (i > 0)
                    {
                        try{Rs = System.Convert.ToInt16(data.Substring(i+1));}
                        catch {line = "Bit field wrong format ";return false;}
                        if ((Rs > 7) | (Rs < 0)) { line = "Bit field out of range "; return false; }
                    }
                        break;
                    //following have no data field!
                    case "CLRWDT":case "RETURN":case "SLEEP":case "RETFIE":case "CLRW":case "NOP":
                    
                        break;
                    //following has value from 0,1,2,3
                    case "BANKSEL":
                        if (!CheckAsmData(ref data, 0x03, ref Rd, ref Rs)) { line = data; return false; }
                        break;

                    default:
                        line = "Unkown ASm code" + op; return false; break;
                }
            }
            else
            {
                // OCR codes
                switch (op)
                {
                    //inline litterals ... data needs to be 8bits


                    case "":
                        break;
                    case "MOVI":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        break;
                    case "MOV":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "ADD":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "SUB":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "AND":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "EOR":
                        //todo... how to exor
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "INC":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "DEC":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "IN":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "OUT":
                        data = data.Substring(data.IndexOf(",") + 1);
                        if (!GetRegister(ref data, ref Rs)) { line = data; return false; }
                        data = "";
                        break;
                    case "JP":
                        break;
                    case "JZ":
                        break;
                    case "JNZ":
                        break;
                    case "JGT": //note note real.. added for flow diagrams
                        break;
                    case "RCALL":
                        break;
                    case "RET":
                        break;
                    case "SHL":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "SHR":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "RLF":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;
                    case "RRF":
                        if (!GetRegister(ref data, ref Rd)) { line = data; return false; }
                        data = "";
                        break;

                    case "BTS": // included for simple flow diagrams.... syntax is BTS   Q,n
                        data = data.Substring(data.IndexOf(",") + 1, 1);// should be the bit number...
                        break;
                    case "BTC":
                        data = data.Substring(data.IndexOf(",") + 1, 1);// should be the bit number...
                        break;

                    case "NOP":
                        break;

                    //emulator only
                    case "EQUB": break;
                    case "BYTE": break;
                    case "BREAK": break;
                    case "TRISQ": break;//added as ext to tristate output port,,,S0 has bit mask  0 for out


                    default:
                        line = "Invalid Op code" + op; return false;
                }
            }
            return true;
        }
        #endregion

        public string IntelCode()
        {
            //going to generate the Intel hex code....for 16F84A  and simple model
            string s = ""; string s1 = ""; string s2 = "";
            string nl = System.Environment.NewLine;
            s = ":020000040000FA"+nl;//set ext address as 0000
            s += ":100000003028132817282028AA000308AB000B1D4E" + nl;
            s += ":100010000D2801308506320881000B112B08830062" + nl;
            s += ":10002000AA0E2A0E0900AC01AC0B142808000430FB" + nl;
            s += ":10003000B100AD011320AD0B1A28B10B1A2808002E" + nl;
            s += ":100040002008B2008100A0308B000030AE001320E9" + nl;
            s += ":10005000AE0B2728A10B2528051000308B000800C7" + nl;
            s += ":1000600000308B00640083160330810083128316F6" + nl;
            //s += ":0C007000FF308600E0308500831285011F" + nl;///used to have ra4 as op
            s += ":0C0070000F308600E0308500831285010F" + nl;// now has RA0..3 and RB4-7 as outputs

            //bytes in code 7 x 16 +12 = 007C  (ie (word)address 3E)so code starts at word 3E
            //going to write hexdata
            //next address is 007C
            int add1 = 0x7C;

            for (int j = 0; j < max_hexlines; j++)
            {
                s1=add1.ToString("X");while(s1.Length<4)s1="0"+s1;
                //no bytes in hexcode[j] is length/2
                //need to swop bytes  in hexword...
                s2="";
                for (int k = 0; k < hexcode[j].Length; k += 4)
                {
                    s2 += hexcode[j].Substring(k + 2, 2) + hexcode[j].Substring(k, 2);
                }
                s1 = ":0"+(hexcode[j].Length/2).ToString("X") + s1 + "00" + s2;
                s += s1+IntelChecksum(s1.Substring(1))+nl;
                add1 += (hexcode[j].Length / 2);
            }

            s += ":02400E00FB3F76"+nl;// config word to add 400E
            s += ":00000001FF" + nl;//end of file

            return s;
        }

        public string IntelChecksum(string s)
        {
            string s1 = "";
            int sum = 0; int i = 0;
            while (s.Length > 1)
            {
                s1 = s.Substring(0, 2);
                i = Convert.ToInt16(s1, 16); sum += i;
                if(s.Length>1)s = s.Substring(2);
            }
            sum = sum % 256;
            i = 0x100 - sum;
            if (i < 0x10) return "0" + i.ToString("X"); else return i.ToString("X");
        }

        public class OCRmodel
        {
            public string[] S = new string[8];
            public string InputPort;
            public string OutputPort;
            public string Z;
            public string W;
            public string C;//only used I think in break window...
        }
    }
}
