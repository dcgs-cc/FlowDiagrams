using System;
using System.Collections.Generic;
using System.Deployment;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FlowDiagrams.Dialogs
{
    public partial class About : Form
    {

        public About()
        {
            InitializeComponent();

            label1.Text = Application.ProductName + " Version = ";
            try
            {
                label1.Text += System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch
            {
                label1.Text +=  Assembly.GetExecutingAssembly().GetName().Version;
            }
            
        }

        public void Setup(bool model)
        {
            pictureBox1.Visible = !model;
            pictureBox2.Visible = model;
            label2.Text=(model)? "Using Simple Model and hex file outoput for programmers and using 16F84"  : "Using Full Model and in-ciruit programmer and 16F8xx";
        }
    }
}
