using System.Collections.Generic;

namespace L4d2_Mod_Manager.Module.Settings
{
    /// <summary>
    /// 软件设置信息
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// 模组文件夹
        /// </summary>
        public List<string> modFileFolder;

        /// <summary>
        /// no_vtf可执行程序
        /// </summary>
        public string NoVtfExecutablePath;

        /// <summary>
        /// vpk可执行程序
        /// </summary>
        public string VPKExecutablePath;
    }
}
