using System;
using System.Reflection;
using System.Windows;
using RapidLib;
using Rapid_Reporter.Forms;
using Rapid_Reporter.HTML;

namespace Rapid_Reporter
{
    internal static class Updater
    {
        internal static void ManualCheckVersion()
        {
            var parsedVersion = GetServerVersion();
            if (parsedVersion == null)
            {
                RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                MessageBox.Show("Unable to retrieve latest version number from GitHub.");
                return;
            }
            if (UpToDateWithLatest(parsedVersion))
            {
                RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                MessageBox.Show("You are currently up to date!\r\nWe will let you know if a new version comes down the pipeline.");
                return;
            }
            ShowUpdateDlg(parsedVersion);
        }

        private static void ShowUpdateDlg(Version parsedVersion)
        {
            var updateDlg = new UpdateForm();
            updateDlg.UpdateServerVersion(parsedVersion);
            updateDlg.ShowDialog();
            switch (updateDlg.Choice)
            {
                case UpdateChosen.Update:
                    RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                    System.Diagnostics.Process.Start("https://github.com/jankcat/rapidreporterplusplus/releases");
                    return;
                case UpdateChosen.Skip:
                    RegUtil.UpdateToSkip = parsedVersion;
                    return;
                case UpdateChosen.Later:
                    RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                    return;
            }
        }

        internal static void CheckVersion()
        {
            var parsedVersion = GetServerVersion();
            if (parsedVersion == null) return;
            if (UpToDateWithLatest(parsedVersion)) return;
            if (!RegUtil.CheckForUpdates) return;
            if (RegUtil.UpdateToSkip == parsedVersion) return;
            ShowUpdateDlg(parsedVersion);
        }

        private static bool UpToDateWithLatest(Version latest)
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            return appVersion >= latest;
        }

        private static Version GetServerVersion()
        {
            //var verCall = HttpCallUtil.HttpGetCall(@"https://raw.githubusercontent.com/jankcat/rapidreporterplusplus/development/currentVersion.txt"); //development
            var verCall = HttpCallUtil.HttpGetCall(@"https://raw.githubusercontent.com/jankcat/rapidreporterplusplus/master/currentVersion.txt"); //master
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
