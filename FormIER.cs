﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Claims;

namespace Ultimate_Control
{
    public partial class FormIER : Form
    {
        public FormIER()
        {
            InitializeComponent();

            try
            {
                string mainPage = "";
                string[] secondaryPages = Array.Empty<string>();

                using (RegistryKey ieKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", false))
                {
                    if (ieKey != null)
                    {
                        mainPage = ieKey.GetValue("Start Page", "").ToString();
                        object secValue = ieKey.GetValue("Secondary Start Pages");
                        if (secValue is string[])
                            secondaryPages = (string[])secValue;
                    }
                }

                // Combine all into textbox
                HomPag.Lines = new[] { mainPage }.Concat(secondaryPages).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load IE home pages: " + ex.Message);
            }

            RegistryKey BHOKeySearch = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Ext\CLSID");
            string BHOKey = BHOKeySearch.GetValue("{1FD49718-1D00-4B19-AF5F-070AF6D5D54C}").ToString();
            if (BHOKey != "0")
            {
                BHOon.Checked = true;
            }
            else
            {
                BHOoff.Checked = true;
            }

            RegistryKey StartIfKeySearch1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer");
            if (StartIfKeySearch1 == null)
            {
                StartHome.Checked = false;
                StartTabs.Checked = false;
            }
            else
            {
                RegistryKey StartIfKeySearch2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                if (StartIfKeySearch2 == null)
                {
                    StartHome.Checked = false;
                    StartTabs.Checked = false;
                }
                else
                {
                    RegistryKey StartIfKeySearch3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                    var StartKey3 = StartIfKeySearch3.GetValue("Enabled");
                    if (StartKey3 == null)
                    {
                        StartHome.Checked = false;
                        StartTabs.Checked = false;
                    }
                    else
                    {
                        RegistryKey StartKeySearch = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                        string StartKey = StartKeySearch.GetValue("Enabled").ToString();
                        if (StartKey == "0")
                        {
                            StartHome.Checked = true;
                        }
                        if (StartKey == "1")
                        {
                            StartTabs.Checked = true;
                        }
                    }
                }
            }
            ApplyStartup.Enabled = false;
            applyBHO.Enabled = false;
            StartUser.SelectedItem = "For current user";

            RegistryKey DefHTTPS = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice");
            RegistryKey DefHTTP = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            if ((DefHTTPS.GetValue("ProgId").ToString() != "Id.HTTPS") && (DefHTTP.GetValue("ProgID").ToString() != "IE.HTTP"))
            {
                DefStatus.Text = "Internet Explorer is not currently your default web browser.";
                DefStatus.ForeColor = Color.Firebrick;
                LinksChoose.Enabled = false;
                TilesOpen.Enabled = false;
            }
            else
            {
                DefStatus.Text = "Internet Explorer is currently your default web browser.";
                DefStatus.ForeColor = Color.Green;
                SetDef.Enabled = false;
                LinksChoose.Enabled = true;
                TilesOpen.Enabled = true;
            }

            RegistryKey DefLinksOpen = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main");
            if (DefLinksOpen.GetValue("AssociationActivationMode") != null)
            {
                if (DefLinksOpen.GetValue("AssociationActivationMode").ToString() == "0")
                {
                    LinksChoose.SelectedItem = "Let Internet Explorer decide";
                }
                if (DefLinksOpen.GetValue("AssociationActivationMode").ToString() == "1")
                {
                    LinksChoose.SelectedItem = "Always in Internet Explorer";
                }
                if (DefLinksOpen.GetValue("AssociationActivationMode").ToString() == "2")
                {
                    LinksChoose.SelectedItem = "Always in Internet Explorer on the desktop";
                }
            }
            if (DefLinksOpen.GetValue("AssociationActivationMode") == null)
            {
                LinksChoose.SelectedItem = "(not selected)";
            }
            RegistryKey TilesOpenSearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main");
            if (TilesOpenSearch.GetValue("ApplicationTileImmersiveActivation") == null)
            {
                TilesOpen.Checked = true;
            }
            else
            {
                if (TilesOpenSearch.GetValue("ApplicationTileImmersiveActivation").ToString() == "1")
                {
                    TilesOpen.Checked = false;
                }
                if (TilesOpenSearch.GetValue("ApplicationTileImmersiveActivation").ToString() == "0")
                {
                    TilesOpen.Checked = true;
                }
            }

            LinksApply.Enabled = false;
        }

        private void EDopen_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("msedge.exe");

        }
        private void IEopen_Click(object sender, EventArgs e)
        {
            try
            {
                Type ieType = Type.GetTypeFromProgID("InternetExplorer.Application");
                dynamic ie = Activator.CreateInstance(ieType);
                ie.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Internet Explorer: " + ex.Message);
            }
        }

        private void IEexe_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe");
        }

        private void applyBHO_Click(object sender, EventArgs e)
        {
            if (BHOon.Checked == true)
            {
                RegistryKey BHOKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Ext\CLSID", true);
                BHOKey.SetValue("{1FD49718-1D00-4B19-AF5F-070AF6D5D54C}", "1");
                //System.Diagnostics.Process.Start("UseEdge.reg");
            }
            if (BHOoff.Checked == true)
            {
                RegistryKey BHOKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Ext\CLSID", true);
                BHOKey.SetValue("{1FD49718-1D00-4B19-AF5F-070AF6D5D54C}", "0");
                //System.Diagnostics.Process.Start("UseIE.reg");
            }
            applyBHO.Enabled = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JackPomiSoftware/IEReturn");
        }

        private void usedef_Click(object sender, EventArgs e)
        {
            RegistryKey startPageKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
            startPageKey.SetValue("Start Page", "https://www.msn.com");
            startPageKey.Close();
            RegistryKey startPageKey1 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main");
            HomPag.Text = (string)startPageKey1.GetValue("Start Page");
        }

        private void usebla_Click(object sender, EventArgs e)
        {
            RegistryKey startPageKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true);
            startPageKey.SetValue("Start Page", "about:blank");
            startPageKey.Close();
            RegistryKey startPageKey1 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main");
            HomPag.Text = (string)startPageKey1.GetValue("Start Page");
        }

        private void usecur_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = HomPag.Lines
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrEmpty(l))
                    .ToArray();

                if (lines.Length == 0)
                {
                    MessageBox.Show("Please enter at least one home page.");
                    return;
                }

                using (RegistryKey ieKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main", true))
                {
                    if (ieKey != null)
                    {
                        // First line = Start Page
                        ieKey.SetValue("Start Page", lines[0]);

                        // Rest = Secondary Start Pages
                        string[] secondary = lines.Skip(1).ToArray();
                        ieKey.SetValue("Secondary Start Pages", secondary, RegistryValueKind.MultiString);
                    }
                }

                MessageBox.Show("Home pages updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to apply IE home pages: " + ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jackpomisoftware.github.io/uc/doc/ie-settings/");
        }

        private void BHOon_CheckedChanged(object sender, EventArgs e)
        {
            applyBHO.Enabled = true;
        }

        private void BHOoff_CheckedChanged(object sender, EventArgs e)
        {
            applyBHO.Enabled = true;
        }

        private void SetDef_Click(object sender, EventArgs e)
        {
            RegistryKey DefHTTPS = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice");
            RegistryKey DefHTTP = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            DefHTTP.SetValue("ProgID", "IE.HTTP");
            DefHTTP.SetValue("Hash", "+rzcSQ6YiCY=");
            DefHTTP.Close();
            DefHTTPS.SetValue("ProgID", "IE.HTTPS");
            DefHTTPS.SetValue("Hash", "CwgHvexaWtU=");
            DefHTTPS.Close();
            SetDef.Enabled = false;
            DefStatus.Text = "Internet Explorer is currently your default web browser.";
            DefStatus.ForeColor = Color.Green;
            LinksChoose.Enabled = true;
            TilesOpen.Enabled = true;
        }

        private void DefRef_Click(object sender, EventArgs e)
        {
            RegistryKey DefHTTPS = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice");
            RegistryKey DefHTTP = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            if ((DefHTTPS.GetValue("ProgId").ToString() != "Id.HTTPS") && (DefHTTP.GetValue("ProgID").ToString() != "IE.HTTP"))
            {
                DefStatus.Text = "Internet Explorer is not currently your default web browser.";
                DefStatus.ForeColor = Color.Firebrick;
                SetDef.Enabled = true;
                LinksChoose.Enabled = false;
                TilesOpen.Enabled = false;
            }
            else
            {
                DefStatus.Text = "Internet Explorer is currently your default web browser.";
                DefStatus.ForeColor = Color.Green;
                SetDef.Enabled = false;
                LinksChoose.Enabled = true;
                TilesOpen.Enabled = true;
            }
        }

        private void LinksApply_Click(object sender, EventArgs e)
        {
            RegistryKey DefLinksOpen = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main");
            if (LinksChoose.SelectedItem == "Let Internet Explorer decide")
            {
                DefLinksOpen.SetValue("AssociationActivationMode", 0);
            }
            if (LinksChoose.SelectedItem == "Always in Internet Explorer")
            {
                DefLinksOpen.SetValue("AssociationActivationMode", 1);
            }
            if (LinksChoose.SelectedItem == "Always in Internet Explorer on the desktop")
            {
                DefLinksOpen.SetValue("AssociationActivationMode", 2);
            }
            if ((LinksChoose.SelectedItem == "(not selected)") && (DefLinksOpen.GetValue("AssociationActivationMode") != null))
            {
                DefLinksOpen.DeleteValue("AssociationActivationMode");
            }

            RegistryKey TilesOpenSearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main");
            if (TilesOpen.Checked == true)
            {
                TilesOpenSearch.SetValue("ApplicationTileImmersiveActivation", 0, RegistryValueKind.DWord);
            }
            if (TilesOpen.Checked == false)
            {
                TilesOpenSearch.SetValue("ApplicationTileImmersiveActivation", 1, RegistryValueKind.DWord);
            }
            LinksApply.Enabled = false;
        }

        private void LinksChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            LinksApply.Enabled = true;
        }

        private void TilesOpen_CheckedChanged(object sender, EventArgs e)
        {
            LinksApply.Enabled = true;
        }

        private void inetcpl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("inetcpl.cpl");
        }

        private void StartUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartUser.SelectedItem == "For all users")
            {
                RegistryKey StartIfKeySearch1 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer");
                if (StartIfKeySearch1 == null)
                {
                    StartHome.Checked = false;
                    StartTabs.Checked = false;
                    StartNA.Checked = true;
                }
                else
                {
                    RegistryKey StartIfKeySearch2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                    if (StartIfKeySearch2 == null)
                    {
                        StartHome.Checked = false;
                        StartTabs.Checked = false;
                        StartNA.Checked = true;
                    }
                    else
                    {
                        RegistryKey StartIfKeySearch3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                        var StartKey3 = StartIfKeySearch3.GetValue("Enabled");
                        if (StartKey3 == null)
                        {
                            StartHome.Checked = false;
                            StartTabs.Checked = false;
                            StartNA.Checked = true;
                        }
                        else
                        {
                            RegistryKey StartKeySearch = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                            string StartKey = StartKeySearch.GetValue("Enabled").ToString();
                            if (StartKey == "0")
                            {
                                StartHome.Checked = true;
                            }
                            if (StartKey == "1")
                            {
                                StartTabs.Checked = true;
                            }
                        }
                    }
                }
                StartNA.Visible = true;
            }
            else
            {
                RegistryKey StartIfKeySearch3 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\ContinuousBrowsing");
                if (StartIfKeySearch3 != null) 
                {
                    var StartKey3 = StartIfKeySearch3.GetValue("Enabled");
                    if (StartKey3 == null)
                    {
                        StartHome.Checked = false;
                        StartTabs.Checked = false;
                    }
                    else
                    {
                        RegistryKey StartKeySearch = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\ContinuousBrowsing");
                        if (StartKeySearch.GetValue("Enabled") != null)
                        {
                            string StartKey = StartKeySearch.GetValue("Enabled").ToString();
                            if (StartKey == "0")
                            {
                                StartHome.Checked = true;
                            }
                            if (StartKey == "1")
                            {
                                StartTabs.Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    StartHome.Checked = false;
                    StartTabs.Checked = false;
                }
                StartNA.Visible = false;
            }
        }

        private void ApplyStartup_Click(object sender, EventArgs e)
        {
            if (StartUser.SelectedItem == "For all users")
            {
                if (StartHome.Checked == true)
                {
                    RegistryKey StartKeySearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                    StartKeySearch.SetValue("Enabled", "0");
                    StartKeySearch.Close();
                }
                if (StartTabs.Checked == true)
                {
                    RegistryKey StartKeySearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                    StartKeySearch.SetValue("Enabled", "1");
                    StartKeySearch.Close();
                }
                if (StartNA.Checked == true)
                {
                    RegistryKey StartKeySearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Internet Explorer\ContinuousBrowsing");
                    StartKeySearch.DeleteValue("Enabled");
                    StartKeySearch.Close();
                }
            }
            else
            {
                if (StartHome.Checked == true)
                {
                    RegistryKey StartKeySearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\ContinuousBrowsing");
                    StartKeySearch.SetValue("Enabled", "0");
                    StartKeySearch.Close();
                }
                if (StartTabs.Checked == true)
                {
                    RegistryKey StartKeySearch = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\ContinuousBrowsing");
                    StartKeySearch.SetValue("Enabled", "1");
                    StartKeySearch.Close();
                }
            }
            ApplyStartup.Enabled = false;
        }

        private void StartTabs_CheckedChanged(object sender, EventArgs e)
        {
            ApplyStartup.Enabled = true;
        }

        private void StartHome_CheckedChanged(object sender, EventArgs e)
        {
            ApplyStartup.Enabled = true;
        }

    }
}
