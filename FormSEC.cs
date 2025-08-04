using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Ultimate_Control.Properties;

namespace Ultimate_Control
{
    public partial class FormSEC : Form
    {
        public FormSEC()
        {
            InitializeComponent();

            /////////////////////////////////////////////////
            /////////////////////////////////////////////////
            //// U S E R   A C C O U N T   C O N T R O L ////
            /////////////////////////////////////////////////
            /////////////////////////////////////////////////

            RegistryKey InitKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");

            //////////
            // Check if User elevation is disabled
            //////////

            if (InitKey.GetValue("ConsentPromptBehaviorUser").ToString() == "1" || InitKey.GetValue("ConsentPromptBehaviorUser").ToString() == "3")
            {
                checkNoElev.Checked = false;
            }

            if (InitKey.GetValue("ConsentPromptBehaviorUser").ToString() == "0")
            {
                checkNoElev.Checked = true;
            }

            //////////
            // Check if signed apps only
            //////////

            if (InitKey.GetValue("ValidateAdminCodeSignatures").ToString() == "1")
            {
                checkSign.Checked = true;
            }

            if (InitKey.GetValue("ValidateAdminCodeSignatures").ToString() == "0")
            {
                checkSign.Checked = false;
            }

            //////////
            // Check if LUA is off
            //////////

            if (InitKey.GetValue("EnableLUA").ToString() == "0")
            {
                checkLUA.Checked = true;
                buttonApply2.Enabled = false;
                Slider.Enabled = false;
                warn.Visible = false;

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "0" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "0")
                {
                    Slider.Value = 0;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "5" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "0")
                {
                    Slider.Value = 1;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "5" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "1")
                {
                    Slider.Value = 2;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "2" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "1")
                {
                    Slider.Value = 3;
                }
            }

            if (InitKey.GetValue("EnableLUA").ToString() == "1")
            {
                checkLUA.Checked = false;
                buttonApply2.Enabled = false;
                Slider.Enabled = true;
                labelUACDesc.ForeColor = Color.Black;

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "0" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "0")
                {
                    Slider.Value = 0;
                    labelUACHead.Text = "Level 0: User Account Control turned off";
                    labelUACDesc.Text = "User Account Control won't bother you at all.\r\n\r\nBut be aware that your computer may become vunerable to \r\nmalware and viruses. Jack Pomi Software is NOT responsible for \r\nany of your actions.";
                    warn.Visible = true;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "5" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "0")
                {
                    Slider.Value = 1;
                    labelUACHead.Text = "Level 1: Programs only, no screen darkening";
                    labelUACDesc.Text = "User Account Control will trigger only for programs that require its \r\npermission. In this level, there is no screen darkening.\r\n";
                    warn.Visible = false;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "5" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "1")
                {
                    Slider.Value = 2;
                    labelUACHead.Text = "Level 2: Programs only";
                    labelUACDesc.Text = "User Account Control will trigger only for programs that require its \r\npermission. The screen will become dark when UAC is triggered.\r\n\r\nThis is the default setting in Windows 7 and later.\r\n";
                    warn.Visible = false;
                }

                if (InitKey.GetValue("ConsentPromptBehaviorAdmin").ToString() == "2" && InitKey.GetValue("PromptOnSecureDesktop").ToString() == "1")
                {
                    Slider.Value = 3;
                    labelUACHead.Text = "Level 3: User Account Control is fully enabled";
                    labelUACDesc.Text = "User Account Control will trigger every time you run programs that \r\nrequire its permission or when you change some of the system's \r\nsettings. The screen will become dark when UAC is triggered.\r\n\r\nThis is the only setting available by default in Windows Vista.\r\n";
                    warn.Visible = false;
                }
            }

            /////////////////////////
            /////////////////////////
            //// F I R E W A L L ////
            /////////////////////////
            /////////////////////////

            button2.MouseEnter += new EventHandler(button2_MouseEnter);
            button2.MouseLeave += new EventHandler(button2_MouseLeave);
            button3.MouseEnter += new EventHandler(button3_MouseEnter);
            button3.MouseLeave += new EventHandler(button3_MouseLeave);
            button4.MouseEnter += new EventHandler(button4_MouseEnter);
            button4.MouseLeave += new EventHandler(button4_MouseLeave);

            CheckPrivateFirewallSettings();
            CheckPublicFirewallSettings();
            CheckDomainFirewallSettings();
            SubscribeToChangeEvents(this);
            HideDomain();
            DisplayNetworkInformation();
            DisplayNetworkType();
            CheckNetType();
        }

        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        //// F I R E W A L L   N E T W O R K   N A M E   A N D   T Y P E ////
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        private void DisplayNetworkInformation()
        {
            try
            {
                var activeNetworks = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up &&
                               n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .ToList();

                if (activeNetworks.Count == 0)
                {
                    NetworkName.Text = "Not connected";
                }
                else if (activeNetworks.Count == 1)
                {
                    NetworkName.Text = activeNetworks[0].Name;
                }
                else
                {
                    NetworkName.Text = "Multiple networks";
                }
            }
            catch
            {
                NetworkName.Text = "Error getting network info";
            }
        }

        private bool NetType = false;

        private void DisplayNetworkType()
        {
            try
            {
                // Get the current network type
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up &&
                        ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        // Check if this is the active network interface
                        if (ni.GetIPProperties().GetIPv4Properties() != null)
                        {
                            // Get the network category (Public/Private/DomainAuthenticated)
                            NetworkType.Image = ni.GetIPProperties().GetIPv4Properties().IsDhcpEnabled
                            ? Properties.Resources.NetTypeHome // Use your private network image
                            : Properties.Resources.NetTypePublic; // Use your public network image

                            NetType = ni.GetIPProperties().GetIPv4Properties().IsDhcpEnabled
                            ? true
                            : false;
                            return;
                        }
                    }
                }

                NetworkType.Image = Properties.Resources.NetTypeHome;
            }
            catch (Exception ex)
            {
                NetworkType.Text = "Error: " + ex.Message;
            }
        }

        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////
        //// F I R E W A L L   W I N D O W   C O N T R O L ////
        ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////

        private bool FPri = false;
        private bool FPub = false;
        private bool FD = false;

        private bool FBlockPri = false;
        private bool FBlockPub = false;
        private bool FBlockD = false;

        private bool FNotifyPri = false;
        private bool FNotifyPub = false;
        private bool FNotifyD = false;

        private void HideDomain()
        {
            gsettingsDomain.Visible = false;
            buttonFWApply.Top -= 70;
            labelNet.Top -= 70;
            NetworkType.Top -= 70;
            NetworkName.Top -= 70;
            linkLabelMMC.Top -= 70;
        }

        private void HidePublic()
        {
            gsettingsPublic.Visible = false;
            groupDomain.Top -= 70;
            gsettingsDomain.Top -= 70;
            buttonFWApply.Top -= 70;
            labelNet.Top -= 70;
            NetworkType.Top -= 70;
            NetworkName.Top -= 70;
            linkLabelMMC.Top -= 70;
        }

        private void HidePrivate()
        {
            gsettingsHome.Visible = false;
            groupPublic.Top -= 70;
            gsettingsPublic.Top -= 70;
            groupDomain.Top -= 70;
            gsettingsDomain.Top -= 70;
            buttonFWApply.Top -= 70;
            labelNet.Top -= 70;
            NetworkType.Top -= 70;
            NetworkName.Top -= 70;
            linkLabelMMC.Top -= 70;
        }

        private void CheckNetType()
        {
            if (NetType == true)
            {
                HidePublic();
                button3.Image = Properties.Resources.ButtonDown;
                StatusTextPublic.Left -= 100;
                StatusTextPublic.Text = StatusTextPublic.Text + ", not connected";
            }
            if (NetType == false)
            {
                HidePrivate();
                button2.Image = Properties.Resources.ButtonDown;
                StatusTextPrivate.Left -= 100;
                StatusTextPrivate.Text = StatusTextPrivate.Text + ", not connected";
            }
            else
            {
                Console.WriteLine("WHAAAAAAAAAAAAA");
            }
        }

        private void Control_Changed(object sender, EventArgs e)
        {
            buttonFWApply.Enabled = true;
        }

        private void SubscribeToChangeEvents(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += Control_Changed;
                }
                else if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged += Control_Changed;
                }
                // Add more control types as needed

                // Recursively check child controls (for containers like GroupBox)
                if (control.HasChildren)
                {
                    SubscribeToChangeEvents(control);
                }
            }
        }

        private void CheckPrivateFirewallSettings()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "advfirewall show privateprofile";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866); // OEM Russian
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                bool blockAllInbound = false;
                bool notificationsEnabled = false;
                bool firewallEnabled = false;

                // Parse the output line by line
                using (StringReader reader = new StringReader(output))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        // Check for firewall state (ON/OFF)
                        if (line.StartsWith("State") || line.StartsWith("Состояние"))
                        {
                            if (line.Contains("ON") || line.Contains("ВКЛЮЧИТЬ"))
                            {
                                firewallEnabled = true;
                                StatusPanelPrivate.Image = Properties.Resources.StatusPanelOn;
                                StatusIconPrivate.Image = Properties.Resources.StatusIconOn;
                                StatusTextPrivate.Text = "On";
                            }
                            else if (line.Contains("OFF") || line.Contains("ВЫКЛЮЧИТЬ"))
                            {
                                firewallEnabled = false;
                                StatusPanelPrivate.Image = Properties.Resources.StatusPanelOff;
                                StatusIconPrivate.Image = Properties.Resources.StatusIconOff;
                                StatusTextPrivate.Text = "Off";
                            }

                        }

                        // Check for firewall policy (inbound blocking)
                        if (line.StartsWith("Firewall Policy") ||
                            line.StartsWith("Политика брандмауэра"))
                        {
                            if (line.Contains("BlockInboundAlways"))
                            {
                                blockAllInbound = true;
                            }
                            else if (line.Contains("BlockInbound"))
                            {
                                blockAllInbound = false; // Normal blocking (not "always")
                            }
                        }

                        // Check for notification setting
                        if (line.StartsWith("InboundUserNotification"))
                        {
                            if (line.Contains("Enable") || line.Contains("Включить"))
                            {
                                notificationsEnabled = true;
                            }
                            else if (line.Contains("Disable") || line.Contains("Отключить"))
                            {
                                notificationsEnabled = false;
                            }
                        }
                    }
                }

                // Update your UI controls
                radioPrivateON.Checked = firewallEnabled;
                radioPrivateOFF.Checked = !firewallEnabled;
                checkBIPrivate.Checked = blockAllInbound;
                checkNPrivate.Checked = notificationsEnabled;
                FPri = firewallEnabled;
                FNotifyPri = notificationsEnabled;
                FBlockPri = blockAllInbound;

                if (FBlockPri == true && FPri == true)
                {
                    StatusIconPrivate.Image = Properties.Resources.StatusIconBlock;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking firewall settings: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckPublicFirewallSettings()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "advfirewall show publicprofile";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866); // OEM Russian
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                bool blockAllInbound = false;
                bool notificationsEnabled = false;
                bool firewallEnabled = false;

                // Parse the output line by line
                using (StringReader reader = new StringReader(output))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        // Check for firewall state (ON/OFF)
                        if (line.StartsWith("State") || line.StartsWith("Состояние"))
                        {
                            if (line.Contains("ON") || line.Contains("ВКЛЮЧИТЬ"))
                            {
                                firewallEnabled = true;
                                StatusPanelPublic.Image = Properties.Resources.StatusPanelOn;
                                StatusIconPublic.Image = Properties.Resources.StatusIconOn;
                                StatusTextPublic.Text = "On";
                            }
                            else if (line.Contains("OFF") || line.Contains("ВЫКЛЮЧИТЬ"))
                            {
                                firewallEnabled = false;
                                StatusPanelPublic.Image = Properties.Resources.StatusPanelOff;
                                StatusIconPublic.Image = Properties.Resources.StatusIconOff;
                                StatusTextPublic.Text = "Off";
                            }

                        }

                        // Check for firewall policy (inbound blocking)
                        if (line.StartsWith("Firewall Policy") ||
                            line.StartsWith("Политика брандмауэра"))
                        {
                            if (line.Contains("BlockInboundAlways"))
                            {
                                blockAllInbound = true;
                            }
                            else if (line.Contains("BlockInbound"))
                            {
                                blockAllInbound = false; // Normal blocking (not "always")
                            }
                        }

                        // Check for notification setting
                        if (line.StartsWith("InboundUserNotification"))
                        {
                            if (line.Contains("Enable") || line.Contains("Включить"))
                            {
                                notificationsEnabled = true;
                            }
                            else if (line.Contains("Disable") || line.Contains("Отключить"))
                            {
                                notificationsEnabled = false;
                            }
                        }
                    }
                }

                // Update your UI controls
                radioPubON.Checked = firewallEnabled;
                radioPubOFF.Checked = !firewallEnabled;
                checkBIPublic.Checked = blockAllInbound;
                checkNPublic.Checked = notificationsEnabled;
                FPub = firewallEnabled;
                FNotifyPub = notificationsEnabled;
                FBlockPub = blockAllInbound;
                if (FBlockPub == true && FPub == true)
                {
                    StatusIconPublic.Image = Properties.Resources.StatusIconBlock;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking firewall settings: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckDomainFirewallSettings()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "advfirewall show domainprofile";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866); // OEM Russian
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                bool blockAllInbound = false;
                bool notificationsEnabled = false;
                bool firewallEnabled = false;

                // Parse the output line by line
                using (StringReader reader = new StringReader(output))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        // Check for firewall state (ON/OFF)
                        if (line.StartsWith("State") || line.StartsWith("Состояние"))
                        {
                            if (line.Contains("ON") || line.Contains("ВКЛЮЧИТЬ"))
                            {
                                firewallEnabled = true;
                                StatusPanelDomain.Image = Properties.Resources.StatusPanelOn;
                                StatusIconDomain.Image = Properties.Resources.StatusIconOn;
                                StatusTextDomain.Text = "On";
                            }
                            else if (line.Contains("OFF") || line.Contains("ВЫКЛЮЧИТЬ"))
                            {
                                firewallEnabled = false;
                                StatusPanelDomain.Image = Properties.Resources.StatusPanelOff;
                                StatusIconDomain.Image = Properties.Resources.StatusIconOff;
                                StatusTextDomain.Text = "Off";
                            }

                        }

                        // Check for firewall policy (inbound blocking)
                        if (line.StartsWith("Firewall Policy") ||
                            line.StartsWith("Политика брандмауэра"))
                        {
                            if (line.Contains("BlockInboundAlways"))
                            {
                                blockAllInbound = true;
                            }
                            else if (line.Contains("BlockInbound"))
                            {
                                blockAllInbound = false; // Normal blocking (not "always")
                            }
                        }

                        // Check for notification setting
                        if (line.StartsWith("InboundUserNotification"))
                        {
                            if (line.Contains("Enable") || line.Contains("Включить"))
                            {
                                notificationsEnabled = true;
                            }
                            else if (line.Contains("Disable") || line.Contains("Отключить"))
                            {
                                notificationsEnabled = false;
                            }
                        }
                    }
                }

                // Update your UI controls
                radioDomON.Checked = firewallEnabled;
                radioDomOFF.Checked = !firewallEnabled;
                checkBIDomain.Checked = blockAllInbound;
                checkNDomain.Checked = notificationsEnabled;
                FD = firewallEnabled;
                FNotifyD = notificationsEnabled;
                FBlockD = blockAllInbound;
                if (FBlockD == true && FD == true)
                {
                    StatusIconDomain.Image = Properties.Resources.StatusIconBlock;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking firewall settings: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        //// U S E R   A C C O U N T   C O N T R O L ////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        //////////
        // What is UAC
        //////////

        private void linkHelp2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://w.wiki/AxmC");
        }

        //////////
        // Advanced Settings
        //////////

        private void checkLUA_CheckedChanged(object sender, EventArgs e)
        {
            buttonApply2.Enabled = true;
        }

        private void checkNoElev_CheckedChanged(object sender, EventArgs e)
        {
            buttonApply2.Enabled = true;
        }

        private void checkSign_CheckedChanged(object sender, EventArgs e)
        {
            buttonApply2.Enabled = true;
        }

        private void buttonApply2_Click(object sender, EventArgs e)
        {
            RegistryKey LUAKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");

            //////////
            // Disable elevation
            //////////

            if (checkNoElev.Checked == true)
            {
                LUAKey.SetValue("ConsentPromptBehaviorUser", 0);
            }
            if (checkNoElev.Checked == false)
            {
                LUAKey.SetValue("ConsentPromptBehaviorUser", 3);
            }

            //////////
            // Only signed
            //////////

            if (checkSign.Checked == true)
            {
                LUAKey.SetValue("ValidateAdminCodeSignatures", 1);
            }
            if (checkSign.Checked == false)
            {
                LUAKey.SetValue("ValidateAdminCodeSignatures", 0);
            }

            //////////
            // Disable completely
            //////////

            if (checkLUA.Checked == true)
            {
                LUAKey.SetValue("EnableLUA", 0);
                Slider.Enabled = false;
                warn.Visible = false;
            }
            if (checkLUA.Checked == false)
            {
                LUAKey.SetValue("EnableLUA", 1);
                Slider.Enabled = true;
                if (Slider.Value == 0)
                {
                    warn.Visible = true;
                }
                if (Slider.Value == 1)
                {
                    warn.Visible = false;
                }
                if (Slider.Value == 2)
                {
                    warn.Visible = false;
                }
                if (Slider.Value == 3)
                {
                    warn.Visible = false;
                }
            }

            RegistryKey LUAKey2 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");

            if (Slider.Value == 0)
            {
                LUAKey2.SetValue("ConsentPromptBehaviorAdmin", 0);
                LUAKey2.SetValue("PromptOnSecureDesktop", 0);
            }
            if (Slider.Value == 1)
            {
                LUAKey2.SetValue("ConsentPromptBehaviorAdmin", 5);
                LUAKey2.SetValue("PromptOnSecureDesktop", 0);
            }
            if (Slider.Value == 2)
            {
                LUAKey2.SetValue("ConsentPromptBehaviorAdmin", 5);
                LUAKey2.SetValue("PromptOnSecureDesktop", 1);
            }
            if (Slider.Value == 3)
            {
                LUAKey2.SetValue("ConsentPromptBehaviorAdmin", 2);
                LUAKey2.SetValue("PromptOnSecureDesktop", 1);
            }

            buttonApply2.Enabled = false;
        }

        //////////
        // UAC Settings
        //////////

        //////////
        // Slider change
        //////////

        private void Slider_Scroll(object sender, EventArgs e)
        {
            buttonApply2.Enabled = true;
            if (Slider.Value == 0)
            {
                labelUACHead.Text = "Level 0: User Account Control turned off";
                labelUACDesc.Text = "User Account Control won't bother you at all.\r\n\r\nBut be aware that your computer may become vunerable to \r\nmalware and viruses. Jack Pomi Software is NOT responsible for \r\nany of your actions.";
                warn.Visible = true;
            }
            if (Slider.Value == 1)
            {
                labelUACHead.Text = "Level 1: Programs only, no screen darkening";
                labelUACDesc.Text = "User Account Control will trigger only for programs that require its \r\npermission. In this level, there is no screen darkening.\r\n";
                warn.Visible = false;
            }
            if (Slider.Value == 2)
            {
                labelUACHead.Text = "Level 2: Programs only";
                labelUACDesc.Text = "User Account Control will trigger only for programs that require its \r\npermission. The screen will become dark when UAC is triggered.\r\n\r\nThis is the default setting in Windows 7 and later.\r\n";
                warn.Visible = false;
            }
            if (Slider.Value == 3)
            {
                labelUACHead.Text = "Level 3: User Account Control is fully enabled";
                labelUACDesc.Text = "User Account Control will trigger every time you run programs that \r\nrequire its permission or when you change some of the system's \r\nsettings. The screen will become dark when UAC is triggered.\r\n\r\nThis is the only setting available by default in Windows Vista.\r\n";
                warn.Visible = false;
            }
        }

        //////////
        // Links in menu
        //////////

        private void linkUAC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
            linkUAC.Font = new Font(linkUAC.Font.Name, linkUAC.Font.Size, FontStyle.Bold);
            linkFWA.Font = new Font(linkFWA.Font.Name, linkFWA.Font.Size, FontStyle.Regular);
        }

        private void linkDEF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage2);
            linkUAC.Font = new Font(linkUAC.Font.Name, linkUAC.Font.Size, FontStyle.Regular);
            linkFWA.Font = new Font(linkFWA.Font.Name, linkFWA.Font.Size, FontStyle.Regular);
        }

        private void linkFWA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage3);
            linkUAC.Font = new Font(linkUAC.Font.Name, linkUAC.Font.Size, FontStyle.Regular);
            linkFWA.Font = new Font(linkFWA.Font.Name, linkFWA.Font.Size, FontStyle.Bold);
        }

        /////////////////////////
        /////////////////////////
        //// F I R E W A L L ////
        /////////////////////////
        /////////////////////////

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://w.wiki/D4KE");
        }

        private bool isButton2Up = true;
        private bool isButton3Up = true;
        private bool isButton4Up = false;

        private void ChangeButtonFlags()
        {
            if (gsettingsHome.Visible == false && isButton2Up == true)
            {
                isButton2Up = !isButton2Up;
            }
            if (gsettingsPublic.Visible == false && isButton3Up == true)
            {
                isButton3Up = !isButton3Up;
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            ChangeButtonFlags();
            button2.Image = isButton2Up
                ? Properties.Resources.ButtonUpPressed
                : Properties.Resources.ButtonDownPressed;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.Image = isButton2Up
                ? Properties.Resources.ButtonUp
                : Properties.Resources.ButtonDown;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.Image = isButton3Up
                ? Properties.Resources.ButtonUpPressed
                : Properties.Resources.ButtonDownPressed;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.Image = isButton3Up
                ? Properties.Resources.ButtonUp
                : Properties.Resources.ButtonDown;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.Image = isButton4Up
                ? Properties.Resources.ButtonUpPressed
                : Properties.Resources.ButtonDownPressed;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.Image = isButton4Up
                ? Properties.Resources.ButtonUp
                : Properties.Resources.ButtonDown;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isButton2Up = !isButton2Up;
            if (gsettingsHome.Visible == true) {
                HidePrivate();
                button2.Image = Properties.Resources.ButtonDownPressed;
                return;
            }

            if (gsettingsHome.Visible == false)
            {
                gsettingsHome.Visible = true;
                button2.Image = Properties.Resources.ButtonUpPressed;
                groupPublic.Top += 70;
                gsettingsPublic.Top += 70;
                groupDomain.Top += 70;
                gsettingsDomain.Top += 70;
                buttonFWApply.Top += 70;
                labelNet.Top += 70;
                NetworkType.Top += 70;
                NetworkName.Top += 70;
                linkLabelMMC.Top += 70;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isButton3Up = !isButton3Up;
            if (gsettingsPublic.Visible == true)
            {
                HidePublic();
                button3.Image = Properties.Resources.ButtonDownPressed;
                return;
            }

            if (gsettingsPublic.Visible == false)
            {
                gsettingsPublic.Visible = true;
                button3.Image = Properties.Resources.ButtonUpPressed;
                groupDomain.Top += 70;
                gsettingsDomain.Top += 70;
                buttonFWApply.Top += 70;
                labelNet.Top += 70;
                NetworkType.Top += 70;
                NetworkName.Top += 70;
                linkLabelMMC.Top += 70;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isButton4Up = !isButton4Up;
            if (gsettingsDomain.Visible == true)
            {
                HideDomain();
                button4.Image = Properties.Resources.ButtonDownPressed;
                return;
            }

            if (gsettingsDomain.Visible == false)
            {
                gsettingsDomain.Visible = true;
                button4.Image = Properties.Resources.ButtonUpPressed;
                buttonFWApply.Top += 70;
                labelNet.Top += 70;
                NetworkType.Top += 70;
                NetworkName.Top += 70;
                linkLabelMMC.Top += 70;
            }
        }

        private void buttonFWApply_Click(object sender, EventArgs e)
        {
            if (radioPrivateON.Checked && radioPrivateON.Checked != FPri)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set privateprofile state on"; // Command to turn off the private profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FPri = !FPri;
                        StatusPanelPrivate.Image = Properties.Resources.StatusPanelOn;
                        if (checkBIPrivate.Checked)
                        {
                            StatusIconPrivate.Image = Properties.Resources.StatusIconBlock;
                        }
                        else
                        {
                            StatusIconPrivate.Image = Properties.Resources.StatusIconOn;
                        }
                        if (StatusTextPrivate.Text == "Off, not connected")
                        {
                            StatusTextPrivate.Text = "On, not connected";
                        }
                        else
                        {
                            StatusTextPrivate.Text = "On";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to turn on the firewall for Private Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (radioPrivateOFF.Checked && radioPrivateOFF.Checked == FPri)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set privateprofile state off"; // Command to turn off the private profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FPri = !FPri;
                        StatusPanelPrivate.Image = Properties.Resources.StatusPanelOff;
                        StatusIconPrivate.Image = Properties.Resources.StatusIconOff;
                        if (StatusTextPrivate.Text == "On, not connected")
                        {
                            StatusTextPrivate.Text = "Off, not connected";
                        }
                        else
                        {
                            StatusTextPrivate.Text = "Off";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to turn off the firewall for Private Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (checkBIPrivate.Checked != FBlockPri)
            {
                try
                {
                    string firewallPolicy = checkBIPrivate.Checked
                        ? "BlockInboundAlways,AllowOutbound"
                        : "BlockInbound,AllowOutbound";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set privateprofile firewallpolicy {firewallPolicy}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        FBlockPri = !FBlockPri;
                        if (checkBIPrivate.Checked && FPri == true)
                        {
                            StatusIconPrivate.Image = Properties.Resources.StatusIconBlock;
                        }

                        if (checkBIPrivate.Checked == false && FPri == true)
                        {
                            StatusIconPrivate.Image = Properties.Resources.StatusIconOn;
                        }
                    }
                    else
                    {
                        checkBIPrivate.Checked = !checkBIPrivate.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {(checkBIPrivate.Checked ? "enable" : "disable")} block all incoming connections for Private Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkBIPrivate.Checked = !checkBIPrivate.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            if (checkNPrivate.Checked != FNotifyPri)
            {
                try
                {
                    string state = checkNPrivate.Checked ? "enable" : "disable";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set privateprofile settings inboundusernotification {state}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        
                    }
                    else
                    {
                        checkNPrivate.Checked = !checkNPrivate.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {state} firewall notifications for Private Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkNPrivate.Checked = !checkNPrivate.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            if (radioPubON.Checked && radioPubON.Checked != FPub)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set publicprofile state on"; // Command to turn on the public profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FPub = !FPub;
                        StatusPanelPublic.Image = Properties.Resources.StatusPanelOn;
                        if (checkBIPublic.Checked)
                        {
                            StatusIconPublic.Image = Properties.Resources.StatusIconBlock;
                        }
                        else
                        {
                            StatusIconPublic.Image = Properties.Resources.StatusIconOn;
                        }
                        if (StatusTextPublic.Text == "Off, not connected")
                        {
                            StatusTextPublic.Text = "On, not connected";
                        }
                        else
                        {
                            StatusTextPublic.Text = "On";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to turn on the firewall for Public Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (radioPubOFF.Checked && radioPubOFF.Checked == FPub)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set publicprofile state off"; // Command to turn off the public profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FPub = !FPub;
                        StatusPanelPublic.Image = Properties.Resources.StatusPanelOff;
                        StatusIconPublic.Image = Properties.Resources.StatusIconOff;
                        if (StatusTextPublic.Text == "On, not connected")
                        {
                            StatusTextPublic.Text = "Off, not connected";
                        }
                        else
                        {
                            StatusTextPublic.Text = "Off";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to turn off the firewall for Public Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (checkBIPublic.Checked != FBlockPub)
            {
                try
                {
                    string firewallPolicy = checkBIPublic.Checked
                        ? "BlockInboundAlways,AllowOutbound"
                        : "BlockInbound,AllowOutbound";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set publicprofile firewallpolicy {firewallPolicy}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        FBlockPub = !FBlockPub;
                        if (checkBIPublic.Checked && FPub == true)
                        {
                            StatusIconPublic.Image = Properties.Resources.StatusIconBlock;
                        }

                        if (checkBIPublic.Checked == false && FPub == true)
                        {
                            StatusIconPublic.Image = Properties.Resources.StatusIconOn;
                        }
                    }
                    else
                    {
                        checkBIPublic.Checked = !checkBIPublic.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {(checkBIPublic.Checked ? "enable" : "disable")} block all incoming connections for Public Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkBIPublic.Checked = !checkBIPublic.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            if (checkNPublic.Checked != FNotifyPub)
            {
                try
                {
                    string state = checkNPublic.Checked ? "enable" : "disable";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set publicprofile settings inboundusernotification {state}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        
                    }
                    else
                    {
                        checkNPublic.Checked = !checkNPublic.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {state} firewall notifications for Public Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkNPublic.Checked = !checkNPublic.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            if (radioDomON.Checked && radioDomON.Checked != FD)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set domainprofile state on"; // Command to turn on the domain profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FD = !FD;
                        StatusPanelDomain.Image = Properties.Resources.StatusPanelOn;
                        if (checkBIDomain.Checked)
                        {
                            StatusIconDomain.Image = Properties.Resources.StatusIconBlock;
                        }
                        else
                        {
                            StatusIconDomain.Image = Properties.Resources.StatusIconOn;
                        }
                        if (StatusTextDomain.Text == "Off, not connected")
                        {
                            StatusTextDomain.Text = "On, not connected";
                        }
                        else
                        {
                            StatusTextDomain.Text = "On";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to turn on the firewall for Domain Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (radioDomOFF.Checked && radioDomOFF.Checked == FD)
            {
                try
                {
                    // Create a new process to run the command
                    Process process = new Process();
                    process.StartInfo.FileName = "netsh"; // The command-line tool
                    process.StartInfo.Arguments = "advfirewall set domainprofile state off"; // Command to turn off the domain profile firewall
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true; // Hide the command window

                    // Start the process
                    process.Start();
                    process.WaitForExit(); // Wait for the command to complete

                    // Check if the command was successful
                    if (process.ExitCode == 0)
                    {
                        FD = !FD;
                        StatusPanelDomain.Image = Properties.Resources.StatusPanelOff;
                        StatusIconDomain.Image = Properties.Resources.StatusIconOff;
                        if (StatusTextDomain.Text == "On, not connected")
                        {
                            StatusTextDomain.Text = "Off, not connected";
                        }
                        else
                        {
                            StatusTextDomain.Text = "Off";
                        }

                    }
                    else
                    {
                        MessageBox.Show("Failed to turn off the firewall for Domain Profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (checkBIDomain.Checked != FBlockD)
            {
                try
                {
                    string firewallPolicy = checkBIDomain.Checked
                        ? "BlockInboundAlways,AllowOutbound"
                        : "BlockInbound,AllowOutbound";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set domainprofile firewallpolicy {firewallPolicy}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        FBlockD = !FBlockD;
                        if (checkBIDomain.Checked && FD == true)
                        {
                            StatusIconDomain.Image = Properties.Resources.StatusIconBlock;
                        }

                        if (checkBIDomain.Checked == false && FD == true)
                        {
                            StatusIconDomain.Image = Properties.Resources.StatusIconOn;
                        }
                    }
                    else
                    {
                        checkBIDomain.Checked = !checkBIDomain.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {(checkBIDomain.Checked ? "enable" : "disable")} block all incoming connections for Domain Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkBIDomain.Checked = !checkBIDomain.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            if (checkNDomain.Checked != FNotifyD)
            {
                try
                {
                    string state = checkNDomain.Checked ? "enable" : "disable";

                    Process process = new Process();
                    process.StartInfo.FileName = "netsh";
                    process.StartInfo.Arguments = $"advfirewall set domainprofile settings inboundusernotification {state}";
                    process.StartInfo.Verb = "runas"; // Run as administrator
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        
                    }
                    else
                    {
                        checkNDomain.Checked = !checkNDomain.Checked; // Revert the checkbox
                        MessageBox.Show($"Failed to {state} firewall notifications for Domain Profile.",
                                      "Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    checkNDomain.Checked = !checkNDomain.Checked; // Revert the checkbox
                    MessageBox.Show($"An error occurred: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }

            buttonFWApply.Enabled = false;
        }

        private void linkLabelMMC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("wf.msc");
        }

        private void linkOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jackpomisoftware.github.io/uc/doc/security-center/");
        }
    }
}