using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCR_Assembler
{
    public class OCRAssembler
    {
        static int MAX_LINES_ASM = 400;
        string[] OCRCode = new string[MAX_LINES_ASM];   // this is for OCR source
        int max_n = 0;
        public int Hex_Output_start = 0x500;
        public string[] hexcode = new string[MAX_LINES_ASM];

        public int asmcodelines = 0;                // to remember how many lines of PIC code
        public string[] labels = new string[MAX_LINES_ASM];
        public int[] labelvalues_code = new int[MAX_LINES_ASM];
        public int maxcodelines = 0;
        public int maxlabels = 0;
        public int table = 0;

        protected bool GenerateMC(ref string error_message)
        {
            //first run through to get the labels...
            string s = "";//get text
            string s2 = ""; int asm_no = 0; string s3 = "";
            string line = ""; string s1 = ""; int n = 0;
            string label = ""; int Rd = 0; int Rs = 0; string data = ""; string op = "";

            asmcodelines = 0;
            maxlabels = 0;
            maxcodelines = 0;

            labels[maxlabels] = "WAIT1MS";
            labelvalues_code[maxlabels] = 0x300 - Hex_Output_start;
            maxlabels++;

            labels[maxlabels] = "READADC";
            labelvalues_code[maxlabels] = 0x301 - Hex_Output_start;
            maxlabels++;



            for (int i = 0; i < max_n; i++)
            {
                line = OCRCode[i];
                label = ""; op = "";
                s1 = line;
                if (!DecodeLine(ref s1, ref label, ref op, ref Rs, ref Rd, ref data))
                {//we have an error...
                    error_message = line + "at line " + n.ToString() + "   :" + line; return false;
                }
                s1 = line; asm_no = 0;
                ProcessLine_ToAssembler(s1, ref s2, ref s3, ref asm_no, false);
                if (label == "TABLE")
                {
                    table = maxcodelines;
                }
                if (label != "")
                {
                    labels[maxlabels] = label;
                    labelvalues_code[maxlabels] = asmcodelines;
                    maxlabels++;
                }
                if ((op == "BYTE") || (op == "EQUB"))//emulator only
                {
                    op = "";
                }
                if (op != "")
                {
                    asmcodelines += asm_no;
                }
                if (op == "RCALL")
                {
                    op = op;
                }
                n++;
            }



            int no_linescode = 0;
            string outputASM = "";
            string outputHEX = "";
            n = 0;

            for (int i = 0; i < max_n; i++)
            {
                line = OCRCode[i];
                outputHEX = ""; outputASM = "";
                if (!ProcessLine_ToAssembler(line, ref outputASM, ref outputHEX, ref no_linescode, true))
                {//we have an error...
                    error_message= line + "at line " + n.ToString() + "   :" + line; return false;
                }
                hexcode[n] = outputHEX;
                n++;
            }
            return true;
        }
        protected bool ProcessLine_ToAssembler(string input, ref string line, ref string hexline, ref int no_lines, bool ResolveLabels)
        {
            string nl = Environment.NewLine;
            char t1 = (char)0x09; string t = ""; t += t1;
            line = line.ToUpper();
            string label = ""; int i = 0;
            string op = "";
            string data = ""; int Rs = 0; int Rd = 0;
            if (!DecodeLine(ref input, ref label, ref op, ref Rs, ref Rd, ref data))
            {
                return false;
            }
            line = "";
            if (op == "BLANK") return true;
            if (op == "") return true;
            if (label == "TABLE")
            { line += t + "ORG 0x400" + t + nl + label + nl + t; }
            else { line += label + (char)0x09; }



            switch (op)
            {
                case "":
                    line += "nop" + nl;
                    hexline += 0000; no_lines += 1;
                    break;
                case "MOVI":
                    line += "movlw " + t + "0x" + data + nl;
                    line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                    if (data.Length == 1) data = "0" + data;
                    hexline += "30" + data;// movlw
                    hexline += "00" + (Rd + 0xA0).ToString("X");//movwf  (S0 is stored at 0x020 in PIC memory
                    no_lines += 2;
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
                    hexline += "05" + (Rd + 0xA0).ToString("X");//subwf to F                   
                    no_lines += 2;
                    break;
                case "EOR":
                    line += "movf" + t + "R" + Rs.ToString() + ",0 " + nl;
                    line += t + "xorwf" + t + "R" + Rd.ToString() + ",1 " + nl;
                    hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                    hexline += "06" + (Rd + 0xA0).ToString("X");//subwf to F
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
                    //read from portB
                    line += "movf" + t + "PORTB,0" + nl;
                    line += t + "movwf" + t + "R" + Rd.ToString() + nl;
                    hexline += "0806";//movf to W
                    hexline += "00" + (Rd + 0xA0).ToString("X");//movwf
                    no_lines += 2;

                    break;
                case "OUT":
                    line += "movf" + t + "R" + Rs.ToString() + ",0" + nl;
                    line += t + "movwf" + t + "PORTC" + nl;
                    //tricky here cos bits 0-5 can go to port C, but b6/7 to Port A bits 4/5

                    //asm code....
                    //  movf    Rs,W
                    hexline += "08" + (Rs + 0x20).ToString("X");//movf to W
                    //  movwf   PORTC   dont think this messes up serial...
                    hexline += "0087";
                    //  movwf   temp
                    hexline += "00AA";//temp1 is at 2A
                    //  rrf     temp1,1
                    hexline += "0CAA";
                    //  rrf     temp1,1    bit 7 - 5
                    hexline += "0CAA";
                    //  movlw   0x30
                    hexline += "3030";
                    //  andwf   temp1,0
                    hexline += "052A";
                    //  movwf   PORTA
                    hexline += "0085";
                    no_lines += 8;

                    break;
                case "JP":
                    line += "goto" + t + data + nl;
                    i = 0x800;
                    if (ResolveLabels) i += FindLabel_asm(data) + Hex_Output_start;
                    hexline += "2" + i.ToString("X");//goto... 
                    no_lines += 1;
                    break;
                case "JZ":
                    line += "btfsc" + t + "STATUS,2" + nl;
                    line += t + "goto" + t + data + nl;
                    hexline += "1903";// status = 3
                    i = 0x800;
                    if (ResolveLabels) i += FindLabel_asm(data) + Hex_Output_start;
                    hexline += "2" + i.ToString("X");//goto... 
                    no_lines += 2;
                    break;
                case "JNZ":
                    line += "btfss" + t + "STATUS,2" + nl;
                    line += t + "goto" + t + data + nl;
                    hexline += "1D03";// status = 3
                    i = 0x800;
                    if (ResolveLabels) i += FindLabel_asm(data) + Hex_Output_start;
                    hexline += "2" + i.ToString("X");//goto... 
                    no_lines += 2;
                    break;
                case "RCALL":
                    line += "call" + t + data + nl; i = 0;
                    if (ResolveLabels) i += FindLabel_asm(data) + Hex_Output_start;

                    hexline += "2" + i.ToString("X");//goto... 
                    no_lines += 1;
                    break;
                case "RET":
                    line += "return" + nl;
                    hexline += "0008";//ret. 
                    no_lines += 1;
                    break;
                case "SHL":
                    //need to clear C first
                    line += "bcf" + t + "STATUS,0" + nl;
                    line += t + "rlf" + t + "R" + Rd.ToString() + ",1" + nl;
                    hexline += "1003";
                    hexline += "0D" + (Rs + 0xA0).ToString("X");
                    no_lines += 2;
                    break;
                case "SHR":
                    //need to clear C first
                    line += "bcf" + t + "STATUS,0" + nl;
                    line += t + "rrf" + t + "R" + Rd.ToString() + ",1" + nl;
                    hexline += "1003";
                    hexline += "0C" + (Rs + 0xA0).ToString("X");
                    no_lines += 2;
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
            return true;
        }
        protected int FindLabel_asm(string s)
        {
            for (int i = 0; i < maxlabels; i++)
            {
                if (labels[i] == s) return labelvalues_code[i];
            }
            //error_message = line + "at line " + n.ToString() + "   :" + line;
            //MessageBox.Show("Error! Label " + s + " not found!");
            return -1;
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
            if ((r < 0) || (r > 20))
            {
                s = "Invalid Register number " + s[1]; return false;
            }
            return true;
        }
        protected bool DecodeLine(ref string line, ref string label, ref string op, ref int Rs, ref int Rd, ref string data)
        {
            line = line.ToUpper();
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

            switch (op)
            {
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
                //emulator only
                case "EQUB": break;
                case "BYTE": break;


                default:
                    line = "Invalid Op code" + op; return false; break;
            }
            return true;
        }



    }

    public class Programmer
    {
        public string[] hexcode = new string[255];//has 16F876 opcodes for OCR instructions
        public int max_lines = 0;
        public string[] PicCode = new string[255];//has 16F876 data read back
        public int[] BreakJumps = new int[255];
        public string source = "";
        public string[] source_1 = new string[255];//has ocr insrtuctions
        public int Code_start = 0x500;
        public int Code_break = 0x4f0;
        public bool breakset = false;
        public System.IO.Ports.SerialPort serialPort1;

        private int CountWordsinCode()
        {
            int no_words = 0;
            for (int i = 0; i < max_lines; i++)
            {
                no_words += hexcode[i].Length / 4;
            }
            return no_words;
        }
        private bool ProgramQuadWord(int address, string data)
        {
            //this is really for 16F876A which has to program 4 words (8 bytes) at a time..
            bool success = false; string s = "";
            s = SendMessage("@@@@w0" + address.ToString("X") + data);
            s = SendMessage("@@@@Z");//little delay...
            s = SendMessage("@@@@r0" + address.ToString("X"));
            if (s.Length > 15) if (s.Substring(0, 16) == data) success = true;
            if (!success)
            {
                s = s;
            }
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

        private void ReadCode()
        {
            string s = "";
            int n_words = CountWordsinCode();

            int add = Code_start; int n = n_words; s = "";
            //going to use block read (r) for speed 
            while (n > 0)
            {
                s += SendMessage("@@@@r0" + add.ToString("X"));
                n -= 16; add += 16;
               
            }
            for (int i = 0; i < max_lines; i++)
            {
                PicCode[i] = s.Substring(0, hexcode[i].Length);
                s = s.Substring(hexcode[i].Length, s.Length - hexcode[i].Length);
            }
        }


        private string SendMessage(string s)
        {
            byte[] byte1 = new byte[2];
            byte1[0] = 0x0d; byte1[1] = 0;
            serialPort1.ReadExisting();
            serialPort1.Write(s); serialPort1.Write(byte1, 0, 2);

            return WaitMessage();
        }
        private string WaitMessage()
        {
            int x = 0;
            string r = "";
            serialPort1.ReadTimeout = 1000; x = 0;
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


        private bool Program876()
        {
            int add = Code_start; string s = ""; string s1 = "";

            for (int i = 0; i < max_lines; i++)
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
            }
            return true;
        }

        private bool Program876A()
        {
            int add = Code_start; string s, s1, s2, s3;

            int n1 = 0; s2 = ""; s3 = "";
            for (int i = 0; i < max_lines; i++)
            {
                s2 += hexcode[i]; s3 += PicCode[i]; n1 += hexcode[i].Length;
            }

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
                add += 4;
                s3 = s3.Substring(16); s2 = s2.Substring(16);

            }
            return true;
        }

        private bool Program886()
        {
            int add = Code_start; string s, s1, s2, s3;

            int n1 = 0; s2 = ""; s3 = "";
            for (int i = 0; i < max_lines; i++)
            {
                s2 += hexcode[i]; s3 += PicCode[i]; n1 += hexcode[i].Length;
            }

            //need 16 bytes at a time...so 32 chars
            //need s2 and s3 to be a multiple of 32.....
            while (n1 % 64 != 0) { s2 += "0"; s3 += "0"; n1++; }

            while (s2.Length > 63)
            {
                s = s2.Substring(0, 64); s1 = s3.Substring(0, 64);
                if (s != s1)
                {
                    if (!Program16Word(add, s)) { return false; }
                }
                add += 16;

                s3 = s3.Substring(64); s2 = s2.Substring(64);
            }
            return true;
        }


        private bool ProgramPIC()
        {
            ReadCode();//get a copy of the current pic code....

            string pic_type = SendMessage("@@@@X");

            if (pic_type == "16F876") return Program876();
            if (pic_type == "16F876A") return Program876A();
            if (pic_type == "16F886") return Program886();

            return false;
        }
    }
}
