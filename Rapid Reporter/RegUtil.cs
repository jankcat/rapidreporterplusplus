using System;
using System.Globalization;
using Microsoft.Win32;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Rapid_Reporter
{
    internal static class RegUtil
    {
        internal static void InitReg()
        {
            Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("RapidReporterPP");
        }

        private static void CreateRegKey(string name, string value)
        {
            var subKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("RapidReporterPP");
            if (subKey == null)
                return;
            subKey.SetValue(name, value);
            subKey.Close();
        }

        private static string ReadRegKey(string name)
        {
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("RapidReporterPP");
            if (registryKey == null)
                return "";
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

        internal static int ScreenShotPreviewX
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

        internal static int ScreenShotPreviewY
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

        internal static bool ScreenShotPreviewEnabled
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

        internal static bool CheckForUpdates
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

        internal static Color BackgroundColor
        {
            get
            {
                var str = ReadRegKey("BgColor");
                if (string.IsNullOrWhiteSpace(str)) return Color.FromArgb(byte.MaxValue, 0, 104, byte.MaxValue);
                var val = ColorConverter.ConvertFromString(str);
                if (val == null) return Color.FromArgb(byte.MaxValue, 0, 104, byte.MaxValue);
                if (val.GetType() != typeof(Color)) return Color.FromArgb(byte.MaxValue, 0, 104, byte.MaxValue);
                return (Color) val;
            }
            set
            {
                CreateRegKey("BgColor", value.ToString());
            }
        }

        internal static double Transparency
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

        internal static Version UpdateToSkip
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