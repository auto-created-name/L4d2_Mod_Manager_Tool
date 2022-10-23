using Newtonsoft.Json;
using System.IO;

namespace Domain.Settings
{
    public static class SettingFP
    {
        public const string SettingFile = "setting.json";
        private static Setting setting = null;
        public static Setting GetSetting()
        {
            if (setting == null)
            {
                if (File.Exists(SettingFile))
                    setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(SettingFile));
                else
                    setting = new()
                    {
                        GamePath = "",
                        NoVtfExecutablePath = "no_vtf-windows_x64\\no_vtf.exe"
                    };
            }
            return setting;
        }

        public static void SaveSetting(Setting setting)
        {
            var content = JsonConvert.SerializeObject(setting);
            File.WriteAllText(SettingFile, content);
            SettingFP.setting = setting;
        }
    }
}
