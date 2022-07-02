using L4d2_Mod_Manager.Module.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Arthitecture
{
    /// <summary>
    /// 基础设施 - Vtf转换器，将Vtf转为jpg
    /// </summary>
    public class NoVtfConverter
    {
        private static string NoVtfExecutablePath => SettingFP.GetSetting().NoVtfExecutablePath;

        /// <summary>
        /// 将Vtf文件转换为普通图片文件
        /// </summary>
        public static string ConverVtf(string file)
        {
            string res = RunProcess.Run(NoVtfExecutablePath, $"-l jpg --compress \"{Path.GetDirectoryName(file)}\"");
            return Path.ChangeExtension(file, ".jpg");
        }
    }
}
