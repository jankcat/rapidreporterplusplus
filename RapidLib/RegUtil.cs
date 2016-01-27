using System;
using System.Drawing;
using System.Globalization;
using Microsoft.Win32;

namespace RapidLib
{
    
    public static class RegUtil
    {
        public static void InitReg()
        {
            var registryKey = Registry.CurrentUser.CreateSubKey("Software");
            if (registryKey != null)
                registryKey.CreateSubKey("RapidReporterPP");
        }

        private static void CreateRegKey(string name, string value)
        {
            var registryKey = Registry.CurrentUser.CreateSubKey("Software");
            if (registryKey == null) return;
            var subKey = registryKey.CreateSubKey("RapidReporterPP");
            if (subKey == null) return;
            subKey.SetValue(name, value);
            subKey.Close();
        }

        private static string ReadRegKey(string name)
        {
            var openSubKey = Registry.CurrentUser.OpenSubKey("Software");
            if (openSubKey == null) return null;
            var registryKey = openSubKey.OpenSubKey("RapidReporterPP");
            if (registryKey == null) return "";
            try
            {
                var str = registryKey.GetValue(name).ToString();
                registryKey.Close();
                return str;
            }
            catch
            {
                return null;
            }
        }

        public static int ScreenShotPreviewX
        {
            get
            {
                var str = ReadRegKey("ScreenShotPreviewX");
                if (string.IsNullOrWhiteSpace(str)) return -99999999;
                int val;
                var succcess = int.TryParse(str, out val);
                return succcess ? val : -99999999;
            }
            set
            {
                CreateRegKey("ScreenShotPreviewX", value.ToString());
            }
        }

        public static int ScreenShotPreviewY
        {
            get
            {
                var str = ReadRegKey("ScreenShotPreviewY");
                if (string.IsNullOrWhiteSpace(str)) return -99999999;
                int val;
                var succcess = int.TryParse(str, out val);
                return succcess ? val : -99999999;
            }
            set
            {
                CreateRegKey("ScreenShotPreviewY", value.ToString());
            }
        }

        public static bool ScreenShotPreviewEnabled
        {
            get
            {
                var str = ReadRegKey("ScreenShotPreviewEnabled");
                if (string.IsNullOrWhiteSpace(str)) return false;
                bool val;
                var succcess = bool.TryParse(str, out val);
                return succcess && val; // false if failed
            }
            set
            {
                CreateRegKey("ScreenShotPreviewEnabled", value.ToString());
            }
        }

        public static bool CheckForUpdates
        {
            get
            {
                var str = ReadRegKey("CheckForUpdates");
                if (string.IsNullOrWhiteSpace(str)) return true;
                bool val;
                var succcess = bool.TryParse(str, out val);
                return !succcess || val; // true if failed
            }
            set
            {
                CreateRegKey("CheckForUpdates", value.ToString());
            }
        }

        public static Color BackgroundColor
        {
            get
            {
                var str = ReadRegKey("BgColor");
                if (string.IsNullOrWhiteSpace(str)) return Color.FromArgb(byte.MaxValue, 0, 104, byte.MaxValue);
                var val = ColorTranslator.FromHtml(str);
                return val;
            }
            set
            {
                CreateRegKey("BgColor", ColorTranslator.ToHtml(value));
            }
        }

        public static double Transparency
        {
            get
            {
                var str = ReadRegKey("Transparency");
                if (string.IsNullOrWhiteSpace(str)) return 1.0;
                double val;
                var succcess = double.TryParse(str, out val);
                return succcess ? val : 1.0;
            }
            set
            {
                CreateRegKey("Transparency", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public static Version UpdateToSkip
        {
            get
            {
                var str = ReadRegKey("UpdateToSkip");
                if (string.IsNullOrWhiteSpace(str)) return new Version(0, 0, 0, 0);
                try
                {
                    return new Version(str);
                }
                catch
                {
                    return new Version(0, 0, 0, 0);
                }
            }
            set
            {
                CreateRegKey("UpdateToSkip", value.ToString());
            }
        }
    }
}