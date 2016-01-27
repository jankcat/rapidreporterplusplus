using System;
using System.Diagnostics;
using System.Reflection;
using RapidLib.Forms;
using RapidLib.HTTP;

namespace RapidLib
{
    public static class Updater
    {
        public static void ManualCheckVersion()
        {
            var parsedVersion = GetServerVersion();
            if (parsedVersion == null)
            {
                RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                MessageBoxForm.Alert("Unable to retrieve latest version number from GitHub.", "Updater");
                return;
            }
            if (UpToDateWithLatest(parsedVersion))
            {
                RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                MessageBoxForm.Alert("You are currently up to date!\r\nWe will let you know if a new version comes down the pipeline.", "Updater");
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
                    Process.Start("https://github.com/jankcat/rapidreporterplusplus/releases");
                    return;
                case UpdateChosen.Skip:
                    RegUtil.UpdateToSkip = parsedVersion;
                    return;
                case UpdateChosen.Later:
                    RegUtil.UpdateToSkip = new Version(0, 0, 0, 0);
                    return;
            }
        }

        public static void CheckVersion()
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
            var verCall = HttpCallUtil.HttpGetCall(@"https://raw.githubusercontent.com/jankcat/rapidreporterplusplus/master/currentVersion.txt");
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
