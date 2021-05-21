using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EasyTabs;

using CefSharp;
using CefSharp.WinForms;

namespace NeuronWebdriver
{
    public partial class Form1 : Form
    {
        private DateTime StartLoadTime;
        private DateTime EndLoadTime;
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
        }
        private void InitializeChromium()
        {
            Timer.Start();
        }
        protected TitleBarTabs ParentTabs
        {
            get
            {
                return (ParentForm as TitleBarTabs);
            }
        }

        private string StoredHomepageURL = "duckduckgo.com/?kae=d&ks=s&kt=v";

        private void Form1_Load(object sender, EventArgs e)
        {
            NBrowser.Load(StoredHomepageURL);
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void NBrowser_LoadError(object sender, LoadErrorEventArgs e)
        {
            //ErrorTitle.Text = e.ErrorText;
            //ErrorCodeBox.Text = "Error Code: " + e.ErrorCode;
            //ErrorURLBox.Text = "URL: " + e.FailedUrl;
        }

        private void siticoneRoundedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                tabControl1.TabIndex = 0;
                tabControl1.SelectedIndex = 0;
                NBrowser.Load(siticoneRoundedTextBox1.Text);
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

        private void NBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            StartLoadTime = DateTime.Now;
        }
        string mss;
        int msi;
        private void NBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            EndLoadTime = DateTime.Now;
            mss = EndLoadTime.Subtract(StartLoadTime).Milliseconds.ToString();
            msi = EndLoadTime.Subtract(StartLoadTime).Milliseconds;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            tabControl1.TabIndex = 2;
            tabControl1.SelectedIndex = 2;
        }

        private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndex = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Start();
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
    }
}
