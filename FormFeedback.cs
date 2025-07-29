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
    public partial class FormFeedback : Form
    {
        public FormFeedback()
        {
            InitializeComponent();
        }

        private void buttonFBCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jackpomisoftware.github.io/uc");
        }

        private void linkGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JackPomiSoftware/UltimateControl");
        }
    }
}
