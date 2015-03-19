using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace Rapid_Reporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            Logger.Record("[App]: >>>> Rapid Reporter Starting", "App", "info");

            Logger.Record("[App]: Applic Name: " + System.Diagnostics.Process.GetCurrentProcess().ProcessName, "App", "info");
            // OS Version can be analyzed with the help of:
            //  http://stackoverflow.com/questions/545666/how-to-translate-ms-windows-os-version-numbers-into-product-names-in-net
            //  http://www.eggheadcafe.com/software/aspnet/35878122/how-to-determine-windows-flavor-in-net.aspx
            Logger.Record("[App]: OS  Version: " + System.Environment.OSVersion, "App", "info");

            // Framework can be analyzed with the help of:
            //  http://en.wikipedia.org/wiki/List_of_.NET_Framework_versions
            Logger.Record("[App]: Env version: " + System.Environment.Version, "App", "info");

            // Locale can be analyzed with the help of:
            //  http://msdn.microsoft.com/en-us/goglobal/bb964664.aspx
            //  http://msdn.microsoft.com/en-us/goglobal/bb895996
            Logger.Record("[App]: Keyb Locale: " + InputLanguage.CurrentInputLanguage.Culture.KeyboardLayoutId, "App", "info");

            Logger.Record("[App]: Instances #: " + System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length.ToString(), "App", "info");
            Logger.Record("[App]: Working Set: " + System.Environment.WorkingSet.ToString(), "App", "info");
            Logger.Record("[App]: Current Dir: " + System.Environment.CurrentDirectory, "App", "info");
            Logger.Record("[App]: CommandLine: " + System.Environment.CommandLine, "App", "info");
            Logger.Record("[App]: Network  On: " + SystemInformation.Network, "App", "info");
            Logger.Record("[App]: Monitor num: " + SystemInformation.MonitorCount, "App", "info");
            Logger.Record("[App]: WorkingArea: " + SystemInformation.WorkingArea, "App", "info");
        }
        ~App()
        {
            Logger.Record("[App]: <<<< Rapid Reporter Ending", "App", "info");
            Logger.Record("[App]: <<<< ====================================", "App", "info");
        }

    }
}
