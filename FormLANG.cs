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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Ultimate_Control
{
    public partial class FormLanguage : Form
    {
        public FormLanguage()
        {
            InitializeComponent();

            CheckRegistry();
        }

        private void CheckRegistry()
        {
            RegistryKey CheckKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Jack Pomi Software\Ultimate Control");
            if (CheckKey.GetValue("InterfaceLanguage").ToString() == "EN")
            {
                comboBox1.SelectedItem = "EN - English";
            }
            if (CheckKey.GetValue("InterfaceLanguage").ToString() == "RU")
            {
                comboBox1.SelectedItem = "RU - Russian (Русский)";
            }
        }
    }
}
