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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Start();

            CheckRegistry();

            buttonOpenControl.MouseEnter += new EventHandler(buttonOpenControl_MouseEnter);
            buttonOpenControl.MouseLeave += new EventHandler(buttonOpenControl_MouseLeave);

            buttonOpenPC.MouseEnter += new EventHandler(buttonOpenPC_MouseEnter);
            buttonOpenPC.MouseLeave += new EventHandler(buttonOpenPC_MouseLeave);

            buttonIER.MouseEnter += new EventHandler(buttonIER_MouseEnter);
            buttonIER.MouseLeave += new EventHandler(buttonIER_MouseLeave);

            buttonGEN.MouseEnter += new EventHandler(buttonGEN_MouseEnter);
            buttonGEN.MouseLeave += new EventHandler(buttonGEN_MouseLeave);

            buttonSEC.MouseEnter += new EventHandler(buttonSEC_MouseEnter);
            buttonSEC.MouseLeave += new EventHandler(buttonSEC_MouseLeave);

            buttonLAP.MouseEnter += new EventHandler(buttonLAP_MouseEnter);
            buttonLAP.MouseLeave += new EventHandler(buttonLAP_MouseLeave);

            buttonPER.MouseEnter += new EventHandler(buttonPER_MouseEnter);
            buttonPER.MouseLeave += new EventHandler(buttonPER_MouseLeave);

            radio1.MouseEnter += new EventHandler(radio1_MouseEnter);
            radio1.MouseLeave += new EventHandler(radio1_MouseLeave);

            //radioEXP.MouseEnter += new EventHandler(radioEXP_MouseEnter);
            //radioEXP.MouseLeave += new EventHandler(radioEXP_MouseLeave);

            radioGEN.MouseEnter += new EventHandler(radioGEN_MouseEnter);
            radioGEN.MouseLeave += new EventHandler(radioGEN_MouseLeave);

            radioIER.MouseEnter += new EventHandler(radioIER_MouseEnter);
            radioIER.MouseLeave += new EventHandler(radioIER_MouseLeave);

            radioLAP.MouseEnter += new EventHandler(radioLAP_MouseEnter);
            radioLAP.MouseLeave += new EventHandler(radioLAP_MouseLeave);

            radioPER.MouseEnter += new EventHandler(radioPER_MouseEnter);
            radioPER.MouseLeave += new EventHandler(radioPER_MouseLeave);

            radioSEC.MouseEnter += new EventHandler(radioSEC_MouseEnter);
            radioSEC.MouseLeave += new EventHandler(radioSEC_MouseLeave);
        }

        private void CheckRegistry()
        {
            RegistryKey CheckKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Jack Pomi Software\Ultimate Control");
            Console.WriteLine(CheckKey.GetValue("InterfaceLanguage"));
            if (CheckKey.GetValue("InterfaceLanguage") == null)
            {
                CheckKey.SetValue("InterfaceLanguage", "EN");
            }
            if (CheckKey.GetValue("AutoUpdateCheck") == null) 
            {
                CheckKey.SetValue("AutoUpdateCheck", 1);
            }
        }

        private void buttonOpenControl_MouseEnter(object sender, EventArgs e)
        {
            this.buttonOpenControl.BackgroundImage = Properties.Resources.button_hover_s;
        }

        private void buttonOpenControl_MouseLeave(object sender, EventArgs e)
        {
            this.buttonOpenControl.BackgroundImage = null;
        }

        private void buttonOpenPC_MouseEnter(object sender, EventArgs e)
        {
            this.buttonOpenPC.BackgroundImage = Properties.Resources.button_hover_s;
        }

        private void buttonOpenPC_MouseLeave(object sender, EventArgs e)
        {
            this.buttonOpenPC.BackgroundImage = null;
        }

        private void radio1_MouseEnter(object sender, EventArgs e)
        {
            this.radio1.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radio1_MouseLeave(object sender, EventArgs e)
        {
            this.radio1.BackgroundImage = null;
        }

        private void radioEXP_MouseEnter(object sender, EventArgs e)
        {
            this.radioEXP.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioEXP_MouseLeave(object sender, EventArgs e)
        {
            this.radioEXP.BackgroundImage = null;
        }

        private void radioGEN_MouseEnter(object sender, EventArgs e)
        {
            this.radioGEN.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioGEN_MouseLeave(object sender, EventArgs e)
        {
            this.radioGEN.BackgroundImage = null;
        }

        private void radioIER_MouseEnter(object sender, EventArgs e)
        {
            this.radioIER.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioIER_MouseLeave(object sender, EventArgs e)
        {
            this.radioIER.BackgroundImage = null;
        }

        private void radioLAP_MouseEnter(object sender, EventArgs e)
        {
            this.radioLAP.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioLAP_MouseLeave(object sender, EventArgs e)
        {
            this.radioLAP.BackgroundImage = null;
        }

        private void radioPER_MouseEnter(object sender, EventArgs e)
        {
            this.radioPER.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioPER_MouseLeave(object sender, EventArgs e)
        {
            this.radioPER.BackgroundImage = null;
        }

        private void radioSEC_MouseEnter(object sender, EventArgs e)
        {
            this.radioSEC.BackgroundImage = Properties.Resources.SectionHover;
        }

        private void radioSEC_MouseLeave(object sender, EventArgs e)
        {
            this.radioSEC.BackgroundImage = null;
        }

        private void buttonOpenControl_Click(object sender, EventArgs e)
        {
            this.buttonOpenControl.BackgroundImage = Properties.Resources.button_pressed_s;
            System.Diagnostics.Process.Start("control");
        }

        private void buttonOpenPC_Click(object sender, EventArgs e)
        {
            this.buttonOpenPC.BackgroundImage = Properties.Resources.button_pressed_s;
            System.Diagnostics.Process.Start("ms-settings:");
        }

        private void radio1_CheckedChanged(object sender, EventArgs e)
        {
            if (radio1.Checked == true)
            {
                PanelPCOpen.Visible = true;
                WelcomePageLbl.Visible = true;
                HeaderTitle.Text = "Ultimate Control — home page";
                HeaderText.Text = "• Augments standard Windows settings by adding more functionality\r\n• Based on previous Jack Pomi Software programs\r\n\r\nClick on any of the options below to begin";
                HeaderIcon.Image = Properties.Resources.ChoiceHome;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = true;
            }
        }

        private void radioIER_CheckedChanged(object sender, EventArgs e)
        {
            if (radioIER.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Internet Explorer settings";
                HeaderText.Text = "• Re-enable Internet Explorer in Windows 10 and 11\r\n• Choose the browser's home page and startup options\r\n• Set Internet Explorer as your default web browser";
                HeaderIcon.Image = Properties.Resources.ChoiceIER;
                PanelOpenIE.Visible = true;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = false;
            }
        }

        private void radioEXP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioEXP.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Explorer and Taskbar";
                HeaderText.Text = "• Change File Explorer settings\r\n• Personalize the taskbar";
                HeaderIcon.Image = Properties.Resources.ChoiceExp;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = false;
            }
        }

        private void radioGEN_CheckedChanged(object sender, EventArgs e)
        {
            if (radioGEN.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Genuine Center";
                HeaderText.Text = "• Check whether your Windows copy is activated\r\n• See the full product key of your Windows copy\r\n• Read the license agreement";
                HeaderIcon.Image = Properties.Resources.ChoiceISG;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = true;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = false;
            }
        }

        private void radioSEC_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSEC.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Security Center";
                HeaderText.Text = "• Change User Account Control settings or turn it off completely\r\n• Set up Windows Firewall settings";
                HeaderIcon.Image = Properties.Resources.ChoiceSec;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = true;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = false;
            }
        }

        private void radioPER_CheckedChanged(object sender, EventArgs e)
        {
            if (radioPER.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Personalisation";
                HeaderText.Text = "• Change desktop background image\r\n• Change the accent color of windows\r\n• Set up the screen saver\r\n• Personalize the taskbar";
                HeaderIcon.Image = Properties.Resources.ChoicePer;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = false;
                PanelOpenPER.Visible = true;
                piсLogo.Visible = false;
            }
        }

        private void radioLAP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLAP.Checked == true)
            {
                PanelPCOpen.Visible = false;
                WelcomePageLbl.Visible = false;
                HeaderTitle.Text = "Laptop Center";
                HeaderText.Text = "Provides settings exclusive for portable computers, such as:\r\n• Change battery mode\r\n• Change display brightness\r\n• Set up presentation mode\r\n• Connect to an external display";
                HeaderIcon.Image = Properties.Resources.ChoiceLap;
                PanelOpenIE.Visible = false;
                PanelOpenGEN.Visible = false;
                PanelOpenSEC.Visible = false;
                PanelOpenLAP.Visible = true;
                PanelOpenPER.Visible = false;
                piсLogo.Visible = false;
            }
        }

        private void buttonIER_MouseEnter(object sender, EventArgs e)
        {
            this.buttonIER.BackgroundImage = Properties.Resources.button_hover_l;
        }

        private void buttonIER_MouseLeave(object sender, EventArgs e)
        {
            this.buttonIER.BackgroundImage = null;
        }

        private void buttonIER_Click(object sender, EventArgs e)
        {
            var AForm = new FormIER();
            AForm.Show();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            CTMHelp.Show(ptLowerLeft);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var AForm = new FormAbout();
            AForm.Show();
        }

        private void visitOnlineHelpCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ultimatecontrol.github.io/doc/");
        }

        private void sendFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var AForm = new FormFeedback();
            AForm.Show();
        }

        private void buttonGEN_Click(object sender, EventArgs e)
        {
            var AForm = new FormGEN();
            AForm.Show();
            this.buttonGEN.BackgroundImage = Properties.Resources.button_pressed_ms;
        }
        private void buttonGEN_MouseEnter(object sender, EventArgs e)
        {
            this.buttonGEN.BackgroundImage = Properties.Resources.button_hover_ms;
        }

        private void buttonGEN_MouseLeave(object sender, EventArgs e)
        {
            this.buttonGEN.BackgroundImage = null;
        }

        private void buttonSEC_Click(object sender, EventArgs e)
        {
            var AForm = new FormSEC();
            AForm.Show();
            this.buttonSEC.BackgroundImage = Properties.Resources.button_pressed_m;
        }

        private void buttonSEC_MouseEnter(object sender, EventArgs e)
        {
            this.buttonSEC.BackgroundImage = Properties.Resources.button_hover_m;
        }

        private void buttonSEC_MouseLeave(object sender, EventArgs e)
        {
            this.buttonSEC.BackgroundImage = null;
        }

        private void notifyFeedback_BalloonTipClicked(object sender, EventArgs e)
        {
            var AForm = new FormFeedback();
            AForm.Show();
            notifyFeedback.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            notifyFeedback.ShowBalloonTip(3000);
        }

        private void buttonLAP_Click(object sender, EventArgs e)
        {
            var AForm = new FormLAP();
            AForm.Show();
        }

        private void buttonLAP_MouseEnter(object sender, EventArgs e)
        {
            this.buttonLAP.BackgroundImage = Properties.Resources.button_hover_ms;
        }

        private void buttonLAP_MouseLeave(object sender, EventArgs e)
        {
            this.buttonLAP.BackgroundImage = null;
        }

        private void buttonPER_Click(object sender, EventArgs e)
        {
            var AForm = new FormPER();
            AForm.Show();
        }

        private void buttonPER_MouseEnter(object sender, EventArgs e)
        {
            this.buttonPER.BackgroundImage = Properties.Resources.button_hover_ms;
        }

        private void buttonPER_MouseLeave(object sender, EventArgs e)
        {
            this.buttonPER.BackgroundImage = null;
        }

        private void changeLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var AForm = new FormLanguage();
            AForm.Show();
        }
    }
}