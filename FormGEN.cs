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
using System.Management;
using Get_Windows_Product_Key.Tools;

namespace Ultimate_Control
{
    public partial class FormGEN : Form
    {
        public FormGEN()
        {
            InitializeComponent();

            RegistryKey ProductKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform");

            RegistryKey KernelCheck = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (ProductKey != null)
            {
                object productKeyValue = ProductKey.GetValue("BackupProductKeyDefault");

                if (productKeyValue != null)
                {
                    labelKey.Text = "Current product key\r\n" + KeyDecoder.GetWindowsProductKeyFromRegistry();
                    labelN.Visible = false;
                    labelNOT.Visible = false;
                    labelGEN.Visible = true;
                    labelY.Visible = true;
                    picN.Visible = false;
                    picY.Visible = true;
                }
                else
                {
                    labelKey.Text = "Product key not found.";
                    labelN.Visible = true;
                    labelNOT.Visible = true;
                    labelGEN.Visible = false;
                    labelY.Visible = false;
                    picN.Visible = true;
                    picY.Visible = false;
                }
            }
            else
            {
                labelKey.Text = "Error finding product key.";
            }

            RegistryKey OSname = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (OSname != null)
            {
                object OSnameValue = OSname.GetValue("ProductName");

                object OSbuildValue = OSname.GetValue("BuildLab");

                if (OSnameValue != null)
                {
                    labelOS.Text = "Current edition\r\n" + OSnameValue.ToString() + " (" + OSbuildValue.ToString() + ")";
                }
                else
                {
                    labelOS.Text = "No OS info in the registry.";
                }
            }
            else
            {
                labelOS.Text = "No OS info in the registry.";
            }
        }

        private void linkWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jackpomisoftware.github.io/isg.html");
        }

        private void linkEULA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("license.rtf");
        }

        private void linkSLUI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("ms-settings:activation");
        }

        private void linkMIT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JackPomiSoftware/IsGenuine");
        }
    }
}
