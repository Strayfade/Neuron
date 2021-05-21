using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuronWebdriver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppContainer APCON = new AppContainer();
            APCON.Tabs.Add(new EasyTabs.TitleBarTab(APCON) { Content = new Form1 { Text = "New Tab" } });
            APCON.SelectedTabIndex = 0;
            EasyTabs.TitleBarTabsApplicationContext context = new EasyTabs.TitleBarTabsApplicationContext();
            //context.Start(APCON);

            Form1 FORM = new Form1();

            Application.Run(FORM);
        }
    }
}
