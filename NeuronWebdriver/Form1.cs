using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

using EasyTabs;

using CefSharp;
using CefSharp.WinForms;

using NeuronWebdriver.Options;

namespace NeuronWebdriver
{
    public partial class Form1 : Form
    {
        // Create a Options Duckduckgo Parameter Controller Class
        OP_DDG OP_DDG_PC;
        string OP_DDG_URL = "https://duckduckgo.com";
        public string SelectedTheme = "d";
        public string SelectedFont = "p";
        public string SelectedParams = "kae=d";
        public string OP_DDG_GenerateURL()
        {
            string URL = "https://duckduckgo.com/?kae=" + SelectedTheme + "&kt=" + SelectedFont;
            if (SelectedParams.Length > 1)
            {
                URL += "&" + SelectedParams.ToString();
                if (siticoneOSToggleSwith1.Checked)
                {
                    URL += "&kf=1";
                }
                else if (!siticoneOSToggleSwith1.Checked)
                {
                    URL += "&kf=-1";
                }
                if (siticoneOSToggleSwitch1.Checked)
                {
                    URL += "&kh=1";
                }
                else if (!siticoneOSToggleSwitch1.Checked)
                {
                    URL += "&kh=-1";
                }
                if (siticoneOSToggleSwith2.Checked)
                {
                    URL += "&kp=1";
                }
                else if (!siticoneOSToggleSwith2.Checked)
                {
                    URL += "&kp=-2";
                }
                if (siticoneOSToggleSwith3.Checked)
                {
                    URL += "&kc=1";
                }
                else if (!siticoneOSToggleSwith3.Checked)
                {
                    URL += "&kc=-1";
                }
                if (siticoneOSToggleSwith4.Checked)
                {
                    URL += "&k1=1";
                }
                else if (!siticoneOSToggleSwith4.Checked)
                {
                    URL += "&k1=-1";
                }
            }
            return URL;
        }

        // History Vars
        bool SaveBrowserHistory = false;
        string HistoryFilePassword = "";
        string BrowserHistoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\Neuron";

        // Setup Error Logging and stuff
        public string EErrorTitle = "";
        public string EErrorCode = "";
        public string EURL = "";

        // Cool, you can store this here
        public string version = "1.0";

        // Used to calculate load times.
        LoadtimeController LTC;


        public Form1()
        {
            InitializeComponent();
            InitializeChromium();

            Initialize();
        }
        private void InitializeChromium()
        {
            Timer.Start();
        }
        public void Initialize()
        {
            NBrowser.Visible = false;

            TopGradientPanel.Size = new Size(TopGradientPanel.Width, 47);
            EErrorTitle = "";
            EErrorCode = "";
            EURL = "";

            #if DEBUG
                siticoneLabel2.Text = "Development Build " + version;
            #else
                siticoneLabel2.Text = "Release Build " + version;
            #endif

            OP_DDG_URL = OP_DDG_GenerateURL();

            NBrowser.Load(OP_DDG_URL);

            string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(Appdata + "\\Neuron"))
            {
                Directory.CreateDirectory(Appdata + "\\Neuron");
            }
            if (!File.Exists(Appdata + "\\Neuron" + "\\temp.nfs"))
            {
                File.Create(Appdata + "\\Neuron" + "\\temp.nfs");
            }

            NBrowser.Visible = true;

            siticoneRoundedButton5.Text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\Neuron\\";
        }
        protected TitleBarTabs ParentTabs
        {
            get
            {
                return (ParentForm as TitleBarTabs);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        bool EOc = false;
        private void NBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            EErrorTitle = e.ErrorText.ToString();
            EErrorCode = e.ErrorCode.ToString();
            EURL = e.FailedUrl.ToString();
            if (!siticoneRoundedTextBox1.Text.StartsWith("https"))
            {
                if (!siticoneRoundedTextBox1.Text.StartsWith("http"))
                {
                    //NBrowser.Load(("http://www.") + siticoneRoundedTextBox1.Text);
                }
                else
                {
                    EOc = true;
                }
            }
            else
            {
                EOc = true;
            }
        }

        // Main
        private void siticoneRoundedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                tabControl1.TabIndex = 0;
                tabControl1.SelectedIndex = 0;
                string newURL;
                if (siticoneRoundedTextBox1.Text.Contains(" ") || !(siticoneRoundedTextBox1.Text.Contains(".")))
                {
                    string search = "";
                    for (int x = 0; x < siticoneRoundedTextBox1.Text.Length; x++)
                    {
                        search += siticoneRoundedTextBox1.Text[x].ToString().Replace(" ", "+");
                    }
                    if (OP_DDG_URL.EndsWith(".com"))
                    {
                        newURL = OP_DDG_URL + "ia=web&q=" + search;
                    }
                    else
                    {
                        newURL = OP_DDG_URL + "&ia=web&q=" + search;
                    }
                }
                else
                {
                    newURL = siticoneRoundedTextBox1.Text;
                    if (!siticoneRoundedTextBox1.Text.StartsWith("https"))
                    {
                        newURL = (("https://") + siticoneRoundedTextBox1.Text);
                    }
                }
                NBrowser.Load(newURL);
                if (SaveBrowserHistory)
                {
                    WriteToHistory(newURL + " : " + DateTime.Now.ToString());
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            NBrowser.Back();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            NBrowser.Refresh();
        }
        private void siticoneGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void TopGradientPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        // Load Times Calculator
        private void NBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            LTC.StartLoadTime = DateTime.Now;
        }
        string mss;
        int msi;
        private void NBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            LTC.EndLoadTime = DateTime.Now;
            mss = LTC.Diff().Millisecond.ToString();
            msi = LTC.Diff().Millisecond;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                tabControl1.SelectedIndex = 2;
            else if(tabControl1.SelectedIndex == 1)
                tabControl1.SelectedIndex = 0;
            else if(tabControl1.SelectedIndex == 2)
                tabControl1.SelectedIndex = 0;
            else if (tabControl1.SelectedIndex == 3)
                tabControl1.SelectedIndex = 0;
        }
        private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndex = 0;
        }

        // Loop
        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Start(); 

            ErrorTitle.Text = EErrorTitle;
            ErrorCodeBox.Text = "Error Code: " + EErrorCode;
            ErrorURLBox.Text = "URL: " + EURL;

            if (EOc)
            {
                tabControl1.SelectedIndex = 1;
                EOc = false;
            }

            if (msi < 100)
            {
                MSLabel.Text = mss + "ms";
                if (msi < 10)
                    MSLabel.ForeColor = Color.Green;
                else if (msi < 22)
                    MSLabel.ForeColor = Color.GreenYellow;
                else if (msi < 38)
                    MSLabel.ForeColor = Color.YellowGreen;
                else if (msi < 50)
                    MSLabel.ForeColor = Color.Yellow;
                else if (msi < 62)
                    MSLabel.ForeColor = Color.Orange;
                else if (msi < 75)
                    MSLabel.ForeColor = Color.OrangeRed;
                else if (msi < 100)
                    MSLabel.ForeColor = Color.Red;
                else
                    MSLabel.ForeColor = Color.LightGray;
            }
            else
                MSLabel.Text = "-- ms";
        }

        // Misc
        public string strcon(string s1, string s2)
        {
            return s1 + s2;
        }
        
        // Help Links
        public string HL_DDG_URLParams = "https://github.com/Strayfade/Neuron/blob/NeuronRW/GitHub/DDG-URLParams.md";
        public string HL_DDG_Themes = "https://github.com/Strayfade/Neuron/blob/NeuronRW/GitHub/DDG-Themes.md";
        public string HL_DDG_Fonts = "https://github.com/Strayfade/Neuron/blob/NeuronRW/GitHub/DDG-Fonts.md";
        public string HL_NRW_History = "https://github.com/Strayfade/Neuron/blob/NeuronRW/GitHub/NRW-History.md";
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(HL_DDG_URLParams);
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(HL_DDG_Fonts);
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(HL_DDG_Themes);
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(HL_NRW_History);
        }
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(HL_NRW_History);
        }

        // Menus Navigation
        private void siticoneRoundedButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }
        private void siticoneRoundedButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        // DuckDuckGo Settings
        private void siticoneRoundedComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTheme = OP_DDG_PC.OP_DDG_ThemeOptionToParameter(siticoneRoundedComboBox1.SelectedIndex);
            OP_DDG_URL = OP_DDG_GenerateURL();
        }
        private void siticoneRoundedComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedFont = OP_DDG_PC.OP_DDG_FontOptionToParameter(siticoneRoundedComboBox2.SelectedIndex);
            OP_DDG_URL = OP_DDG_GenerateURL();
        }
        private void siticoneRoundedTextBox2_TextChanged(object sender, EventArgs e)
        {
            SelectedParams = siticoneRoundedTextBox2.Text;
            OP_DDG_URL = OP_DDG_GenerateURL();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void WriteToHistory(string website)
        {
            using (FileStream zipToOpen = new FileStream(BrowserHistoryPath + "\\temp.nfs", FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    string[] s = website.Split('\n');
                    foreach (string e in s)
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(RandomString(64) + ".txt");
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            writer.WriteLine(e.ToString().Replace("\n", ""));
                        }
                    }
                }
            }
        }
        // History Settings
        bool HasShowedNotification1 = true;
        private void siticoneOSToggleSwith9_CheckedChanged(object sender, EventArgs e)
        {
            SaveBrowserHistory = siticoneOSToggleSwith9.Checked;
            if (siticoneOSToggleSwith9.Checked)
            {
                using (FileStream zipToOpen = new FileStream(BrowserHistoryPath + "\\temp.nfs", FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry("log.txt");
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            writer.WriteLine("PasswordHere");
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(BrowserHistoryPath + "\\temp.nfs"))
                    File.Delete(BrowserHistoryPath + "\\temp.nfs");
            }
        }
        private void siticoneRoundedButton5_Click(object sender, EventArgs e)
        {
            string OldPath = BrowserHistoryPath;
            FolderBrowserDialog F = new FolderBrowserDialog();
            F.ShowNewFolderButton = true;
            F.Description = "Choose a new folder to store the history file.";
            F.ShowDialog();
            BrowserHistoryPath = F.SelectedPath;
            siticoneRoundedButton5.Text = BrowserHistoryPath;
            try
            {
                if (!Directory.Exists(BrowserHistoryPath))
                    Directory.CreateDirectory(BrowserHistoryPath);
                if (!File.Exists(BrowserHistoryPath + "\\temp.nfs"))
                    File.Create(BrowserHistoryPath + "\\temp.nfs");
                if (!File.Exists(OldPath + "\\temp.nfs"))
                    File.Delete(OldPath + "\\temp.nfs");
            }
            catch(Exception E)
            {
                Console.Write(E.ToString());
            }
        }
        private void siticoneRoundedButton4_Click(object sender, EventArgs e)
        {
            string zipPath = BrowserHistoryPath + "\\temp.nfs";
            if (!Directory.Exists(BrowserHistoryPath + "\\temp"))
            {
                Directory.CreateDirectory(BrowserHistoryPath + "\\temp");
            }
            string extractPath = Path.GetFullPath(BrowserHistoryPath + "\\temp");

            // Ensures that the last character on the extraction path
            // is the directory separator char.
            // Without this, a malicious zip file could try to traverse outside of the expected
            // extraction path.
            if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                extractPath += Path.DirectorySeparatorChar;

            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // Gets the full path to ensure that relative segments are removed.
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                        // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                        // are case-insensitive.
                        if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);
                        string current = System.IO.File.ReadAllText(destinationPath);
                        StreamWriter file = new StreamWriter(BrowserHistoryPath + "\\nfs.txt", append: true);
                        file.WriteLine(current.Replace('\n', ' '));
                        file.Close();
                    }
                }
            }
        }
        private void siticoneRoundedTextBox4_Enter(object sender, EventArgs e)
        {
            if (!HasShowedNotification1)
            {
                NotificationForm F = new NotificationForm();
                F.Show();
                F.Activate();
                HasShowedNotification1 = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
