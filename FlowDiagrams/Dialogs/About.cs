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
    }
}
