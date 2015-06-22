using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlowDiagrams.Dialogs
{
    public partial class DataTable : Form
    {
        public DataTable()
        {
            InitializeComponent();
            setup();
        }

        public void setup()
        {
            dataGridView1.Rows.Add(256);
            string s ="";
            for (int y = 0; y < 256; y++)
            {
                dataGridView1.Rows[y].Cells[8].ValueType = typeof(string);
                dataGridView1.Rows[y].Cells[8].Value = y.ToString("X");
                dataGridView1.Rows[y].Cells[8].ReadOnly = true;
                for (int x = 0; x < 8; x++)
                {
                    s = Convert.ToString(y, 2).PadLeft(8, '0');

                    dataGridView1.Rows[y].Cells[x].ValueType = typeof(string);
                    dataGridView1.Rows[y].Cells[x].Value = s.Substring(x, 1);
                    dataGridView1.Rows[y].Cells[x].ReadOnly = true;
                }
                for (int x = 9; x < 13; x++)
                {
                    dataGridView1.Rows[y].Cells[x].ValueType = typeof(string);
                    dataGridView1.Rows[y].Cells[x].Value = "0";

                    
                }
            }
        }
    }

    public class BinaryType
    {
        public bool value = false;
        public override string ToString()
        {
            if (value) return "1";
            return ("0");
        }
    }
}
