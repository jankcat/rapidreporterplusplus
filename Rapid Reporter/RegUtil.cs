using Microsoft.Win32;

namespace Rapid_Reporter
{
    internal static class RegUtil
    {
        internal const string RegBgColor = "BgColor";

        internal static void InitReg()
        {
            Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("RapidReporterPP");
        }

        internal static void CreateRegKey(string name, string value)
        {
            var subKey = Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("RapidReporterPP");
            if (subKey == null)
                return;
            subKey.SetValue(name, value);
            subKey.Close();
        }

        internal static string ReadRegKey(string name)
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
    }
}