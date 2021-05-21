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
    public partial class AppContainer : TitleBarTabs
    {
        public AppContainer()
        {
            InitializeComponent();
            AeroPeekEnabled = true;
            ChromeTabRenderer Renderer = new ChromeTabRenderer(this);
            Renderer.ShowAddButton = true;
            TabRenderer = Renderer;
        }
        public override TitleBarTab CreateTab()
        {
            return new TitleBarTab(this)
            {
                Content = new Form1
                {
                    Text = "New Tab"
                }
            };
        }
    }
}
