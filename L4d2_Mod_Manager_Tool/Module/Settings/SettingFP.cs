using Newtonsoft.Json;
using System.IO;

namespace L4d2_Mod_Manager_Tool.Module.Settings
{
    public static class SettingFP
    {
        public const string SettingFile = "setting.json";
        public static Setting GetSetting()
        {
            if (File.Exists(SettingFile))
                return JsonConvert.DeserializeObject<Setting>(File.ReadAllText(SettingFile));
            else
                return new() { 
                    GamePath = ""
                    , NoVtfExecutablePath = "no_vtf-windows_x64\\no_vtf.exe"
                };
        }

        public static void SaveSetting(Setting setting)
        {
            var content = JsonConvert.SerializeObject(setting);
            File.WriteAllText(SettingFile, content);
        }
    }
}
