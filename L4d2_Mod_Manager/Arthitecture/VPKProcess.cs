using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using L4d2_Mod_Manager.Utility;
using L4d2_Mod_Manager.Arthitecture;
using L4d2_Mod_Manager.Module.Settings;

namespace L4d2_Mod_Manager.Arthitecture
{
    class VPKProcess
    {
        private static string VPKPath => SettingFP.GetSetting().VPKExecutablePath;

        public static void ExtractFile(string vpk, string targetDir, params string[] file)
        {
            string arg = $"x \"{vpk}\" " + string.Join(' ', file.Select(x => $"{x}"));
            RunProcess.Run(VPKPath, targetDir, arg);
        }

        public static string[] ListFile(string vpk)
        {
            string arg = $"l \"{vpk}\" ";
            string res = RunProcess.Run(VPKPath, arg);
            //过滤掉末尾多余的东西
            return res.Split("\r\n").Where(str => !str.EndsWith(')')).ToArray();
        }
    }
}
