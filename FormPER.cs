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
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;

namespace Ultimate_Control
{
    public partial class FormPER : Form
    {
        // WinAPI import
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(
            int uAction, int uParam,
            StringBuilder lpvParam, int fuWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_GETDESKWALLPAPER = 0x0073;
        private const int MAX_PATH = 260;
        const int SPI_SETDESKWALLPAPER = 0x0014;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;
        const string DWM_PATH = @"Software\Microsoft\Windows\DWM";
        const string ACCENT_PATH = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent";
        const string THEME_PATH = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        public FormPER()
        {
            InitializeComponent();
            LoadTaskbarSettings();
            checkTaskLock.CheckedChanged += TaskbarSettingChanged;
            checkTaskHideD.CheckedChanged += TaskbarSettingChanged;
            checkTaskHideT.CheckedChanged += TaskbarSettingChanged;
            checkTaskSmall.CheckedChanged += TaskbarSettingChanged;
            checkTaskPower.CheckedChanged += TaskbarSettingChanged;
            checkTaskPeek.CheckedChanged += TaskbarSettingChanged;
            checkTaskBadge.CheckedChanged += TaskbarSettingChanged;
            checkTaskSeconds.CheckedChanged += TaskbarSettingChanged;
            checkTaskChat.CheckedChanged += TaskbarSettingChanged;
            checkTaskCortana.CheckedChanged += TaskbarSettingChanged;
            checkTaskDesk.CheckedChanged += TaskbarSettingChanged;
            checkTaskFlash.CheckedChanged += TaskbarSettingChanged;
            checkTaskPeople.CheckedChanged += TaskbarSettingChanged;
            checkTaskView.CheckedChanged += TaskbarSettingChanged;
            checkTaskWidgets.CheckedChanged += TaskbarSettingChanged;

            comboTaskLoc.SelectedIndexChanged += TaskbarSettingChanged;
            comboTaskCombine.SelectedIndexChanged += TaskbarSettingChanged;
            comboTaskAlign.SelectedIndexChanged += TaskbarSettingChanged;
            comboTaskSearch.SelectedIndexChanged += TaskbarSettingChanged;

            LoadCurrentWallpaperAndStyle();
            //LoadPictureModeSamples();

            LoadColorSetting();

            PopulateWallpaperColors();

            LoadScreensavers();
            buttonScrApply.Enabled = false;

            string scrName = comboScreensaver.SelectedItem.ToString();
            string scrPath = Path.Combine(Environment.SystemDirectory, scrName + ".scr");

            if (File.Exists(scrPath))
            ShowScreensaverPreview(scrPath);

            Color currentAccent = GetCurrentAccentColor();
            CurrentColor.BackColor = currentAccent;

            colorSky.Tag = ColorTranslator.FromHtml("#74B8FC");
            colorTwilight.Tag = ColorTranslator.FromHtml("#0045AC");
            colorSea.Tag = ColorTranslator.FromHtml("#31CECE");
            colorLeaf.Tag = ColorTranslator.FromHtml("#15A600");
            colorGrass.Tag = ColorTranslator.FromHtml("#98D937");
            colorSun.Tag = ColorTranslator.FromHtml("#FADC0E");
            colorPumpkin.Tag = ColorTranslator.FromHtml("#FF9900");
            colorRuby.Tag = ColorTranslator.FromHtml("#CE0F0F");
            colorFucsia.Tag = ColorTranslator.FromHtml("#FF0099");
            colorPink.Tag = ColorTranslator.FromHtml("#FCC7F8");
            colorViolet.Tag = ColorTranslator.FromHtml("#6F3CA2");
            colorLavender.Tag = ColorTranslator.FromHtml("#8C5A94");
            colorPalebrown.Tag = ColorTranslator.FromHtml("#95814A");
            colorChocolate.Tag = ColorTranslator.FromHtml("#501B1B");
            colorGray.Tag = ColorTranslator.FromHtml("#555555");
            colorFrost.Tag = ColorTranslator.FromHtml("#FBFBFB");

            colorSky.Click += ColorBox_Click;
            colorTwilight.Click += ColorBox_Click;
            colorSea.Click += ColorBox_Click;
            colorLeaf.Click += ColorBox_Click;
            colorGrass.Click += ColorBox_Click;
            colorSun.Click += ColorBox_Click;
            colorPumpkin.Click += ColorBox_Click;
            colorRuby.Click += ColorBox_Click;
            colorFucsia.Click += ColorBox_Click;
            colorPink.Click += ColorBox_Click;
            colorViolet.Click += ColorBox_Click;
            colorLavender.Click += ColorBox_Click;
            colorPalebrown.Click += ColorBox_Click;
            colorChocolate.Click += ColorBox_Click;
            colorGray.Click += ColorBox_Click;
            colorFrost.Click += ColorBox_Click;

            colorSky.MouseEnter += new EventHandler(colorSky_MouseEnter);
            colorSky.MouseLeave += new EventHandler(colorSky_MouseLeave);
            colorTwilight.MouseEnter += new EventHandler(colorTwilight_MouseEnter);
            colorTwilight.MouseLeave += new EventHandler(colorTwilight_MouseLeave);
            colorSea.MouseEnter += new EventHandler(colorSea_MouseEnter);
            colorSea.MouseLeave += new EventHandler(colorSea_MouseLeave);
            colorLeaf.MouseEnter += new EventHandler(colorLeaf_MouseEnter);
            colorLeaf.MouseLeave += new EventHandler(colorLeaf_MouseLeave);
            colorGrass.MouseEnter += new EventHandler(colorGrass_MouseEnter);
            colorGrass.MouseLeave += new EventHandler(colorGrass_MouseLeave);
            colorSun.MouseEnter += new EventHandler(colorSun_MouseEnter);
            colorSun.MouseLeave += new EventHandler(colorSun_MouseLeave);
            colorPumpkin.MouseEnter += new EventHandler(colorPumpkin_MouseEnter);
            colorPumpkin.MouseLeave += new EventHandler(colorPumpkin_MouseLeave);
            colorRuby.MouseEnter += new EventHandler(colorRuby_MouseEnter);
            colorRuby.MouseLeave += new EventHandler(colorRuby_MouseLeave);

            colorFucsia.MouseEnter += new EventHandler(colorFucsia_MouseEnter);
            colorFucsia.MouseLeave += new EventHandler(colorFucsia_MouseLeave);
            colorPink.MouseEnter += new EventHandler(colorPink_MouseEnter);
            colorPink.MouseLeave += new EventHandler(colorPink_MouseLeave);
            colorViolet.MouseEnter += new EventHandler(colorViolet_MouseEnter);
            colorViolet.MouseLeave += new EventHandler(colorViolet_MouseLeave);
            colorLavender.MouseEnter += new EventHandler(colorLavender_MouseEnter);
            colorLavender.MouseLeave += new EventHandler(colorLavender_MouseLeave);
            colorPalebrown.MouseEnter += new EventHandler(colorPalebrown_MouseEnter);
            colorPalebrown.MouseLeave += new EventHandler(colorPalebrown_MouseLeave);
            colorChocolate.MouseEnter += new EventHandler(colorChocolate_MouseEnter);
            colorChocolate.MouseLeave += new EventHandler(colorChocolate_MouseLeave);
            colorGray.MouseEnter += new EventHandler(colorGray_MouseEnter);
            colorGray.MouseLeave += new EventHandler(colorGray_MouseLeave);
            colorFrost.MouseEnter += new EventHandler(colorFrost_MouseEnter);
            colorFrost.MouseLeave += new EventHandler(colorFrost_MouseLeave);

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is PictureBox pic && pic.Name.StartsWith("color"))
                {
                    pic.Click += new EventHandler(ColorBox_Click);
                }
            }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Lock Screen\Creative"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("RotatingLockScreenEnabled");
                        if (value is int intValue)
                        {
                            if (intValue == 1)
                            {
                                comboBoxLock.SelectedItem = "Windows Spotlight";
                                ShowSpotlightNotAvailable();
                            }
                            else
                            {
                                comboBoxLock.SelectedItem = "Pictures";
                                //LoadPictureModeSamples();
                            }
                        }
                        else
                        {
                            // Default fallback
                            comboBoxLock.SelectedItem = "Pictures";
                            //LoadPictureModeSamples();
                        }
                    }
                    else
                    {
                        // Key doesn't exist — assume picture mode
                        comboBoxLock.SelectedItem = "Pictures";
                        //LoadPictureModeSamples();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read lock screen mode: " + ex.Message);
                comboBoxLock.SelectedItem = "Pictures";
                //LoadPictureModeSamples();
            }
        }

        private void heckTaskSeconds_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void colorSky_MouseEnter(object sender, EventArgs e)
        {
            this.colorSky.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorSky_MouseLeave(object sender, EventArgs e)
        {
            this.colorSky.BackgroundImage = null;
        }

        private void colorTwilight_MouseEnter(object sender, EventArgs e)
        {
            this.colorTwilight.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorTwilight_MouseLeave(object sender, EventArgs e)
        {
            this.colorTwilight.BackgroundImage = null;
        }

        private void colorSea_MouseEnter(object sender, EventArgs e)
        {
            this.colorSea.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorSea_MouseLeave(object sender, EventArgs e)
        {
            this.colorSea.BackgroundImage = null;
        }

        private void colorLeaf_MouseEnter(object sender, EventArgs e)
        {
            this.colorLeaf.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorLeaf_MouseLeave(object sender, EventArgs e)
        {
            this.colorLeaf.BackgroundImage = null;
        }

        private void colorGrass_MouseEnter(object sender, EventArgs e)
        {
            this.colorGrass.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorGrass_MouseLeave(object sender, EventArgs e)
        {
            this.colorGrass.BackgroundImage = null;
        }

        private void colorSun_MouseEnter(object sender, EventArgs e)
        {
            this.colorSun.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorSun_MouseLeave(object sender, EventArgs e)
        {
            this.colorSun.BackgroundImage = null;
        }

        private void colorPumpkin_MouseEnter(object sender, EventArgs e)
        {
            this.colorPumpkin.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorPumpkin_MouseLeave(object sender, EventArgs e)
        {
            this.colorPumpkin.BackgroundImage = null;
        }

        private void colorRuby_MouseEnter(object sender, EventArgs e)
        {
            this.colorRuby.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorRuby_MouseLeave(object sender, EventArgs e)
        {
            this.colorRuby.BackgroundImage = null;
        }

        private void colorFucsia_MouseEnter(object sender, EventArgs e)
        {
            this.colorFucsia.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorFucsia_MouseLeave(object sender, EventArgs e)
        {
            this.colorFucsia.BackgroundImage = null;
        }

        private void colorPink_MouseEnter(object sender, EventArgs e)
        {
            this.colorPink.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorPink_MouseLeave(object sender, EventArgs e)
        {
            this.colorPink.BackgroundImage = null;
        }

        private void colorViolet_MouseEnter(object sender, EventArgs e)
        {
            this.colorViolet.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorViolet_MouseLeave(object sender, EventArgs e)
        {
            this.colorViolet.BackgroundImage = null;
        }

        private void colorLavender_MouseEnter(object sender, EventArgs e)
        {
            this.colorLavender.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorLavender_MouseLeave(object sender, EventArgs e)
        {
            this.colorLavender.BackgroundImage = null;
        }

        private void colorPalebrown_MouseEnter(object sender, EventArgs e)
        {
            this.colorPalebrown.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorPalebrown_MouseLeave(object sender, EventArgs e)
        {
            this.colorPalebrown.BackgroundImage = null;
        }

        private void colorChocolate_MouseEnter(object sender, EventArgs e)
        {
            this.colorChocolate.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorChocolate_MouseLeave(object sender, EventArgs e)
        {
            this.colorChocolate.BackgroundImage = null;
        }

        private void colorGray_MouseEnter(object sender, EventArgs e)
        {
            this.colorGray.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorGray_MouseLeave(object sender, EventArgs e)
        {
            this.colorGray.BackgroundImage = null;
        }

        private void colorFrost_MouseEnter(object sender, EventArgs e)
        {
            this.colorFrost.BackgroundImage = Properties.Resources.ColorBack;
        }

        private void colorFrost_MouseLeave(object sender, EventArgs e)
        {
            this.colorFrost.BackgroundImage = null;
        }

        private void LoadColorSetting()
        {
            const string personalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string dwmPath = @"Software\Microsoft\Windows\DWM";

            // Load EnableTransparency and ColorPrevalence (taskbar/start) from Personalize
            using (var personalizeKey = Registry.CurrentUser.OpenSubKey(personalizePath))
            {
                if (personalizeKey != null)
                {
                    // EnableTransparency
                    var transparencyVal = personalizeKey.GetValue("EnableTransparency");
                    if (transparencyVal is int transparency)
                        checkBoxTransparency.Checked = transparency == 1;
                    else
                        checkBoxTransparency.Checked = true; // default

                    // ColorPrevalence for taskbar/start
                    var colorPrevalenceVal = personalizeKey.GetValue("ColorPrevalence");
                    if (colorPrevalenceVal is int cp)
                        checkBoxColorStart.Checked = cp == 1;
                    else
                        checkBoxColorStart.Checked = true; // default
                }
                else
                {
                    // Defaults if key missing
                    checkBoxTransparency.Checked = true;
                    checkBoxColorStart.Checked = true;
                }
            }

            // Load ColorPrevalence for title bars/window borders from DWM
            using (var dwmKey = Registry.CurrentUser.OpenSubKey(dwmPath))
            {
                if (dwmKey != null)
                {
                    var colorPrevalenceVal = dwmKey.GetValue("ColorPrevalence");
                    if (colorPrevalenceVal is int cp)
                        checkBoxColorTitle.Checked = cp == 1;
                    else
                        checkBoxColorTitle.Checked = true; // default
                }
                else
                {
                    checkBoxColorTitle.Checked = true;
                }
            }

            using (var personalizeKey = Registry.CurrentUser.OpenSubKey(personalizePath))
            {
                if (personalizeKey != null)
                {
                    var appsUseLight = personalizeKey.GetValue("AppsUseLightTheme") as int?;
                    var systemUseLight = personalizeKey.GetValue("SystemUsesLightTheme") as int?;

                    // Set comboModeWin (System UI)
                    if (systemUseLight == 1) comboModeWin.SelectedIndex = 1;
                    else if (systemUseLight == 0) comboModeWin.SelectedIndex = 0;

                    // Set comboModeApp (Apps)
                    if (appsUseLight == 1) comboModeApp.SelectedIndex = 1;
                    else if (appsUseLight == 0) comboModeApp.SelectedIndex = 0;

                    // Optional: Set comboMode if you use "System Default / Light / Dark"
                    if (appsUseLight == systemUseLight)
                    {
                        comboMode.SelectedIndex = appsUseLight == 1 ? 1 : 0; // 0 = Light, 1 = Dark
                    }
                    else
                    {
                        comboMode.SelectedIndex = 2; // System Default (mixed)
                        comboModeApp.Visible = true;
                        labelModeApp.Visible = true;
                        comboModeWin.Visible = true;
                        labelModeWin.Visible = true;
                    }
                }
            }
        }

        private Color GetCurrentAccentColor()
        {
            const string accentKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent";
            const string valueName = "AccentColor";

            using (var key = Registry.CurrentUser.OpenSubKey(accentKeyPath))
            {
                if (key != null)
                {
                    var val = key.GetValue(valueName);
                    if (val != null && val is int)
                    {
                        int abgr = (int)val;

                        // Windows stores color as ABGR in DWORD
                        byte a = (byte)((abgr >> 24) & 0xFF);
                        byte b = (byte)((abgr >> 16) & 0xFF);
                        byte g = (byte)((abgr >> 8) & 0xFF);
                        byte r = (byte)(abgr & 0xFF);

                        return Color.FromArgb(a, r, g, b);
                    }
                }
            }

            // Fallback to a default color if key not found
            return SystemColors.Highlight;
        }

        private void ColorBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pic && pic.Tag is Color color)
            {
                ApplyAccentColor(color);
            }
        }

        private void ApplyAccentColor(Color color)
        {
            int abgr = (color.A << 24) | (color.B << 16) | (color.G << 8) | color.R;

            // Update Personalize keys
            using (var personalize = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true))
            {
                personalize?.SetValue("ColorPrevalence", 1, RegistryValueKind.DWord);
            }

            // Update Accent keys
            using (var accent = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", true))
            {
                accent?.SetValue("AccentColor", abgr, RegistryValueKind.DWord);
                accent?.SetValue("AccentColorMenu", abgr, RegistryValueKind.DWord);
                accent?.SetValue("StartColorMenu", abgr, RegistryValueKind.DWord);
            }

            // Update DWM keys
            using (var dwm = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM", true))
            {
                dwm?.SetValue("ColorizationColor", abgr, RegistryValueKind.DWord);
                dwm?.SetValue("ColorPrevalence", 1, RegistryValueKind.DWord);
            }

            // Broadcast change
            RefreshSystemColors();

            Color currentAccent = GetCurrentAccentColor();
            CurrentColor.BackColor = currentAccent;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(int uAction, int uParam, IntPtr lpvParam, int fuWinIni);

        const int SPI_SETNONCLIENTMETRICS = 0x002A;
        const int SPIF_SENDCHANGE = 0x02;

        public static void RefreshSystemColors()
        {
            SystemParametersInfo(SPI_SETNONCLIENTMETRICS, 0, IntPtr.Zero, SPIF_SENDCHANGE);
        }

        private void LoadScreensavers()
        {
            string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);

            // Combine .scr files from both folders
            var scrFiles = Directory.GetFiles(system32Path, "*.scr")
                .Distinct()
                .ToList();

            foreach (var file in scrFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                comboScreensaver.Items.Add(fileName);
            }

            // Get current screensaver
            string currentScr = GetCurrentScreensaver();

            if (!string.IsNullOrEmpty(currentScr))
            {
                string currentName = Path.GetFileNameWithoutExtension(currentScr);
                int index = comboScreensaver.Items.IndexOf(currentName);
                if (index >= 0)
                    comboScreensaver.SelectedIndex = index;
                else
                    comboScreensaver.SelectedIndex = 0; // fallback
            }
            else
            {
                comboScreensaver.SelectedIndex = 0; // No screensaver
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop"))
            {
                if (key != null)
                {
                    string timeoutValue = key.GetValue("ScreenSaveTimeOut", "600").ToString();
                    if (int.TryParse(timeoutValue, out int seconds))
                    {
                        numericTimeout.Value = Math.Min(Math.Max(seconds / 60, (int)numericTimeout.Minimum), (int)numericTimeout.Maximum);
                    }
                }
            }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", false))
                {
                    string secureValue = key?.GetValue("ScreenSaverIsSecure", "0").ToString();
                    checkBoxSecure.Checked = secureValue == "1";
                }
            }
            catch
            {
                // Optional: Handle errors
                checkBoxSecure.Checked = false;
            }
        }

        private string GetCurrentScreensaver()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop"))
            {
                return key?.GetValue("SCRNSAVE.EXE") as string;
            }
        }

        Process currentPreviewProcess = null;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        const int GWL_STYLE = -16;
        const uint WS_VISIBLE = 0x10000000;
        const uint WS_CHILD = 0x40000000;

        void ShowScreensaverPreview(string scrPath)
        {
            // Kill previous preview if it exists
            if (currentPreviewProcess != null && !currentPreviewProcess.HasExited)
            {
                try { currentPreviewProcess.Kill(); } catch { }
                currentPreviewProcess = null;
            }

            IntPtr parentHandle = panelThumbnail.Handle;

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = scrPath,
                Arguments = $"/p {parentHandle}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                Process proc = Process.Start(psi);
                currentPreviewProcess = proc;

                // Give it some time to load
                proc.WaitForInputIdle(1000);

                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    SetParent(proc.MainWindowHandle, parentHandle);
                    SetWindowLong(proc.MainWindowHandle, GWL_STYLE, WS_VISIBLE | WS_CHILD);
                    MoveWindow(proc.MainWindowHandle, 0, 0, panelThumbnail.Width, panelThumbnail.Height, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to preview screensaver: " + ex.Message);
            }
        }

        private void ShowSpotlightNotAvailable()
        {
            flowLayoutPanelSpotlight.Controls.Clear();

            Label label = new Label();
            label.Text = "Coming in the next beta version.";
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label.ForeColor = Color.Gray;
            label.Margin = new Padding(10);

            flowLayoutPanelSpotlight.Controls.Add(label);
        }

        private void LoadDummyImage()
        {
            PictureBox pic = new PictureBox();
            pic.Width = 100;
            pic.Height = 100;
            pic.BackColor = Color.Blue;
            pic.Margin = new Padding(10);

            flowLayoutPanelSpotlight.Controls.Add(pic);
        }

        private void LoadCurrentWallpaperAndStyle()
        {
            try
            {
                // Get wallpaper path
                string wallpaperPath = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop")?.GetValue("WallPaper") as string;

                if (!string.IsNullOrEmpty(wallpaperPath) && File.Exists(wallpaperPath))
                {

                    radioWallPic.Checked = true;
                    radioWallCol.Checked = false;
                    panelWallCol.Visible = false;

                    // Set the path in the textbox
                    textBoxWall.Text = wallpaperPath;

                    // Load image into PictureBox as background
                    pictureBoxWall.BackgroundImage = Image.FromFile(wallpaperPath);

                    // Read style settings
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
                    string style = regKey.GetValue("WallpaperStyle", "0").ToString();
                    string tile = regKey.GetValue("TileWallpaper", "0").ToString();

                    if (tile == "1")
                    {
                        comboBoxWall.SelectedIndex = 3; // Tile
                        pictureBoxWall.BackgroundImageLayout = ImageLayout.Tile;
                    }
                    else
                    {
                        switch (style)
                        {
                            case "0":
                                comboBoxWall.SelectedIndex = 0; // Center
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Center;
                                break;
                            case "6":
                                comboBoxWall.SelectedIndex = 1; // Fit
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Center;
                                break;
                            case "10":
                                comboBoxWall.SelectedIndex = 2; // Fill
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case "2":
                                comboBoxWall.SelectedIndex = 3; // Stretch
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Stretch;
                                break;
                            case "22":
                                comboBoxWall.SelectedIndex = 5; // Span
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Stretch;
                                break;
                            default:
                                comboBoxWall.SelectedIndex = 0;
                                pictureBoxWall.BackgroundImageLayout = ImageLayout.Center;
                                break;
                        }
                    }
                }
                else
                {
                    // Assume it's a solid color
                    radioWallPic.Checked = false;
                    radioWallCol.Checked = true;
                    panelWallCol.Visible = true;
                    panelWallPic.Visible = false;

                    textBoxWall.Text = "";

                    pictureBoxWall.BackgroundImage = null;

                    // Optionally load color preview
                    string colorString = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "Background", "0 0 0").ToString();
                    string[] rgbParts = colorString.Split(' ');
                    if (rgbParts.Length == 3 &&
                        int.TryParse(rgbParts[0], out int r) &&
                        int.TryParse(rgbParts[1], out int g) &&
                        int.TryParse(rgbParts[2], out int b))
                    {
                        Color color = Color.FromArgb(r, g, b);
                        pictureBoxWall.BackColor = color; // Optional preview
                    }

                    comboBoxWall.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load current wallpaper.\n\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Color[] wallpaperColors = new Color[]
{
    Color.White, Color.Black, Color.Gray, Color.Silver,
    Color.Red, Color.Orange, Color.Gold, Color.Yellow,
    Color.Green, Color.Teal, Color.Blue, Color.SkyBlue,
    Color.Indigo, Color.Violet, Color.Pink, Color.Brown
};

        private void PopulateWallpaperColors()
        {
            panelWallCol.Controls.Clear(); // Clear any existing items

            foreach (Color color in wallpaperColors)
            {
                PictureBox pic = new PictureBox
                {
                    Width = 32,
                    Height = 32,
                    BackColor = color,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(4),
                    Cursor = Cursors.Hand,
                    Tag = color
                };

                pic.Click += ColorPic_Click;
                panelWallCol.Controls.Add(pic);
            }
        }

        private void LoadTaskbarSettings()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", false);
                RegistryKey keyExplorer = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer", false);
                RegistryKey keyStuckRects = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3", false);

                if (key != null)
                {
                    checkTaskLock.Checked = Convert.ToBoolean((int)key.GetValue("TaskbarSizeMove", 1) == 0);
                    checkTaskSmall.Checked = Convert.ToBoolean((int)key.GetValue("TaskbarSmallIcons", 0) == 1);
                    checkTaskPower.Checked = Convert.ToBoolean((int)key.GetValue("DontUsePowerShellOnWinX", 0) == 0);
                    checkTaskBadge.Checked = Convert.ToBoolean((int)key.GetValue("ShowBadges", 1) == 1);

                    // Combine buttons
                    int combine = (int)key.GetValue("TaskbarGlomLevel", 0);
                    comboTaskCombine.SelectedIndex = combine; // 0: Always combine, 1: Combine when full, 2: Never combine
                }

                try
                {
                    RegistryKey key1 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
                    RegistryKey key2 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");

                    bool enabled = true;

                    if (key1 != null && key2 != null)
                    {
                        int disablePreview = Convert.ToInt32(key1.GetValue("DisablePreviewDesktop", 0));
                        int enablePeek = Convert.ToInt32(key2.GetValue("EnableAeroPeek", 1));
                        enabled = (disablePreview == 0) && (enablePeek == 1);
                    }

                    checkTaskPeek.Checked = enabled;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Aero Peek setting: " + ex.Message);
                }

                if (keyExplorer != null)
                {
                    RegistryKey keyS = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3");
                    if (keyS != null)
                    {
                        byte[] settings = (byte[])keyS.GetValue("Settings");
                        if (settings != null && settings.Length > 8)
                        {
                            checkTaskHideD.Checked = (settings[8] == 0x7B);
                        }
                    }
                    checkTaskHideT.Checked = Convert.ToBoolean((int)keyExplorer.GetValue("TaskbarAutoHideInTabletMode", 0) == 1);
                }

                if (keyStuckRects != null)
                {
                    byte[] settings = (byte[])keyStuckRects.GetValue("Settings");
                    if (settings != null && settings.Length >= 12)
                    {
                        int location = settings[12]; // 0: Bottom, 1: Left, 2: Right, 3: Top
                        comboTaskLoc.SelectedIndex = location;
                    }
                }
                int align = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", 0);
                comboTaskAlign.SelectedIndex = align == 1 ? 1 : 0;

                int searchMode = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", 0);
                comboTaskSearch.SelectedIndex = Math.Min(searchMode, 3);  // Ensure no crash if > 3

                checkTaskSeconds.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", 0)) == 1;
                checkTaskCortana.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCortanaButton", 0)) == 1;
                checkTaskPeople.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", 0)) == 1;
                checkTaskView.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", 1)) == 1;
                checkTaskWidgets.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", 1)) == 1;
                checkTaskChat.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", 1)) == 1;
                checkTaskDesk.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", 1)) == 1;
                checkTaskFlash.Checked = Convert.ToInt32(Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableTaskMgrFlash", 0)) == 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Taskbar settings:\n" + ex.Message);
            }
        }

        private void TaskbarSettingChanged(object sender, EventArgs e)
        {
            ApplyTaskbarSettings();
        }

        private void ApplyTaskbarSettings()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", true);
                RegistryKey keyExplorer = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer", true);
                RegistryKey keyStuckRects = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3", true);

                if (key != null)
                {
                    key.SetValue("TaskbarSizeMove", checkTaskLock.Checked ? 0 : 1, RegistryValueKind.DWord);
                    key.SetValue("TaskbarSmallIcons", checkTaskSmall.Checked ? 1 : 0, RegistryValueKind.DWord);
                    key.SetValue("DontUsePowerShellOnWinX", checkTaskPower.Checked ? 0 : 1, RegistryValueKind.DWord);
                    key.SetValue("ShowBadges", checkTaskBadge.Checked ? 1 : 0, RegistryValueKind.DWord);

                    key.SetValue("TaskbarGlomLevel", comboTaskCombine.SelectedIndex, RegistryValueKind.DWord);
                }

                if (keyExplorer != null)
                {
                    keyExplorer.SetValue("TaskbarAutoHideInTabletMode", checkTaskHideT.Checked ? 1 : 0, RegistryValueKind.DWord);
                }

                if (keyStuckRects != null)
                {
                    byte[] settings = (byte[])keyStuckRects.GetValue("Settings");
                    if (settings != null && settings.Length >= 13)
                    {
                        settings[12] = (byte)comboTaskLoc.SelectedIndex; // 0: Bottom, 1: Left, 2: Right, 3: Top
                        keyStuckRects.SetValue("Settings", settings, RegistryValueKind.Binary);
                    }
                }

                try
                {
                    RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
                    RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\DWM");

                    if (checkTaskPeek.Checked)
                    {
                        key1.SetValue("DisablePreviewDesktop", 0, RegistryValueKind.DWord);
                        key2.SetValue("EnableAeroPeek", 1, RegistryValueKind.DWord);
                    }
                    else
                    {
                        key1.SetValue("DisablePreviewDesktop", 1, RegistryValueKind.DWord);
                        key2.SetValue("EnableAeroPeek", 0, RegistryValueKind.DWord);
                    }

                    key1.Close();
                    key2.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating Aero Peek setting: " + ex.Message);
                }

                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search", "SearchboxTaskbarMode", comboTaskSearch.SelectedIndex, RegistryValueKind.DWord);
                int newAlign = comboTaskAlign.SelectedIndex == 1 ? 1 : 0;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarAl", newAlign, RegistryValueKind.DWord);

                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowSecondsInSystemClock", checkTaskSeconds.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowCortanaButton", checkTaskCortana.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced\People", "PeopleBand", checkTaskPeople.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowTaskViewButton", checkTaskView.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarDa", checkTaskWidgets.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarMn", checkTaskChat.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "TaskbarSd", checkTaskDesk.Checked ? 1 : 0, RegistryValueKind.DWord);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisableTaskMgrFlash", checkTaskFlash.Checked ? 0 : 1, RegistryValueKind.DWord);

                // Refresh Explorer to apply most changes
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c taskkill /f /im explorer.exe && start explorer.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to apply Taskbar settings:\n" + ex.Message);
            }
        }

        private void SetTaskbarAutohide(bool enable)
        {
            string regPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(regPath, writable: true);

            if (key != null)
            {
                byte[] value = key.GetValue("Settings") as byte[];
                if (value != null && value.Length > 8)
                {
                    value[8] = enable ? (byte)0x7B : (byte)0x02; // Modify the 9th byte

                    key.SetValue("Settings", value, RegistryValueKind.Binary);
                    key.Close();

                    // Restart Explorer so the change takes effect
                    RestartExplorer();
                }
                else
                {
                    MessageBox.Show("Failed to read the Settings value.");
                }
            }
            else
            {
                MessageBox.Show("Failed to open registry key.");
            }
        }

        private void RestartExplorer()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c taskkill /f /im explorer.exe && start explorer.exe",
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }

        private void ColorPic_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pic && pic.Tag is Color color)
            {
                string rgb = $"{color.R} {color.G} {color.B}";

                // Set solid color background
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Colors", "Background", rgb, RegistryValueKind.String);

                // Clear wallpaper image
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallPaper", "", RegistryValueKind.String);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "PicturePosition", "0", RegistryValueKind.String);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallpaperStyle", "0", RegistryValueKind.String);

                // Notify system
                SystemParametersInfo(20, 0, (string)null, 0x01 | 0x02);

                // Update UI
                pictureBoxWall.BackgroundImage = null;
                pictureBoxWall.BackColor = color;
                textBoxWall.Text = "(Solid Color)";
            }
        }


        private void buttonFind_Click(object sender, EventArgs e)
        {
            // Set filters to image formats
            OpenWall.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";

            // Optional: Set initial folder
            OpenWall.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (OpenWall.ShowDialog() == DialogResult.OK)
            {
                // Get selected file path
                string selectedPath = OpenWall.FileName;

                // Show in textbox
                textBoxWall.Text = selectedPath;

                // Optionally preview it in PictureBox
                if (File.Exists(selectedPath))
                {
                    pictureBoxWall.BackgroundImage = Image.FromFile(selectedPath);
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Zoom;
                }

                string style = "10"; // default to Fill
                string tile = "0";

                switch (comboBoxWall.SelectedIndex)
                {
                    case 0: // Fill (Zoom)
                        style = "10"; tile = "0"; break;
                    case 1: // Center
                        style = "6"; tile = "0"; break;
                    case 2: // Stretch
                        style = "2"; tile = "0"; break;
                    case 3: // Tile
                        style = "0"; tile = "1"; break;
                    default:
                        style = "10"; tile = "0"; break;
                }

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", writable: true))
                {
                    if (key != null)
                    {
                        key.SetValue("WallpaperStyle", style);
                        key.SetValue("TileWallpaper", tile);
                    }
                }

                // 4. Apply wallpaper using Windows API
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, selectedPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }

        private void comboBoxWall_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxWall.SelectedIndex;

            string wallpaperStyle = "0";
            string tileWallpaper = "0";

            switch (index)
            {
                case 0: // Center
                    wallpaperStyle = "0";
                    tileWallpaper = "0";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Center;
                    break;
                case 1: // Fit
                    wallpaperStyle = "6";
                    tileWallpaper = "0";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Zoom;
                    break;
                case 2: // Fill
                    wallpaperStyle = "10";
                    tileWallpaper = "0";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Zoom;
                    break;
                case 3: // Stretch
                    wallpaperStyle = "2";
                    tileWallpaper = "0";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Stretch;
                    break;
                case 4: // Tile
                    wallpaperStyle = "0";
                    tileWallpaper = "1";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Tile;
                    break;
                case 5: // Span
                    wallpaperStyle = "22";
                    tileWallpaper = "0";
                    pictureBoxWall.BackgroundImageLayout = ImageLayout.Zoom;
                    break;
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", writable: true))
            {
                if (key != null)
                {
                    key.SetValue("WallpaperStyle", wallpaperStyle);
                    key.SetValue("TileWallpaper", tileWallpaper);
                }
            }

            // Apply wallpaper again to refresh style
            string currentWallpaper = textBoxWall.Text;
            if (!string.IsNullOrWhiteSpace(currentWallpaper) && File.Exists(currentWallpaper))
            {
                SystemParametersInfo(20, 0, currentWallpaper, 1 | 2);
            }
        }

        private void LoadPictureModeSamples()
        {
            flowLayoutPanelSpotlight.Controls.Clear();

            string picturesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Web", "Screen");

            if (!Directory.Exists(picturesFolder))
            {
                MessageBox.Show("Sample pictures folder not found.");
                return;
            }

            // Get all .jpg and .png files
            var imageFiles = Directory.GetFiles(picturesFolder)
                .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (imageFiles.Length == 0)
            {
                Label label = new Label();
                label.Text = "No sample images found.";
                label.AutoSize = true;
                label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                label.ForeColor = Color.Gray;
                label.Margin = new Padding(10);
                flowLayoutPanelSpotlight.Controls.Add(label);
                return;
            }

            foreach (var file in imageFiles)
            {
                try
                {
                    PictureBox pic = new PictureBox();
                    pic.Width = 100;
                    pic.Height = 100;
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    pic.Image = Image.FromFile(file);
                    pic.Tag = file;
                    pic.Margin = new Padding(5);
                    pic.Cursor = Cursors.Hand;

                    pic.Click += (s, e) =>
                    {
                        string selectedImage = (string)((PictureBox)s).Tag;
                        string sharedImagePath = @"C:\Windows\Web\Screen\UltimateControl_Lock.jpg";

                        try
                        {
                            // Copy selected image to a system-wide location
                            File.Copy(selectedImage, sharedImagePath, true);

                            // Create the registry key if it doesn't exist
                            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Personalization"))
                            {
                                if (key != null)
                                {
                                    // Set the lock screen image path
                                    key.SetValue("LockScreenImage", sharedImagePath, RegistryValueKind.String);
                                    key.SetValue("NoLockScreen", 0, RegistryValueKind.DWord);
                                }
                            }

                            // Run gpupdate to refresh policy
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = "/c gpupdate /target:computer /force",
                                Verb = "runas",
                                CreateNoWindow = true,
                                UseShellExecute = true
                            });

                            MessageBox.Show("Lock screen image applied.\nYou may need to sign out or reboot to see the change.", "Done");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to set lock screen image:\n" + ex.Message, "Error");
                        }
                    };

                    flowLayoutPanelSpotlight.Controls.Add(pic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load image:\n{file}\n\n{ex.Message}");
                }
            }
        }

        private void comboBoxLock_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBoxLock.SelectedItem?.ToString();

            if (selected == "Pictures")
            {
                LoadPictureModeSamples();
            }
            else if (selected == "Windows Spotlight")
            {
                ShowSpotlightNotAvailable();
            }
        }

        private void checkBoxTransparency_CheckedChanged(object sender, EventArgs e)
        {
            const string personalizeKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string valueName = "EnableTransparency";

            using (var key = Registry.CurrentUser.OpenSubKey(personalizeKeyPath, writable: true))
            {
                if (key != null)
                {
                    int newValue = checkBoxTransparency.Checked ? 1 : 0;
                    key.SetValue(valueName, newValue, RegistryValueKind.DWord);
                }
                else
                {
                    // If the key does not exist, create it and set the value
                    using (var newKey = Registry.CurrentUser.CreateSubKey(personalizeKeyPath))
                    {
                        int newValue = checkBoxTransparency.Checked ? 1 : 0;
                        newKey.SetValue(valueName, newValue, RegistryValueKind.DWord);
                    }
                }
            }

            // Optionally notify the system of the change
            NotifyAccentColorChanged();
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam,
    uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

        const int HWND_BROADCAST = 0xffff;
        const int WM_SETTINGCHANGE = 0x001A;
        const uint SMTO_ABORTIFHUNG = 0x0002;

        private void NotifyAccentColorChanged()
        {
            SendMessageTimeout(new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE, IntPtr.Zero, "ImmersiveColorSet",
                SMTO_ABORTIFHUNG, 100, out _);
        }

        private void checkBoxColorStart_CheckedChanged(object sender, EventArgs e)
        {
            const string personalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (var key = Registry.CurrentUser.OpenSubKey(personalizePath, true))
            {
                if (key != null)
                {
                    int val = checkBoxColorStart.Checked ? 1 : 0;
                    key.SetValue("ColorPrevalence", val, RegistryValueKind.DWord);
                }
            }
            NotifyAccentColorChanged();
        }

        private void checkBoxColorTitle_CheckedChanged(object sender, EventArgs e)
        {
            const string dwmPath = @"Software\Microsoft\Windows\DWM";
            using (var key = Registry.CurrentUser.OpenSubKey(dwmPath, true))
            {
                if (key != null)
                {
                    int val = checkBoxColorTitle.Checked ? 1 : 0;
                    key.SetValue("ColorPrevalence", val, RegistryValueKind.DWord);
                }
            }
            NotifyAccentColorChanged();
        }

        private void comboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string personalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (var key = Registry.CurrentUser.OpenSubKey(personalizePath, true))
            {
                if (key != null && comboMode.SelectedIndex != 2)
                {
                    int val = comboMode.SelectedIndex;
                    key.SetValue("AppsUseLightTheme", val, RegistryValueKind.DWord);
                    key.SetValue("SystemUsesLightTheme", val, RegistryValueKind.DWord);
                    comboModeApp.Visible = false;
                    labelModeApp.Visible = false;
                    comboModeWin.Visible = false;
                    labelModeWin.Visible = false;

                    comboModeApp.SelectedIndex = comboMode.SelectedIndex;
                    comboModeWin.SelectedIndex = comboMode.SelectedIndex;
                }
                else if (key != null && comboMode.SelectedIndex == 2)
                {
                    comboModeApp.Visible = true;
                    labelModeApp.Visible = true;
                    comboModeWin.Visible = true;
                    labelModeWin.Visible = true;
                }
            }
            NotifyAccentColorChanged();
        }

        private void comboModeWin_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string personalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (var key = Registry.CurrentUser.OpenSubKey(personalizePath, true))
            {
                if (key != null)
                {
                    int val = comboModeWin.SelectedIndex;
                    key.SetValue("SystemUsesLightTheme", val, RegistryValueKind.DWord);
                }
            }
        }

        private void comboModeApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string personalizePath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            using (var key = Registry.CurrentUser.OpenSubKey(personalizePath, true))
            {
                if (key != null)
                {
                    int val = comboModeApp.SelectedIndex;
                    key.SetValue("AppsUseLightTheme", val, RegistryValueKind.DWord);
                }
            }
        }

        private void buttonScrApply_Click(object sender, EventArgs e)
        {
            if (comboScreensaver.SelectedIndex == 0) // "None" selected
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    key?.DeleteValue("SCRNSAVE.EXE", false); // Remove screensaver setting
                }
            }
            else
            {
                string selectedName = comboScreensaver.SelectedItem.ToString();
                string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                string fullPath = Path.Combine(system32Path, selectedName + ".scr");

                if (!File.Exists(fullPath))
                {
                    MessageBox.Show("Selected screensaver file not found in System32.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    key?.SetValue("SCRNSAVE.EXE", fullPath);
                }
            }

            int timeoutInSeconds = (int)numericTimeout.Value * 60;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
            {
                if (key != null)
                {
                    key.SetValue("ScreenSaveTimeOut", timeoutInSeconds.ToString(), RegistryValueKind.String);
                }
            }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    if (key != null)
                    {
                        key.SetValue("ScreenSaverIsSecure", checkBoxSecure.Checked ? "1" : "0", RegistryValueKind.String);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update logon screen setting: " + ex.Message);
            }

            buttonScrApply.Enabled = false;
        }

        private void comboScreensaver_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonScrApply.Enabled = true;
            if (comboScreensaver.SelectedIndex == 0) { buttonScrPreview.Enabled = false; }
            else { buttonScrPreview.Enabled = true; }

            if (comboScreensaver.SelectedIndex == 0)
            {
                // No preview for "None"
                if (currentPreviewProcess != null && !currentPreviewProcess.HasExited)
                    currentPreviewProcess.Kill();
                return;
            }

            string scrName = comboScreensaver.SelectedItem.ToString();
            string scrPath = Path.Combine(Environment.SystemDirectory, scrName + ".scr");

            if (File.Exists(scrPath))
                ShowScreensaverPreview(scrPath);
        }

        private void buttonScrPreview_Click(object sender, EventArgs e)
        {
            string selectedName = comboScreensaver.SelectedItem.ToString();
            string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string fullPath = Path.Combine(system32Path, selectedName + ".scr");

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("Screensaver file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(fullPath, "/s");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch preview:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonScrConfigure_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (comboScreensaver.SelectedIndex == 0)
            {
                MessageBox.Show("No screensaver selected.", "Configure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedName = comboScreensaver.SelectedItem.ToString();
            string system32Path = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string fullPath = Path.Combine(system32Path, selectedName + ".scr");

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("Screensaver file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = fullPath,
                    Arguments = "/c",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open configuration dialog:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericTimeout_ValueChanged(object sender, EventArgs e)
        {
            buttonScrApply.Enabled = true;
        }

        private void checkBoxSecure_CheckedChanged(object sender, EventArgs e)
        {
            buttonScrApply.Enabled = true;
        }

        private void linkOtherColors_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.AnyColor = true;
                colorDialog.FullOpen = true;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;
                    ApplyAccentColor(selectedColor);
                    CurrentColor.BackColor = selectedColor;
                }
            }
        }

        private void radioWallCol_CheckedChanged(object sender, EventArgs e)
        {
            panelWallCol.Visible = true;
            panelWallPic.Visible = false;

        }

        private void radioWallPic_CheckedChanged(object sender, EventArgs e)
        {
            panelWallCol.Visible = false;
            panelWallPic.Visible = true;
        }

        private void buttonTaskCustomizeNotify_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = "shell:::{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}",
                UseShellExecute = true  // Required for shell: commands
            });
        }

        private void checkTaskHideD_CheckedChanged(object sender, EventArgs e)
        {
            SetTaskbarAutohide(checkTaskHideD.Checked);
        }

        private void linkDesktop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage1);
            linkDesktop.Font = new Font(linkDesktop.Font.Name, linkDesktop.Font.Size, FontStyle.Bold);
            linkColors.Font = new Font(linkColors.Font.Name, linkColors.Font.Size, FontStyle.Regular);
            linkScr.Font = new Font(linkScr.Font.Name, linkScr.Font.Size, FontStyle.Regular);
            linkTsk.Font = new Font(linkTsk.Font.Name, linkTsk.Font.Size, FontStyle.Regular);
        }

        private void linkColors_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage2);
            linkDesktop.Font = new Font(linkDesktop.Font.Name, linkDesktop.Font.Size, FontStyle.Regular);
            linkColors.Font = new Font(linkColors.Font.Name, linkColors.Font.Size, FontStyle.Bold);
            linkScr.Font = new Font(linkScr.Font.Name, linkScr.Font.Size, FontStyle.Regular);
            linkTsk.Font = new Font(linkTsk.Font.Name, linkTsk.Font.Size, FontStyle.Regular);
        }

        private void linkScr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage4);
            linkDesktop.Font = new Font(linkDesktop.Font.Name, linkDesktop.Font.Size, FontStyle.Regular);
            linkColors.Font = new Font(linkColors.Font.Name, linkColors.Font.Size, FontStyle.Regular);
            linkScr.Font = new Font(linkScr.Font.Name, linkScr.Font.Size, FontStyle.Bold);
            linkTsk.Font = new Font(linkTsk.Font.Name, linkTsk.Font.Size, FontStyle.Regular);
        }

        private void linkTsk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tabPage5);
            linkDesktop.Font = new Font(linkDesktop.Font.Name, linkDesktop.Font.Size, FontStyle.Regular);
            linkColors.Font = new Font(linkColors.Font.Name, linkColors.Font.Size, FontStyle.Regular);
            linkScr.Font = new Font(linkScr.Font.Name, linkScr.Font.Size, FontStyle.Regular);
            linkTsk.Font = new Font(linkTsk.Font.Name, linkTsk.Font.Size, FontStyle.Bold);
        }

        private void linkOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ultimatecontrol.github.io/doc/");
        }
    }
}