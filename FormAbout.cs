using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ultimate_Control
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            CheckRegistry();

        }
        private bool AUC = true;

        private void CheckRegistry()
        {
            RegistryKey CheckKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Jack Pomi Software\Ultimate Control");
            if (CheckKey.GetValue("AutoUpdateCheck").ToString() == "0")
            {
                checkBox1.Checked = false;
                AUC = false;
            }
            else
            {
                checkBox1.Checked = true;
                AUC = true;
            }
        }

        private void linkVisit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ultimatecontrol.github.io/");
        }

        private void buttonAboutOK_Click(object sender, EventArgs e)
        {
            RegistryKey CheckKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Jack Pomi Software\Ultimate Control");
            if (checkBox1.Checked && checkBox1.Checked != AUC) 
            {
                CheckKey.SetValue("AutoUpdateCheck", 1);
            }
            if (checkBox1.Checked == false && checkBox1.Checked != AUC)
            {
                CheckKey.SetValue("AutoUpdateCheck", 0);
            }
            this.Close();
        }
    }
}
