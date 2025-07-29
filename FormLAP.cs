using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace Ultimate_Control
{
    public partial class FormLAP : Form
    {
        public FormLAP()
        {
            InitializeComponent();

            trackBarBrightness.Value = GetCurrentBrightness();

            RegistryKey BatKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes");

            if (BatKey.GetValue("ActivePowerScheme").ToString() == "381b4222-f694-41f0-9685-ff5bb260df2e")
            {
                combat.Text = "Balanced";
                radioBalance.Checked = true;
                linkBalanced.Visible = true;
            }

            if (BatKey.GetValue("ActivePowerScheme").ToString() == "a1841308-3541-4fab-bc81-f71556f20b4a")
            {
                combat.Text = "Power saver";
                radioEconomy.Checked = true;
                linkEconomy.Visible = true;
            }

            if (BatKey.GetValue("ActivePowerScheme").ToString() == "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c")
            {
                combat.Text = "High performance";
                radioPerf.Checked = true;
                linkPerf.Visible = true;
            }

            if (BatKey.GetValue("ActivePowerScheme").ToString() == "e9a42b02-d5df-448d-aa00-03f14749eb61")
            {
                combat.Text = "Ultimate Performance";
                radioUlt.Checked = true;
                linkUlt.Visible = true;
            }

            buttonApplyQ.Enabled = false;
            buttonApply2.Enabled = false;
        }

        void SetPowerPlan(string guid)
        {
            ProcessStartInfo psi = new ProcessStartInfo("powercfg", $"/S {guid}")
            {
                UseShellExecute = true,
                Verb = "runas" // ensures elevation prompt
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to change power plan: " + ex.Message);
            }
        }

        private void linkHelp2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void Slider_Scroll(object sender, EventArgs e)
        {

        }

        private void linkPres_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("presentationsettings.exe");
        }

        private void linkExt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("displayswitch.exe");
        }

        private void linkWMC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mblctr.exe");
        }

        private void combat_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonApplyQ.Enabled = true;
        }

        private void buttonApplyQ_Click(object sender, EventArgs e)
        {
            if (combat.Text == "Balanced")
            {
                SetPowerPlan("381b4222-f694-41f0-9685-ff5bb260df2e");
                linkBalanced.Visible = true;
                radioBalance.Checked = true;
                linkEconomy.Visible = false;
                linkPerf.Visible = false;
                linkUlt.Visible = false;
            }

            else if (combat.Text == "High performance")
            {
                SetPowerPlan("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
                linkPerf.Visible = true;
                radioPerf.Checked = true;
                linkEconomy.Visible = false;
                linkBalanced.Visible = false;
                linkUlt.Visible = false;
            }

            else if (combat.Text == "Power saver")
            {
                SetPowerPlan("a1841308-3541-4fab-bc81-f71556f20b4a");
                linkBalanced.Visible = false;
                radioEconomy.Checked = true;
                linkEconomy.Visible = true;
                linkPerf.Visible = false;
                linkUlt.Visible = false;
            }

            else if (combat.Text == "Ultimate Performance")
            {
                SetPowerPlan("e9a42b02-d5df-448d-aa00-03f14749eb61");
                linkBalanced.Visible = false;
                radioUlt.Checked = true;
                linkEconomy.Visible = false;
                linkPerf.Visible = false;
                linkUlt.Visible = true;
            }

            buttonApplyQ.Enabled = false;
            buttonApply2.Enabled = false;
        }

        private void linkBattery_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage2);
            linkBattery.Font = new Font(linkBattery.Font.Name, linkBattery.Font.Size, FontStyle.Bold);
            linkQuick.Font = new Font(linkQuick.Font.Name, linkQuick.Font.Size, FontStyle.Regular);
        }

        private void linkQuick_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
            linkBattery.Font = new Font(linkBattery.Font.Name, linkBattery.Font.Size, FontStyle.Regular);
            linkQuick.Font = new Font(linkQuick.Font.Name, linkQuick.Font.Size, FontStyle.Bold);
        }

        private void linkBalanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("control.exe", "powercfg.cpl,,1");
        }

        private void linkPerf_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("control.exe", "powercfg.cpl,,1");
        }

        private void linkEconomy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("control.exe", "powercfg.cpl,,1");
        }

        private void linkUlt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("control.exe", "powercfg.cpl,,1");
        }

        private void radioBalance_CheckedChanged(object sender, EventArgs e)
        {
            radioBalance.Font = new Font(radioBalance.Font.Name, radioBalance.Font.Size, FontStyle.Bold);
            radioPerf.Font = new Font(radioPerf.Font.Name, radioPerf.Font.Size, FontStyle.Regular);
            radioEconomy.Font = new Font(radioEconomy.Font.Name, radioEconomy.Font.Size, FontStyle.Regular);
            radioUlt.Font = new Font(radioUlt.Font.Name, radioUlt.Font.Size, FontStyle.Regular);
            buttonApply2.Enabled = true;
        }

        private void radioPerf_CheckedChanged(object sender, EventArgs e)
        {
            radioBalance.Font = new Font(radioBalance.Font.Name, radioBalance.Font.Size, FontStyle.Regular);
            radioPerf.Font = new Font(radioPerf.Font.Name, radioPerf.Font.Size, FontStyle.Bold);
            radioEconomy.Font = new Font(radioEconomy.Font.Name, radioEconomy.Font.Size, FontStyle.Regular);
            radioUlt.Font = new Font(radioUlt.Font.Name, radioUlt.Font.Size, FontStyle.Regular);
            buttonApply2.Enabled = true;
        }

        private void radioEconomy_CheckedChanged(object sender, EventArgs e)
        {
            radioBalance.Font = new Font(radioBalance.Font.Name, radioBalance.Font.Size, FontStyle.Regular);
            radioPerf.Font = new Font(radioPerf.Font.Name, radioPerf.Font.Size, FontStyle.Regular);
            radioEconomy.Font = new Font(radioEconomy.Font.Name, radioEconomy.Font.Size, FontStyle.Bold);
            radioUlt.Font = new Font(radioUlt.Font.Name, radioUlt.Font.Size, FontStyle.Regular);
            buttonApply2.Enabled = true;
        }

        private void radioUlt_CheckedChanged(object sender, EventArgs e)
        {
            radioBalance.Font = new Font(radioBalance.Font.Name, radioBalance.Font.Size, FontStyle.Regular);
            radioPerf.Font = new Font(radioPerf.Font.Name, radioPerf.Font.Size, FontStyle.Regular);
            radioEconomy.Font = new Font(radioEconomy.Font.Name, radioEconomy.Font.Size, FontStyle.Regular);
            radioUlt.Font = new Font(radioUlt.Font.Name, radioUlt.Font.Size, FontStyle.Bold);
            buttonApply2.Enabled = true;
        }

        private void buttonApply2_Click(object sender, EventArgs e)
        {
            if (radioBalance.Checked == true) 
            {
                SetPowerPlan("381b4222-f694-41f0-9685-ff5bb260df2e");
                combat.Text = "Balanced";
                linkBalanced.Visible = true;
                linkEconomy.Visible = false;
                linkPerf.Visible = false;
                linkUlt.Visible = false;
                buttonApply2.Enabled = false;
            }

            if (radioPerf.Checked == true)
            {
                SetPowerPlan("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
                combat.Text = "High performance";
                linkBalanced.Visible = false;
                linkEconomy.Visible = false;
                linkPerf.Visible = true;
                linkUlt.Visible = false;
                buttonApply2.Enabled = false;
            }

            if (radioEconomy.Checked == true)
            {
                SetPowerPlan("a1841308-3541-4fab-bc81-f71556f20b4a");
                combat.Text = "Power saver";
                linkBalanced.Visible = false;
                linkEconomy.Visible = true;
                linkPerf.Visible = false;
                linkUlt.Visible = false;
                buttonApply2.Enabled = false;
            }

            if (radioUlt.Checked == true)
            {
                SetPowerPlan("e9a42b02-d5df-448d-aa00-03f14749eb61");
                combat.Text = "Ultimate Performance";
                linkBalanced.Visible = false;
                linkEconomy.Visible = false;
                linkPerf.Visible = false;
                linkUlt.Visible = true;
                buttonApply2.Enabled = false;
            }
        }

        private void SetBrightness(byte brightness)
        {
            try
            {
                var scope = new ManagementScope("root\\wmi");
                var query = new SelectQuery("WmiMonitorBrightnessMethods");

                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObj in searcher.Get())
                    {
                        mObj.InvokeMethod("WmiSetBrightness", new object[] { 1, brightness });
                        break; // Apply to the first display only
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting brightness: " + ex.Message);
            }
        }

        private byte GetCurrentBrightness()
        {
            try
            {
                var scope = new ManagementScope("root\\wmi");
                var query = new SelectQuery("WmiMonitorBrightness");

                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObj in searcher.Get())
                    {
                        return (byte)mObj["CurrentBrightness"];
                    }
                }
            }
            catch (Exception ex)
            {
                labelBrightness.Text = "Your device does not support\r\nthis feature.";
                trackBarBrightness.Enabled = false;
            }

            return 0;
        }

        private void linkOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ultimatecontrol.github.io/doc/");
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            byte brightness = (byte)trackBarBrightness.Value;
            SetBrightness(brightness);
        }
    }
}
