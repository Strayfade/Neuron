using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
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
        public string SelectedParams = "?kf=1";
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
        string CurrentWebsite = "";

        // Cool, you can store this here
        public string version = "1.2";

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
        public string EErrorTitle = "";
        public string EErrorCode = "";
        public string EURL = "";
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
            CurrentWebsite = OP_DDG_URL;

            string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(Appdata + "\\Neuron"))
            {
                Directory.CreateDirectory(Appdata + "\\Neuron");
            }
            if (Directory.Exists(Appdata + "\\Neuron\\temp"))
            {
                Directory.Delete(Appdata + "\\Neuron\\temp", true);
            }
            ClearFile(Appdata + "\\Neuron" + "\\temp.nfs");
            DeleteFile(Appdata + "\\Neuron" + "\\nfs.txt");

            NBrowser.Visible = true;

            DeleteFile(BrowserHistoryPath + "\\temp.nfs");

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

        // Main and Loop
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
                CurrentWebsite = newURL;
                if (SaveBrowserHistory)
                {
                    WriteToHistory(GetFullDomain(NBrowser.Address) + " : " + DateTime.Now.ToString() + " : " + CurrentWebsite);
                }
            }
        }
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

        // Domain Retrieval Functions
        private List<string> ListSplit(string input, char delimiter)
        {
            string[] T = input.ToString().Split(delimiter);
            List<string> R = new List<string>();
            foreach (string S in T)
            {
                R.Add(S.ToString());
            }
            return R;
        }
        private string GetFullDomain(string URL)
        {
            List<string> Spliced = ListSplit(URL, '.');
            string DomainType = Spliced[Spliced.Count - 1].Split('/')[0];
            string[] DomainNames = Spliced[Spliced.Count - 2].Split('/');
            string DomainName = DomainNames[DomainNames.Length - 1];
            return DomainName + "." + DomainType;
        }
        private string GetDomainName(string URL)
        {
            List<string> Spliced = ListSplit(URL, '.');
            string[] DomainNames = Spliced[Spliced.Count - 2].Split('/');
            string DomainName = DomainNames[DomainNames.Length - 1];
            return DomainName;
        }
        private string GetDomainType(string URL)
        {
            List<string> Spliced = ListSplit(URL, '.');
            string DomainType = Spliced[Spliced.Count - 1].Split('/')[0];
            return DomainType;
        }

        // Navigation
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            NBrowser.Back();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            NBrowser.Load(NBrowser.Address);
        }
        private void siticoneGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void TopGradientPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void siticoneRoundedButton7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                tabControl1.SelectedIndex = 2;
            else if (tabControl1.SelectedIndex == 1)
                tabControl1.SelectedIndex = 0;
            else if (tabControl1.SelectedIndex == 2)
                tabControl1.SelectedIndex = 0;
            else if (tabControl1.SelectedIndex == 3)
                tabControl1.SelectedIndex = 0;
        }
        private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndex = 0;
        }
        private void siticoneRoundedButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }
        private void siticoneRoundedButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        // Load Times Calculator
        string mss;
        int msi;
        private void NBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            LTC.StartLoadTime = DateTime.Now;
        }
        private void NBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            LTC.EndLoadTime = DateTime.Now;
            mss = LTC.Diff().Millisecond.ToString();
            msi = LTC.Diff().Millisecond;
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

        // History Functions
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void WriteToHistory(string website)
        {
            AddEntry(BrowserHistoryPath + "\\temp.nfs", RandomString(128), website);
        }
        bool HasShowedNotification1 = true;
        private void siticoneOSToggleSwith9_CheckedChanged(object sender, EventArgs e)
        {
            SaveBrowserHistory = siticoneOSToggleSwith9.Checked;
            string path = BrowserHistoryPath + "\\temp.nfs";
            if (siticoneOSToggleSwith9.Checked)
            {
                ClearFile(path);
                AddEntry(path, "Start", "Begin File");
            }
            else
            {
                DeleteFile(path);
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
            catch (Exception E)
            {
                Console.Write(E.ToString());
            }
        }
        private void siticoneRoundedButton4_Click(object sender, EventArgs e)
        {
            DeleteFile(BrowserHistoryPath + "\\nfs.txt");
            System.Threading.Thread.Sleep(100);
            string path = BrowserHistoryPath + "\\temp.nfs";
            //AddEntry(path, "End", "End of File");
            System.Threading.Thread.Sleep(100);
            string h = GetAllEntries(path);
            System.Threading.Thread.Sleep(100);

            File.WriteAllLines(BrowserHistoryPath + "\\nfs.txt", new string[] { h.ToString() });

            SaveFileDialog S = new SaveFileDialog();
            S.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            S.FilterIndex = 1;
            S.RestoreDirectory = true;

            if (S.ShowDialog() == DialogResult.OK)
            {
                if (S.FileName != "")
                {
                    File.Copy(BrowserHistoryPath + "\\nfs.txt", S.FileName, true);
                }
            }
            S.Dispose();
            System.Threading.Thread.Sleep(100);

            string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DeleteFile(BrowserHistoryPath + "\\nfs.txt");
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
        private void CreateFile(string location)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(location)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(location));
                }
                if (!File.Exists(Path.GetFullPath(location)))
                {
                    File.Create(Path.GetFullPath(location));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void DeleteFile(string location)
        {
            try
            {
                if (File.Exists(Path.GetFullPath(location)))
                {
                    File.Delete(Path.GetFullPath(location));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void ClearFile(string File)
        {
            try
            {
                DeleteFile(File);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            try
            {
                CreateFile(File);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void AddEntry(string File, string EntryName, string Data)
        {
            try
            {
                using (FileStream zipToOpen = new FileStream(File, FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(EntryName + ".txt");
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            writer.Write(GetAllEntries(File) + "\n" + Data);
                            System.Threading.Thread.Sleep(100);
                            writer.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private string GetEntry(string File, string EntryName)
        {
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(File))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName == EntryName)
                        {
                            // Gets the full path to ensure that relative segments are removed.
                            if (!Directory.Exists(File))
                            {
                                Directory.CreateDirectory(File);
                            }
                            string extractPath = File + "\\" + EntryName + ".txt";
                            string destinationPath = Path.GetFullPath(extractPath);

                            // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                            // are case-insensitive.

                            if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                                entry.ExtractToFile(destinationPath);

                            string current = System.IO.File.ReadAllText(destinationPath);
                            DeleteFile(destinationPath);
                            return current;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }
        private string GetAllEntries(string File)
        {
            string returnval = "";
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(File))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        Stream S = entry.Open();
                        StreamReader SR = new StreamReader(S);
                        returnval += SR.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return returnval;
        }
        private void siticoneRoundedButton6_Click(object sender, EventArgs e)
        {
            string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (Directory.Exists(Appdata + "\\Neuron\\temp"))
            {
                Directory.Delete(Appdata + "\\Neuron\\temp", true);
            }
            if (File.Exists(Appdata + "\\Neuron\\temp.nfs"))
            {
                File.Delete(Appdata + "\\Neuron\\temp.nfs");
            }
            File.Create(Appdata + "\\Neuron\\temp.nfs");
            ClearFile(BrowserHistoryPath + "\\temp.nfs");
        }

        // Debugging
        private void MSLabel_Click(object sender, EventArgs e)
        {
            siticoneRoundedTextBox1.Text = GetDomainName(NBrowser.Address);
        }

        private void linkLabel5_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (Directory.Exists(BrowserHistoryPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = BrowserHistoryPath,
                        FileName = "explorer.exe"
                    };
                    Process.Start(startInfo);
                }
                else
                {   
                    MessageBox.Show(string.Format("{0} Directory does not exist!", BrowserHistoryPath));
                }
            }
            catch(Exception x){}
        }
    }
}
