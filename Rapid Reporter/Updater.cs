using System;
using System.Reflection;
using Rapid_Reporter.Forms;
using Rapid_Reporter.HTML;

namespace Rapid_Reporter
{
    internal class Updater
    {
        internal void CheckVersion()
        {
            var parsedVersion = GetServerVersion();
            if (parsedVersion == null) return;
            if (UpToDateWithLatest(parsedVersion)) return;
            if (!AllowedToCheckForUpdates(parsedVersion)) return;

            var updateDlg = new UpdateForm();
            updateDlg.ShowDialog();

            switch (updateDlg.Choice)
            {
                case UpdateChosen.Update:
                    SetAllowedToCheckValue(new Version(0,0,0,0));
                    System.Diagnostics.Process.Start("https://github.com/jankcat/rapidreporterplusplus/releases");
                    break;
                case UpdateChosen.Skip:
                    SetAllowedToCheckValue(parsedVersion);
                    break;
                case UpdateChosen.Later:
                    SetAllowedToCheckValue(new Version(0, 0, 0, 0));
                    return;
            }
        }

        private static bool AllowedToCheckForUpdates(Version serverVersion)
        {
            var str = RegUtil.ReadRegKey("UpdateToSkip");
            if (string.IsNullOrWhiteSpace(str))
                return true;
            Version regver;
            try
            {
                regver = new Version(str);
            }
            catch
            {
                return true;
            }
            return regver != serverVersion;
        }

        private static void SetAllowedToCheckValue(Version updateToSkip)
        {
            RegUtil.CreateRegKey("UpdateToSkip", updateToSkip.ToString());
        }

        private bool UpToDateWithLatest(Version latest)
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return appVersion >= latest;
        }

        private static Version GetServerVersion()
        {
            //var verCall = HttpCallUtil.HttpGetCall(@"https://raw.githubusercontent.com/jankcat/rapidreporterplusplus/development/currentVersion.txt"); //development
            var verCall = HTML.HttpCallUtil.HttpGetCall(@"https://raw.githubusercontent.com/jankcat/rapidreporterplusplus/master/currentVersion.txt"); //master
            if (string.IsNullOrWhiteSpace(verCall.Message)) return null;
            var ver = verCall.Message.Split(Convert.ToChar("."));
            if (ver.Length != 4) return null;
            try
            {
                var parsedVersion = new Version(verCall.Message);
                return parsedVersion;
            }
            catch
            {
                return null;
            }
        }
    }
}
