using Domain.Settings;
using System.IO;

namespace L4d2_Mod_Manager_Tool.Arthitecture
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
            string res = RunProcess.Run(NoVtfExecutablePath, $"-l png --compress \"{Path.GetDirectoryName(file)}\"");
            return Path.ChangeExtension(file, ".png");
        }
    }
}
