using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlowDiagrams
{
    public enum ProcessType { unknown,movi, mov, addi, subi, wait, and, orr, xor, add, sub }

    [Serializable]
    public class FlowObject :Control, ISerializable 
    {
        public Guid id;
        public int Line_Number;//only assigned on parse...
        public bool Parsed;
        public string text = "";
        public bool Selected = false;
        public string Comment = "";
        public float scale=100f;

        public string[] Asmcode = new string[255];
        public int n_asm = 0;

        //a control that enables drags
        private Rectangle dragBoxFromMouseDown;
        public Point screenOffset;
        public Point start_drag;
        //defaults
        public int x_position=200;
        public int y_position=100;
        public int width=100;
        public int height=40;

        public FlowObject FlowOut;
        public FlowObject FlowoutSide;

        public delegate void Click_Select(FlowObject control);
        public event Click_Select Box_Clicked;

        public delegate void Click_Right(FlowObject control,MouseEventArgs e);
        public event Click_Right Box_RightClicked;

        public FlowObject()
        {
            id = Guid.NewGuid();
            Initialize();
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id",this.id);
            info.AddValue("Line_Number",this.Line_Number);
            info.AddValue("text", this.text);
            info.AddValue("Asmcode", this.Asmcode);
            info.AddValue("n_asm", this.n_asm);
            info.AddValue("x_position", this.x_position);
            info.AddValue("y_position", this.y_position);
            info.AddValue("width", this.width);
            info.AddValue("height", this.height);
            info.AddValue("FlowOut", FlowOut);
            info.AddValue("FlowOutSide", FlowoutSide);
            info.AddValue("Comment", Comment);
        }

        public FlowObject(SerializationInfo info, StreamingContext context)
        {
            Deserialise(info, context);
        }

        protected void Deserialise(SerializationInfo info, StreamingContext context)
        {
            id = (Guid)info.GetValue("id", typeof(Guid));
            Line_Number = (int)info.GetValue("Line_Number", typeof(int));
            text = (string)info.GetValue("text", typeof(string));
            Asmcode = (string[])info.GetValue("Asmcode", typeof(string[]));
            n_asm = (int)info.GetValue("n_asm", typeof(int));
            x_position = (int)info.GetValue("x_position", typeof(int));
            y_position = (int)info.GetValue("y_position", typeof(int));
            width = (int)info.GetValue("width", typeof(int));
            height = (int)info.GetValue("height", typeof(int));
            Location = new Point(x_position - width / 2, y_position);
            Name = id.ToString();
            FlowOut = null; FlowoutSide = null;
            Width = width; Height = height;
            FlowOut = (FlowObject)info.GetValue("FlowOut", typeof(FlowObject));
            FlowoutSide = (FlowObject)info.GetValue("FlowOutSide", typeof(FlowObject));
            Comment = (string)info.GetString("Comment");
        }


        #endregion

        private void Initialize()
        {
            Location = new Point(x_position - width / 2, y_position);
            Name = id.ToString();
            FlowOut = null; FlowoutSide = null;
            Width = width; Height = height;

            AllowDrop = true;
            DragDrop += new DragEventHandler(FlowObject_DragDrop);
            DragEnter += new DragEventHandler(FlowObject_DragEnter);

            MouseDown += new MouseEventHandler(FlowObject_MouseDown);
            MouseUp += new MouseEventHandler(FlowObject_MouseUp);
            MouseMove += new MouseEventHandler(FlowObject_MouseMove);
            MouseDoubleClick += new MouseEventHandler(FlowObject_MouseDoubleClick);
            MouseClick += new MouseEventHandler(FlowObject_MouseClick);
        }

        void FlowObject_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        void FlowObject_DragDrop(object sender, DragEventArgs e)
        {
            //so link it....
            if (e.Effect == DragDropEffects.Link)
            {
                DragData dd = (DragData)e.Data.GetData(typeof(DragData));
                string item = dd.Name;
                int flowout = dd.Flowout;
                FlowObject c = (FlowObject)this.Parent.Controls[this.Parent.Controls.IndexOfKey(item)];
                if (dd.Flowout == 1) c.FlowoutSide = this;
                else  c.FlowOut = this;
                this.Parent.Invalidate();
            }
        }

        void FlowObject_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Box_RightClicked != null) Box_RightClicked(this, e);
            }
        }

        void FlowObject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Box_Clicked != null) Box_Clicked(this);
            }
        }


        public void Draw(Graphics g, Rectangle r)
        {
            //have to move the drawing to teh right place.... bodge..
            //g.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, this.x_position - this.width/2, this.y_position);
            PaintEventArgs e1 = new PaintEventArgs(g, r);
            OnPaint(e1);
        }

        protected override void  OnPaint(PaintEventArgs e)
        {
            e.Graphics.ScaleTransform(scale/100,scale/100);
            base.OnPaint(e);
        }

        public bool LinecutsBoxVertical(int x, int y1, int y2)
        {
            int x1 = x_position - width / 2 - 10;
            int x2 = x_position + width / 2 + 10;
            if ((x > x1) && (x < x2) && (y_position < y2) && (y_position+ height > y1)) return true; 
            return false;
        }


        public bool LinecutsBoxHoriontal(int y, int x1, int x2)
        {
            int y1 = y_position - (height / 2) - 10;
            int y2 = y_position + (height / 2) + 10;
            if ((y > y1) && (y < y2) && (x_position < x2) && (x_position > x1)) return true; 
            return false;
        }
        

        void FlowObject_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEffect = DoDragDrop(this.Name, DragDropEffects.All | DragDropEffects.Move);
                }
            }

            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    if (this.GetType() != typeof(EndBox))//don't link from endbox
                    {
                        screenOffset = SystemInformation.WorkingArea.Location;
                        DragData d1 = new DragData(); d1.Name = this.Name; d1.Flowout = 0;
                        //if we are a decision box we need to decide if bottom or left...
                        if (this.GetType() == typeof(DecisionBox))
                        {
                            //side if x if over 80% of way across
                            if ((e.X > width / 2) && (e.Y < 3 * height / 4))
                            {
                                d1.Flowout = 1;
                            }
                        }
                        if (this.GetType() == typeof(SimpleDecisionBox))
                        {
                            //side if x if over 80% of way across
                            if ((e.X > width / 2) && (e.Y < 3 * height / 4))
                            {
                                d1.Flowout = 1;
                            }
                        }
                        DragDropEffects dropEffect = DoDragDrop(d1, DragDropEffects.All | DragDropEffects.Link);
                    }
                }
            }
        }

        public class DragData
        {
            public string Name = "";
            public int Flowout = 0;
        }

        void FlowObject_MouseUp(object sender, MouseEventArgs e)
        {
            dragBoxFromMouseDown = Rectangle.Empty;
        }

        void FlowObject_MouseDown(object sender, MouseEventArgs e)
        {
            Size dz = SystemInformation.DragSize;
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dz.Width / 2),
                                                               e.Y - (dz.Height / 2)), dz);
            start_drag = new Point(e.X, e.Y);
        }




    }

    public class FlowDiagramConnectors
    {
        //just to remember where we have drawn them....
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public FlowDiagramConnectors(int X1, int Y1, int X2, int Y2)
        {
            //need to order them...
            if (X1 == X2)
            {
                if (Y1 < Y2) { y1 = Y1; y2 = Y2; x1 = X1; x2 = X2; } else { y1 = Y2; y2 = Y1; x1 = X2; x2 = X1; }
            }
            else
            {
                if (X1 < X2) { y1 = Y1; y2 = Y2; x1 = X1; x2 = X2; } else { y1 = Y2; y2 = Y1; x1 = X2; x2 = X1; }
            }     
        }
    }

    [Serializable]
    public class FlowDiagram  :ISerializable

    {
        public System.Collections.Generic.List<FlowObject> BoxesList = new List<FlowObject>();
        [NonSerialized]public System.Collections.Generic.List<FlowDiagramConnectors> ConnectorList= new List<FlowDiagramConnectors>();
        public float scale=100f;
       
        
        public StartBox start = new StartBox();
        public FlowDiagram()
        {
            BoxesList.Add(start);
        }


        private int FindFreeHorizontal(int y, int x1, int x2, int incy)
        {
            //going to try to find free horizontal from x1 to x2 starting from y
            bool found = false; int x = 0;
            //order it so that x1<x2
            if (x1 > x2) { x = x1; x1 = x2; x2 = x; }; 
            /*
            while (!found)
            {
                foreach (FlowDiagramConnectors c in ConnectorList)
                {
                    found = true;
                    if ((c.y1 == y) && (c.y2 == y))
                    {
                        //on our horizontal  we know x1 is least
                        //need to check if c.x1 or c.x2 are least
                        if (c.x1 > c.x2)
                        {
 
                            return y;
                        }
                        if (!((x2 <= c.x1) || (x1 >= c.x2)))
                        {
                            //overlap...  so move a bit??
                            found = false;
                            y += incy;
                            break;
                        }
                    }
                }
            }
             * */
            //now boxes....
            found = false;
            while (!found)
            {
                found = true;
                foreach (FlowObject f1 in BoxesList)
                {
                    if (f1.LinecutsBoxHoriontal(y, x1, x2))
                    {
                        //so need to detour arround this one....
                        y += incy;
                        f1.Selected = true;
                        found = false;
                        break;
                    }
                }
            }

            return y;
        }
        private int FindFreeVertical(int x, int y1, int y2, int incx)
        {
            //going to try to find free vertical from y1 to y2 starting from x
            bool found = false; int y = 0;
            //order it so that y1<y2
            if (y1 > y2) { y = y1; y1 = y2; y2 = y; };
            while (!found)
            {
                found = true;
                foreach (FlowDiagramConnectors c in ConnectorList)
                {
                    found = true;
                    if ((c.x1 == x) && (c.x2 == x))
                    {
                        //on our vertical  we know y1 is least
                        //check that c.cy1 is less than c.y2
                        if (c.y1 > c.y2)
                        {
                            y1 = y1;
                        }
                        if (!( (y2<=c.y1)||(y1>=c.y2)))
                        {
                            //overlap...  so move a bit??
                            found = false;
                            x += incx;
                            break;
                        }
                    }
                }
            }
 
            found = false;
            while (!found)
            {
                found = true;
                foreach (FlowObject f1 in BoxesList)
                {
                    if((f1.FlowoutSide != null)&&(x>300))
                    {
                        x=x;
                    }
                    if (f1.LinecutsBoxVertical(x, y1, y2))
                    {  
                        //so need to detour arround this one....
                        x += incx;
                        found = false;
                        break;
                    }
                }
            }

            return x;
        }


        private void DrawConnector(Graphics g, Pen p, int x1, int y1, int x2, int y2, Point A)
        {
            int y = 0; 
            if (y1 > y2) { y = y1; y1 = y2; y2 = y; }//order it so that y1<y2
            g.DrawLine(p, x1+A.X, y1+A.Y, x2+A.X, y2+A.Y);
            ConnectorList.Add(new FlowDiagramConnectors(x1, y1, x2, y2));
        }


        private void Draw2(Graphics g, Point A)
        {
            Pen p1 = new Pen(System.Drawing.Brushes.Black, 3);
            A.X = (int)(A.X * 100/scale); A.Y = (int)(A.Y * 100/scale);

            g.ScaleTransform(scale/100,scale/100);
            int y = 0; ConnectorList.Clear();
            int dy1 = 10;
            foreach (FlowObject f in BoxesList)
            {
                if (f.FlowOut != null)
                {
                    int x1 = f.x_position; 
                    int y1 = f.y_position + f.height;
                    int x2 = f.FlowOut.x_position; 
                    int y2 = f.FlowOut.y_position;

                    //so need to join x1,y1 to x2,y2                 
                    //x is the vertical route...
                    //so down a litle
                    DrawConnector(g, p1, x1, y1, x1, y1 + dy1,A);
                    DrawConnector(g, p1, x2, y2, x2, y2 - dy1,A);
                    y1 += dy1; y2 -= dy1;

                    int x = FindFreeVertical(x1, y1, y2, dy1);
                    int x0 = FindFreeVertical(x1, y1, y2, -dy1);
                    if ((x - x1) > (x1 - x0)) x = x0;  //use the nearest...

                    //so at present route is from x1,y1 to x,y1  to x,y2 to x2,y2
                    //now need to check the horizontals are OK..
                    if (x != x1)// so going across
                    {
                        DrawConnector(g, p1, x1, y1 , x, y1,A);
                    }
                    DrawConnector(g, p1, x, y1, x, y2,A);
                    DrawConnector(g, p1, x2, y2, x, y2,A );

                }
                if (f.FlowoutSide != null)
                {
                    //have a decision box and a side output.....
                    int x1 = f.x_position + f.width / 2; int y1 = f.y_position + f.height / 2;
                    int x2 = f.FlowoutSide.x_position; int y2 = f.FlowoutSide.y_position;
                    
                    int x = FindFreeVertical(x1+20, y1, y2-20, 1);
                    DrawConnector(g, p1, x1, y1, x, y1,A);
                    DrawConnector(g, p1, x, y1, x, y2 - 20,A);
                    DrawConnector(g, p1, x, y2 - 20, x2, y2 - 20,A);
                    DrawConnector(g, p1, x2, y2 - 20, x2, y2,A);
                }
            }              
        }

        public void ConnectTwo(Graphics g, Pen p, int x1, int y1, int x2, int y2, ref int n, Point A)
        {
            int n1 = n; int x; int y; int inc0 = 0;
            int inc = 5; inc0 = inc;
            if (n1 > 10) 
                return;
            if (x1 == x2)
            {
                //pure vertical
                x = FindFreeVertical(x1, y1, y2, inc);
                int x0 = FindFreeVertical(x1, y1, y2, -inc);
                if ((x - x1) > (x1 - x0)) { x = x0; inc0 = -inc; }  //use the nearest...
                if (x == x1)
                {
                    //no obstruction  so draw...
                    DrawConnector(g, p, x1, y1, x2, y2 , A);return;
                }
                else
                {
                    //lets see how far we can go....
                    y = y1 + 1;
                    while ((FindFreeVertical(x1, y1, y, inc0) == x1) && (y < y2)) { y++; }
                    DrawConnector(g, p, x1, y1, x1, y,A);
                    x = FindFreeVertical(x1, y, y2, inc0);
                    //so need to connect from x1,y to x,y1  and then x,y1 to x,y2  and then x,y2 to x2,y2
                    n1++; ConnectTwo(g, p, x1, y, x, y, ref n1,A); n1++;
                    ConnectTwo(g, p, x, y, x, y, ref n1,A); n1++;  
                    ConnectTwo(g, p, x, y, x2, y2, ref n1,A); 
                    return;
                }
            }

            if (y1 == y2)
            {
                //pure horiz
                y =  FindFreeHorizontal(y1,x1,x2, inc); 
                int y0 = FindFreeHorizontal(y1,x1,x2,-inc);
                if ((y - y1) > (y1-y0)) y = y0;  //use the nearest...
                if (y == y1)
                {
                    DrawConnector(g, p, x1, y1, x2, y2,A);
                    return;
                }
                else
                {
                    DrawConnector(g, p, x1, y1, x2, y2,A);
                    return;
                    //so need to connect from x1,y1 to x1,y  and then x1,y to x2,y  and then x2,y to x2,y2
                    n1++;
                    ConnectTwo(g, p, x1, y1, x1, y, ref n1,A); n1++;
                    ConnectTwo(g, p, x1, y, x2, y, ref n1,A); n1++;
                    ConnectTwo(g, p, x2, y, x2, y2, ref n1,A); n1++;        
                    return;
                }
                return;
            }
            //so neither vert or horiz...  so split into  two....

            ConnectTwo(g, p, x1, y1, x2, y1, ref n1,A); 
            ConnectTwo(g,p,x2,y1,x2,y2,ref n1,A);
            return;
        }

        public void Draw(Graphics g, Point A)
        {
            Draw2(g, A); return;
        }


        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //to save object to archive
            info.AddValue("Start", start);
            info.AddValue("BoxesList", BoxesList);

        }

        public FlowDiagram(SerializationInfo info, StreamingContext ctxt)
        {
            start = (StartBox)info.GetValue("Start", typeof(StartBox));
            this.BoxesList = (List<FlowObject>)info.GetValue("BoxesList", typeof(List<FlowObject>));
        }

        #endregion
    }

    [Serializable]
    public class StartBox : FlowObject, ISerializable
    {
        public StartBox()
        {    
            text = "Start";
            FlowOut = null;          
        }
        protected override void OnPaint(PaintEventArgs e)
        {        

            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Black, 3);
            if (Selected) p = new Pen(Brushes.Aqua, 3);
            Font f = new Font("Arial", 10);
            g.DrawEllipse(p, 1, 1,(float)(width*0.96), (float)(height * 0.90));
            g.DrawString(text, f, Brushes.Black, 20, 10);
            //base.OnPaint(e);
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public StartBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion
    }
    [Serializable]
    public class InputBox : FlowObject, ISerializable
    {

        public InputBox(int x, int y)
        {
            text = "Input S0";
            x_position = x;
            y_position = y;
            Location = new Point(x-width/2,y);
            Asmcode[0] = "linexx:     IN S0,I"; n_asm = 1;
        }

        public InputBox(int x, int y, string Sn)
        {
            text = "Input "+Sn;
            x_position = x;
            y_position = y;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:     IN "+Sn+",I"; n_asm = 1;
            text = Sn;
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public InputBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); 
            Font f = new Font("Arial", 10);
            if (Selected) BackColor = Color.Aqua;
            //if (Selected) p = new Pen(Brushes.Aqua, 3);
            System.Drawing.Point[] points1 = new Point[5];
            //want to bend by about 20%;
            int x =width/5; int y = 0;
            points1[0] = new Point(x, y); points1[4] = new Point(x, y);
            x = width; points1[1] = new Point(x, y); x = x-width/5; y = y + height-3;
            points1[2] = new Point(x, y);
            points1[3] = new Point(0, y);
            e.Graphics.DrawLines(p, points1);
            e.Graphics.DrawString(text, f, Brushes.Black, width/5, (height-20)/2);

        }

        public void Asm()
        {
            Asmcode[0] = "linexx:      IN " + text.Substring(6) + ",I"; n_asm = 1;
        }
    }
    [Serializable]
    public class ADCBox : FlowObject, ISerializable
    {

        public ADCBox(int x, int y)
        {
            text = "Read ADC,S0";
            x_position = x;
            y_position = y;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:     rcall READADC"; n_asm = 1;
        }

        public void Asm()
        {
            Asmcode[0] = "linexx:     rcall READADC"; n_asm = 1;
        }
        
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public ADCBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3);
            Font f = new Font("Arial", 10);
            if (Selected) BackColor = Color.Aqua;
            //if (Selected) p = new Pen(Brushes.Aqua, 3);
            System.Drawing.Point[] points1 = new Point[5];
            //want to bend by about 20%;
            int x = width / 5; int y = 0;
            points1[0] = new Point(x, y); points1[4] = new Point(x, y);
            x = width; points1[1] = new Point(x, y); x = x - width / 5; y = y + height - 3;
            points1[2] = new Point(x, y);
            points1[3] = new Point(0, y);

            e.Graphics.DrawLines(p, points1);

            e.Graphics.DrawString(text.Substring(0,4), f, Brushes.Black, width/5, 0);
            e.Graphics.DrawString(text.Substring(5), f, Brushes.Black, width/10, (int)(0.4*height));

        }
    }
    [Serializable]
    public class OutputBox : FlowObject, ISerializable
    {

        public OutputBox(int x, int y)
        {
            y_position = y; text = "Out S0"; x_position = x;
            Location = new Point(x-width/2, y);
            Asm();
        }
        public OutputBox(int x, int y, string Sn)
        {
            text = "Out "+Sn;
            x_position = x;
            y_position = y;
            Location = new Point(x - width / 2, y);
            Asm();
        }

        public void Asm()
        {
            Asmcode[0] = "linexx:     Out Q," + text.Substring(4); n_asm = 1;
        }
        
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public OutputBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            System.Drawing.Point[] points1 = new Point[5];
            int x = width / 5; int y = 0;
            points1[0] = new Point(x, y); points1[4] = new Point(x, y);
            x = width; points1[1] = new Point(x, y); x = x - width / 5; y = y + height - 3;
            points1[2] = new Point(x, y);
            points1[3] = new Point(0, y);
            g.DrawLines(p, points1);
            e.Graphics.DrawString(text, f, Brushes.Black, width / 8, (height - 20) / 2);
        }
    }
    [Serializable]
    public class EndBox : FlowObject, ISerializable
    {

        public EndBox(int x, int y)
        {
            y_position = y; text = "Stop"; x_position = x;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:     JP   linexx"; n_asm = 1;
        }
        
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public EndBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Black, 3);
            if (Selected) p = new Pen(Brushes.Aqua, 3);
            Font f = new Font("Arial", 10);
            g.DrawEllipse(p, 1, 1, (float)(width * 0.96), (float)(height * 0.90));
            g.DrawString(text, f, Brushes.Black, 20, 10);
            
        }
    }

    [Serializable]
    public class ReturnBox : FlowObject, ISerializable
    {
        //used by sub-process in place of end...
        public ReturnBox(int x, int y)
        {
            y_position = y; text = "Return"; x_position = x;
            Location = new Point(x - width / 2, y);
            Asm();
        }

        public void Asm()
        {
            Asmcode[0] = "linexx:     NOP"; n_asm = 1;
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public ReturnBox(SerializationInfo info, StreamingContext ctxt)
        {
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Black, 3);
            if (Selected) p = new Pen(Brushes.Aqua, 3);
            Font f = new Font("Arial", 10);
            g.DrawEllipse(p, 0, 0, width, (float)(height * 0.95));
            g.DrawString(text, f, Brushes.Black, 20, 10);

        }
    }

    [Serializable]
    public class ProcessBox : FlowObject, ISerializable
    {
        public ProcessType op_type;
        public string Sn = "";
        public string Sm = "";
        public string b = "";

        public ProcessBox(int x, int y)
        {
            y_position = y; text = "S0 = 0"; x_position = x;
            Location = new Point(x - width / 2, y);
            op_type = ProcessType.movi;
            b = "00"; Sn = "S0"; Sm = "S0";
            Asm();
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("op_type", op_type);
            info.AddValue("Sn", Sn);
            info.AddValue("Sm", Sm);
            info.AddValue("b", b);
            base.GetObjectData(info, context);
        }

        public ProcessBox(SerializationInfo info, StreamingContext ctxt)
        {
            op_type = (ProcessType)info.GetValue("op_type",typeof(ProcessType));
            Sn = info.GetString("Sn");
            Sm = info.GetString("Sm");
            b = info.GetString("b");
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            if(text.Length>9)
                f = new Font("Arial", 8);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            g.DrawRectangle(p, 0, 0, width - 3, height - 3);
            g.DrawString(text, f, Brushes.Black, 0, (height - 20) / 2);
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
        public void Asm()
        {
            string s="";
            switch (op_type)
            {
                case ProcessType.movi:
                    s = Sn + " = " + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:    movi   " + Sn + "," + b; n_asm = 1;
                    break;
                case ProcessType.mov:
                    s = Sn + " = " + Sm;
                    Asmcode[0] = "linexx:     mov  " + Sn + "," + Sm; n_asm = 1;
                    break;
                case ProcessType.addi:
                    s = Sn + " = " + Sn + "+" + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:    movi   s8," + b;
                    Asmcode[1] = "          add    " + Sn + ",s8"; n_asm  = 2;
                    break;
                case ProcessType.subi:
                    s = Sn + " = " + Sn + "-" + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:    movi   s8," + b; 
                    Asmcode[1] = "          sub    " + Sn + ",s8"; n_asm  = 2;
                    break;
                case ProcessType.wait:
                    s = "Wait " + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:      MOVI S8," + b;
                    Asmcode[1] = "Dellinexx:         RCALL wait1ms";
                    Asmcode[2] = "            DEC S8";
                    Asmcode[3] = "            JNZ Dellinexx"; n_asm = 4;
                    break;
                case ProcessType.and:
                    s = Sn + "=" + Sn + " AND " + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:      MOVI S8," + b;
                    Asmcode[1] = "    and     " + Sn + ",S8"; n_asm  = 2;
                    break;
                case ProcessType.orr:
                    s = Sn + "=" + Sn + " OR " + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:      MOVI S8," + b;
                    Asmcode[1] = "    orr     " + Sn + ",S8"; n_asm  = 2;
                    break;
                case ProcessType.xor:
                    s = Sn + "=" + Sn + " XOR " + b; CheckHexByte(b);
                    Asmcode[0] = "linexx:      MOVI S8," + b;
                    Asmcode[1] = "    eor     " + Sn + ",S8"; n_asm  = 2;
                    break;
                case ProcessType.add:
                    s = Sn + "=" + Sn + " + " + Sm;
                    Asmcode[0] = "linexx:     add     " + Sn + "," + Sm; n_asm  = 1;
                    break;
                case ProcessType.sub:
                    s = Sn + "=" + Sn + " - " + Sm;
                    Asmcode[0] = "linexx:     sub     " + Sn + "," + Sm; n_asm = 1;
                    break;
                default: break;
            }
            text = s;
        }

    }
    [Serializable]
    public class SubProcessBox : FlowObject, ISerializable
    {
        public string FileName;
        public FlowDiagram diagram1;
        public string Description;

        public SubProcessBox(int x, int y)
        {
            y_position = y; 
            text = "...";
            x_position = x;
            Location = new Point(x - width / 2, y); Asm();
        }
        public void Asm()
        {
            Asmcode[0] = "linexx:  nop"; n_asm = 1;
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FileName", FileName);
            info.AddValue("FlowDiagram", diagram1);
            info.AddValue("Description", Description);
            base.GetObjectData(info, context);
        }

        public SubProcessBox(SerializationInfo info, StreamingContext ctxt)
        {
            FileName = info.GetString("FileName");
            diagram1 = (FlowDiagram)info.GetValue("FlowDiagram", typeof(FlowDiagram));
            Description = info.GetString("Description");
            base.Deserialise(info, ctxt);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            if (text.Length > 9)
                f = new Font("Arial", 8);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            g.DrawRectangle(p, 0, 0, width - 3, height - 3);
            g.DrawString(text, f, Brushes.Black, 0, (height - 20) / 2);
/*
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            if (text.Length > 9)
                f = new Font("Arial", 8);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            PointF [] points1 = new PointF[6];
            points1[0]= new PointF(width/4,0);points1[1]= new PointF(3*width/4,0);points1[2]= new PointF(width-3,height/2);
            points1[3] = new PointF(3 * width / 4, height - 3); points1[4] = new PointF(width / 4, height-3); points1[5] = new PointF(0, height/2);
            g.DrawPolygon(p, points1);
            g.DrawString(text, f, Brushes.Black, 10, (height - 20) / 2);
 */
        }

    }
    [Serializable]
    public class DecisionBox : FlowObject, ISerializable
    {
        public bool YesSide = true;//ie yes to side
        public bool RightSide = true; //ie No out to the right... not yet implemented
        public DecisionBox(int x, int y)
        {
            y_position = y; text = "S0 = 0"; x_position = x;
            height += height; Height = height;
            FlowoutSide = null;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:   movi  S8,0";
            Asmcode[1] = "         sub  S8,S0";
            Asmcode[2] = "         jz   lineyy";
            Asmcode[3] = "         jp   linezz";
            n_asm = 4;
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("YesSide", YesSide);
            info.AddValue("RightSide", RightSide);
            base.GetObjectData(info, context);
        }

        public DecisionBox(SerializationInfo info, StreamingContext context)
        {
            YesSide = (bool)info.GetValue("YesSide", typeof(bool));
            base.Deserialise(info, context);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            System.Drawing.Point[] points1 = new Point[5];
            int x = width/2; int y = 0;
            points1[0] = new Point(x, y); 
            points1[4] = new Point(x, y);
            x = width; y = height / 2; 
            points1[1] = new Point(x, y);
            x = width / 2; y = height;
            points1[2] = new Point(x, y);
            x = 0; y = height / 2;
            points1[3] = new Point(x, y);

            g.DrawLines(p, points1);
            g.DrawString(text, f, Brushes.Black, 10, (height-20)/2);
            f = new Font("Arial", 10);
            string s1 = "Y"; string s2 = "N";
            if (YesSide) { s1 = "N"; s2 = "Y"; }
            g.DrawString(s2, f, Brushes.Black, width - 25, (height - 20) / 2);
            g.DrawString(s1, f, Brushes.Black, width/2-10, height-20);
        }

        public void Asm(string Sn,string b,string op)
        {
            try
            {
                int i = System.Convert.ToInt32(b, 16);
                if (i < 0) throw new System.Exception();
                if (i > 0xff) throw new System.Exception();
            }
            catch
            {
                MessageBox.Show("Byte " +b + " is not a valid Hexdecimal value between 0 and FF");
            }
            if (op == "=")
            {
                Asmcode[0] = "linexx:    movi  S8," + b;
                Asmcode[1] = "           sub   S8," + Sn;
                Asmcode[2] = "           jz    lineyy";
                Asmcode[3] = "           jp    linezz"; n_asm = 4;
            }
            if (op == ">")
            {
                Asmcode[0] = "linexx:    movi  S8," + b;
                Asmcode[1] = "           sub   S8," + Sn;//test if eq...
                Asmcode[2] = "           jgt    lineyy";//test true... 
                Asmcode[3] = "           jp     linezz";//test false
                n_asm = 4;
            }
            text = Sn + op + b;
        }

    }


    [Serializable]
    public class BreakBox : FlowObject, ISerializable
    {
        public int break_number = 0;
        public BreakBox(int x, int y)
        {
            y_position = y; text = "Break 00"; x_position = x;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:   movi  S8,33";
            Asmcode[1] = "          RCALL  BREAK"; n_asm = 2;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Pen p = new Pen(Brushes.Red, 3);
            if (Selected) p = new Pen(Brushes.Aqua, 3);
            Font f = new Font("Arial", 10);
            g.DrawEllipse(p, 0, 0, width, height);
            g.DrawString(text, f, Brushes.Black, 20, 10);
            //base.OnPaint(e);
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("break_number", break_number);
            base.GetObjectData(info, context);
        }

        public BreakBox(SerializationInfo info, StreamingContext ctxt)
        {
            break_number = info.GetInt16("break_number");
            base.Deserialise(info, ctxt);
        }

        #endregion
    }


    //these for simple version
    [Serializable]
    public class OutputPin : FlowObject, ISerializable
    {
        public int pin_number = 0;
        public bool state = false;//off
        public OutputPin(int x, int y)
        {
            y_position = y; text = "A OFF"; x_position = x;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:   BTC    Q,0"; n_asm = 1;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            System.Drawing.Point[] points1 = new Point[5];
            int x = width / 5; int y = 0;
            points1[0] = new Point(x, y); points1[4] = new Point(x, y);
            x = width; points1[1] = new Point(x, y); x = x - width / 5; y = y + height - 3;
            points1[2] = new Point(x, y);
            points1[3] = new Point(0, y);
            g.DrawLines(p, points1);
            e.Graphics.DrawString(text, f, Brushes.Black, width / 8, (height - 20) / 2);
            //base.OnPaint(e);
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("pin_number", pin_number);
            info.AddValue("state", state);
            base.GetObjectData(info, context);
        }

        public OutputPin(SerializationInfo info, StreamingContext ctxt)
        {
            pin_number = info.GetInt16("pin_number");
            state = info.GetBoolean("state");
            base.Deserialise(info, ctxt);
        }

        #endregion
    }
    [Serializable]
    public class PlayNote : FlowObject, ISerializable
    {
        public int frequency = 440;/// between 250 and 1300
        public int TimeValue = 0;
        public int Length = 10;//ie 1 sec

        public PlayNote(int x, int y)
        {
            y_position = y; text = "Play 440"; x_position = x;
            Location = new Point(x - width / 2, y);

            decimal f1 = frequency;
            int TimeValue = (int)(1000000 / (f1 * 16));
            TimeValue = 0x100 - TimeValue;
            Asmcode[0] = "linexx:   movi   S0,"+TimeValue.ToString("X"); n_asm = 0;
            Asmcode[1] = "          movi   S1,10";
            Asmcode[2] = "          rcall   PlayS0"; n_asm = 3;



        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            System.Drawing.Point[] points1 = new Point[5];
            int x = width / 5; int y = 0;
            points1[0] = new Point(x, y); points1[4] = new Point(x, y);
            x = width; points1[1] = new Point(x, y); x = x - width / 5; y = y + height - 3;
            points1[2] = new Point(x, y);
            points1[3] = new Point(0, y);
            g.DrawLines(p, points1);

            e.Graphics.DrawString(text, f, Brushes.Black, width / 8, (height - 20) / 2);
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("frequency", frequency);
            base.GetObjectData(info, context);
        }

        public PlayNote(SerializationInfo info, StreamingContext ctxt)
        {
            frequency = info.GetInt16("frequency");
            base.Deserialise(info, ctxt);
        }

        #endregion
    }
    [Serializable]
    public class WaitSec : FlowObject, ISerializable
    {
        public decimal wait = 2;

        public WaitSec(int x, int y)
        {
            y_position = y; text = "Wait 2s"; x_position = x;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:     MOVI   S9,2";
            Asmcode[1] = "Dellinexx:  RCALL wait1Sec";
            Asmcode[2] = "            DEC   S9";
            Asmcode[3] = "            JNZ Dellinexx";
            n_asm = 4;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            if (text.Length > 9)
                f = new Font("Arial", 8);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            g.DrawRectangle(p, 0, 0, width - 3, height - 3);
            g.DrawString(text, f, Brushes.Black, 0, (height - 20) / 2);
            //base.OnPaint(e);
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("wait", wait);
            base.GetObjectData(info, context);
        }

        public WaitSec(SerializationInfo info, StreamingContext ctxt)
        {
            wait = info.GetInt16("wait");
            base.Deserialise(info, ctxt);
        }

        #endregion
    }

    [Serializable]
    public class SimpleDecisionBox : FlowObject, ISerializable
    {
        public bool YesSide = true;//ie yes to side
        public int Pinnumber = 0;
        public SimpleDecisionBox(int x, int y)
        {
            y_position = y; text = "I0 = 0??"; x_position = x;
            height += height; Height = height;
            FlowoutSide = null;
            Location = new Point(x - width / 2, y);
            Asmcode[0] = "linexx:  movi S9,1";
            Asmcode[1] = "         in   S8,I";
            Asmcode[2] = "         and  S8,S9";
            Asmcode[3] = "         jz   lineyy";
            Asmcode[4] = "         jp   linezz";
            n_asm = 5;
        }
        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("YesSide", YesSide);
            info.AddValue("PinNuber", Pinnumber);
            base.GetObjectData(info, context);
        }

        public SimpleDecisionBox(SerializationInfo info, StreamingContext context)
        {
            YesSide = (bool)info.GetValue("YesSide", typeof(bool));
            Pinnumber = (int)info.GetValue("YesSide", typeof(int));
            base.Deserialise(info, context);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); Pen p = new Pen(Brushes.Black, 3); Font f = new Font("Arial", 10);
            Graphics g = e.Graphics;
            if (Selected) BackColor = Color.Aqua;
            System.Drawing.Point[] points1 = new Point[5];
            int x = width / 2; int y = 0;
            points1[0] = new Point(x, y);
            points1[4] = new Point(x, y);
            x = width; y = height / 2;
            points1[1] = new Point(x, y);
            x = width / 2; y = height;
            points1[2] = new Point(x, y);
            x = 0; y = height / 2;
            points1[3] = new Point(x, y);

            g.DrawLines(p, points1);
            g.DrawString(text, f, Brushes.Black, 10, (height - 20) / 2);
            f = new Font("Arial", 10);
            string s1 = "Y"; string s2 = "N";
            if (YesSide) { s1 = "N"; s2 = "Y"; }
            g.DrawString(s2, f, Brushes.Black, width - 25, (height - 20) / 2);
            g.DrawString(s1, f, Brushes.Black, width / 2 - 10, height - 20);
        }


    }



}